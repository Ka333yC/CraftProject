using System;
using System.Linq;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using Extensions;
using ChunkCore;
using PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using Assets.Scripts.Core.MeshCreation;
using Assets.Scripts.Core.ChunkPhysicsCore.ChunkPhysicsScripts.Components.Elements.BlockPhysicsGetters;
using Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements;
using Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements.BlockGraphicsGetters;

namespace PhysicsCore.ChunkPhysicsCore.LifeTimeControl
{
	public class ColliderMeshPartsContainer : IDisposable
	{
		private BlocksPhysicsGetter _blocksPhysicsGetter;
		private readonly MeshPart[,,] _meshParts;
		private readonly object _lock = new object();

		public ColliderMeshPartsContainer(BlocksPhysicsGetter blocksPhysicsGetter)
		{
			_blocksPhysicsGetter = blocksPhysicsGetter;
			_meshParts = ChunkSizeArrayPool<MeshPart>.Shared.Rent();
		}

		public void Dispose()
		{
			ChunkSizeArrayPool<MeshPart>.Shared.Return(_meshParts);
		}

		public void UpdateAllMeshes(CancellationToken token)
		{
			var from = new Vector3Int(0, 0, 0);
			var to = ChunkConstantData.ChunkScale;
			using CachedBlocksPhysicsGetter cachedBlocksPhysicsGetter =
				new CachedBlocksPhysicsGetter(_blocksPhysicsGetter);
			cachedBlocksPhysicsGetter.CacheBlocksPhysics(from, to);
			UpdateMeshes(from, to, cachedBlocksPhysicsGetter, token);
		}

		public void UpdateWallMeshes(Face wall, CancellationToken token)
		{
			Vector3Int updateMeshesFrom = new Vector3Int();
			Vector3Int updateMeshesTo = new Vector3Int();
			Vector3Int cacheBlockGraphicsFrom = new Vector3Int();
			Vector3Int cacheBlockGraphicsTo = new Vector3Int();
			GetWallBounds(wall, out updateMeshesFrom, out updateMeshesTo,
				out cacheBlockGraphicsFrom, out cacheBlockGraphicsTo);
			using CachedBlocksPhysicsGetter cachedBlocksPhysicsGetter =
				new CachedBlocksPhysicsGetter(_blocksPhysicsGetter);
			cachedBlocksPhysicsGetter.CacheBlocksPhysics(cacheBlockGraphicsFrom, cacheBlockGraphicsTo);
			UpdateMeshes(updateMeshesFrom, updateMeshesTo, cachedBlocksPhysicsGetter, token);
		}

		public IEnumerable<MeshPart> GetMeshParts()
		{
			foreach(var meshPart in _meshParts)
			{
				if(meshPart != null)
				{
					yield return meshPart;
				}
			}

			yield break;
		}

		public void UpdateMesh(Vector3Int position)
		{
			lock(_lock)
			{
				_meshParts[position.x, position.y, position.z] = CalculateMeshPart(_blocksPhysicsGetter, position);
			}
		}

		private void UpdateMeshes(Vector3Int from, Vector3Int to, IBlocksPhysicsGetter blocksPhysicsGetter,
			CancellationToken token)
		{
			for(int y = from.y; y < to.y; y++)
			{
				token.ThrowIfCancellationRequested();
				for(int x = from.x; x < to.x; x++)
				{
					for(int z = from.z; z < to.z; z++)
					{
						UpdateMesh(blocksPhysicsGetter, new Vector3Int(x, y, z));
					}
				}
			}
		}

		private void UpdateMesh(IBlocksPhysicsGetter blocksPhysicsGetter, Vector3Int position)
		{
			lock(_lock)
			{
				_meshParts[position.x, position.y, position.z] =
					CalculateMeshPart(blocksPhysicsGetter, position);
			}
		}

		private MeshPart CalculateMeshPart(IBlocksPhysicsGetter blocksPhysicsGetter, Vector3Int blockPosition)
		{
			IPhysicsBlockComponent blockPhysics = blocksPhysicsGetter.GetBlockPhysics(blockPosition);
			if(blockPhysics == null)
			{
				return null;
			}

			Face facesToSimulatePhysics = GetPhysicsFaces(blocksPhysicsGetter, blockPhysics, blockPosition);
			return CreateMeshPart(blockPhysics, blockPosition,
				facesToSimulatePhysics);
		}

		private Face GetPhysicsFaces(IBlocksPhysicsGetter blocksPhysicsGetter, 
			IPhysicsBlockComponent blockPhysics, Vector3Int blockPosition)
		{
			Face facesToSimulatePhysics = Face.None;
			for(byte faceIndex = 0; faceIndex < 6; faceIndex++)
			{
				Face face = (Face)MathfExtensions.PositivePowOfTwo(faceIndex);
				// Если блок прозрачен, то точно отрисовываем или IsFaceDrawable
				bool isPhysical = !blockPhysics.IsFull(face) ||
					IsFacePhysical(blocksPhysicsGetter, blockPosition, face);
				if(isPhysical)
				{
					facesToSimulatePhysics.AddFace(face);
				}
			}

			return facesToSimulatePhysics;
		}

		private bool IsFacePhysical(IBlocksPhysicsGetter blocksPhysicsGetter, Vector3Int blockPosition, Face face)
		{
			Vector3Int borderingBlockPosition = blockPosition + face.ToVector();
			// Если выше чанка или ниже чанка, то всегда прозрачно
			if(!ChunkConstantData.IsPositionInChunkByY(borderingBlockPosition))
			{
				return true;
			}

			IPhysicsBlockComponent borderingBlockPhysics;
			if(ChunkConstantData.IsPositionInChunkByX(borderingBlockPosition) &&
				ChunkConstantData.IsPositionInChunkByZ(borderingBlockPosition))
			{
				// Если внутри текущего чанка
				borderingBlockPhysics = blocksPhysicsGetter.GetBlockPhysics(borderingBlockPosition);
			}
			else
			{
				blocksPhysicsGetter = _blocksPhysicsGetter.GetBorderingGetter(face);
				// Если blocksGraphicsContainer равен null, то чанк отсутствует
				if(blocksPhysicsGetter == null)
				{
					return false;
				}

				// Так как позиция вышла за границы чанка, приводим её к позиции в чанке
				borderingBlockPosition = ChunkConstantData.WorldToBlockPositionInChunk(borderingBlockPosition);
				borderingBlockPhysics = blocksPhysicsGetter.GetBlockPhysics(borderingBlockPosition);
			}

			Face borderingBlockFace = face.Reverse();
			return borderingBlockPhysics == null || !borderingBlockPhysics.IsFull(borderingBlockFace);
		}

		private MeshPart CreateMeshPart(IPhysicsBlockComponent blockPhysics, Vector3Int blockPosition,
			Face facesToSimulatePhysics)
		{
			if(facesToSimulatePhysics == Face.None)
			{
				return null;
			}

			MeshPart meshPart = new MeshPart(blockPosition);
			for(int i = 0; i < 6; i++)
			{
				// Если сторона отсутствует в facesToDraw, то пропускаем её
				Face face = (Face)MathfExtensions.PositivePowOfTwo(i);
				if(!facesToSimulatePhysics.HasFace(face))
				{
					continue;
				}

				meshPart.AddMeshDataPart(blockPhysics.GetMeshDataPart(face));
			}

			return meshPart;
		}

		private void GetWallBounds(Face wall, out Vector3Int updateMeshesFrom,
			out Vector3Int updateMeshesTo, out Vector3Int cacheBlockPhysicsFrom,
			out Vector3Int cacheBlockPhysicsTo)
		{
			if(wall == Face.Forward)
			{
				updateMeshesFrom = new Vector3Int(0, 0, ChunkConstantData.ChunkScale.z - 1);
				updateMeshesTo = ChunkConstantData.ChunkScale;
				cacheBlockPhysicsFrom = new Vector3Int(0, 0, ChunkConstantData.ChunkScale.z - 2);
				cacheBlockPhysicsTo = ChunkConstantData.ChunkScale;
			}
			else if(wall == Face.Back)
			{
				updateMeshesFrom = new Vector3Int(0, 0, 0);
				updateMeshesTo = new Vector3Int(ChunkConstantData.ChunkScale.x, ChunkConstantData.ChunkScale.y, 1);
				cacheBlockPhysicsFrom = new Vector3Int(0, 0, 0);
				cacheBlockPhysicsTo =
					new Vector3Int(ChunkConstantData.ChunkScale.x, ChunkConstantData.ChunkScale.y, 2);
			}
			else if(wall == Face.Right)
			{
				updateMeshesFrom = new Vector3Int(ChunkConstantData.ChunkScale.x - 1, 0, 0);
				updateMeshesTo = ChunkConstantData.ChunkScale;
				cacheBlockPhysicsFrom = new Vector3Int(ChunkConstantData.ChunkScale.x - 2, 0, 0);
				cacheBlockPhysicsTo = ChunkConstantData.ChunkScale;
			}
			else if(wall == Face.Left)
			{
				updateMeshesFrom = new Vector3Int(0, 0, 0);
				updateMeshesTo = new Vector3Int(1, ChunkConstantData.ChunkScale.y, ChunkConstantData.ChunkScale.z);
				cacheBlockPhysicsFrom = new Vector3Int(0, 0, 0);
				cacheBlockPhysicsTo =
					new Vector3Int(2, ChunkConstantData.ChunkScale.y, ChunkConstantData.ChunkScale.z);
			}
			else
			{
				throw new ArgumentException($"Unknown wall face {wall}");
			}
		}
	}
}
