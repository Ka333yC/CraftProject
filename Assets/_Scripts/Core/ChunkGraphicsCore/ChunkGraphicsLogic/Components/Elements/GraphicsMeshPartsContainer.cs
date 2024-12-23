using Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements.BlockGraphicsGetters;
using Assets.Scripts.Core.MeshCreation;
using ChunkCore;
using Extensions;
using GraphicsCore.ChunkGraphicsCore.BlockGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkGraphicsCore.ChunkGraphicsScripts.Components.Elements
{
	public class GraphicsMeshPartsContainer : IDisposable
	{
		private readonly object _lock = new object();
		private readonly BlocksGraphicsGetter _blocksGraphicsGetter;
		private readonly MeshPart[,,] _meshParts;

		public GraphicsMeshPartsContainer(BlocksGraphicsGetter blocksGraphicsGetter)
		{
			_blocksGraphicsGetter = blocksGraphicsGetter;
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
			using CachedBlocksGraphicsGetter cachedBlocksGraphicsGetter =
				new CachedBlocksGraphicsGetter(_blocksGraphicsGetter);
			cachedBlocksGraphicsGetter.CacheBlocksGraphics(from, to);
			UpdateMeshes(from, to, cachedBlocksGraphicsGetter, token);
		}

		public void UpdateWallMeshes(Face wall, CancellationToken token)
		{
			Vector3Int updateMeshesFrom = new Vector3Int();
			Vector3Int updateMeshesTo = new Vector3Int();
			Vector3Int cacheBlockGraphicsFrom = new Vector3Int();
			Vector3Int cacheBlockGraphicsTo = new Vector3Int();
			GetWallBounds(wall, out updateMeshesFrom, out updateMeshesTo,
				out cacheBlockGraphicsFrom, out cacheBlockGraphicsTo);
			using CachedBlocksGraphicsGetter cachedBlocksGraphicsGetter =
				new CachedBlocksGraphicsGetter(_blocksGraphicsGetter);
			cachedBlocksGraphicsGetter.CacheBlocksGraphics(cacheBlockGraphicsFrom, cacheBlockGraphicsTo);
			UpdateMeshes(updateMeshesFrom, updateMeshesTo, cachedBlocksGraphicsGetter, token);
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
				_meshParts[position.x, position.y, position.z] = CalculateMeshPart(_blocksGraphicsGetter, position);
			}
		}

		private void UpdateMeshes(Vector3Int from, Vector3Int to, IBlocksGraphicsGetter blocksGraphicsGetter, 
			CancellationToken token)
		{
			for(int y = from.y; y < to.y; y++)
			{
				token.ThrowIfCancellationRequested();
				for(int x = from.x; x < to.x; x++)
				{
					for(int z = from.z; z < to.z; z++)
					{
						UpdateMesh(blocksGraphicsGetter, new Vector3Int(x, y, z));
					}
				}
			}
		}

		private void UpdateMesh(IBlocksGraphicsGetter blocksGraphicsGetter, Vector3Int position)
		{
			lock(_lock)
			{
				_meshParts[position.x, position.y, position.z] = 
					CalculateMeshPart(blocksGraphicsGetter, position);
			}
		}

		private MeshPart CalculateMeshPart(IBlocksGraphicsGetter blocksGraphicsGetter, Vector3Int blockPosition)
		{
			IGraphicsBlockComponent blockGraphics = blocksGraphicsGetter.GetBlockGraphics(blockPosition);
			if(blockGraphics == null)
			{
				return null;
			}

			Face facesToDraw = GetDrawingFaces(blocksGraphicsGetter, blockGraphics, blockPosition);
			return CreateMeshPart(blockGraphics, blockPosition,
				 facesToDraw);
		}

		private Face GetDrawingFaces(IBlocksGraphicsGetter blocksGraphicsGetter, 
			IGraphicsBlockComponent blockGraphics, Vector3Int blockPosition)
		{
			Face facesToDraw = Face.None;
			for(byte faceIndex = 0; faceIndex < 6; faceIndex++)
			{
				Face face = (Face)MathfExtensions.PositivePowOfTwo(faceIndex);
				// Отрисовываем если текущий блок(сторона) прозрачен или если соседний блок(сторона) прозрачен
				bool isDrawable = blockGraphics.IsTransparent(face) ||
					IsFaceDrawable(blocksGraphicsGetter, blockPosition, face);
				if(isDrawable)
				{
					facesToDraw.AddFace(face);
				}
			}

			return facesToDraw;
		}

		private bool IsFaceDrawable(IBlocksGraphicsGetter blocksGraphicsGetter, Vector3Int blockPosition, Face face)
		{
			Vector3Int borderingBlockPosition = blockPosition + face.ToVector();
			// Если выше чанка или ниже чанка, то всегда прозрачно
			if(!ChunkConstantData.IsPositionInChunkByY(borderingBlockPosition))
			{
				return true;
			}

			IGraphicsBlockComponent borderingBlockGraphics;
			if(ChunkConstantData.IsPositionInChunkByX(borderingBlockPosition) &&
				ChunkConstantData.IsPositionInChunkByZ(borderingBlockPosition))
			{
				// Если внутри текущего чанка
				borderingBlockGraphics = blocksGraphicsGetter.GetBlockGraphics(borderingBlockPosition);
			}
			else
			{
				blocksGraphicsGetter = _blocksGraphicsGetter.GetBorderingGetter(face);
				// Если blocksGraphicsContainer равен null, то чанк отсутствует
				if(blocksGraphicsGetter == null)
				{
					return false;
				}

				// Так как позиция вышла за границы чанка, приводим её к позиции в чанке
				borderingBlockPosition = ChunkConstantData.WorldToBlockPositionInChunk(borderingBlockPosition);
				borderingBlockGraphics = blocksGraphicsGetter.GetBlockGraphics(borderingBlockPosition);
			}

			Face borderingBlockFace = face.Reverse();
			return borderingBlockGraphics == null || borderingBlockGraphics.IsTransparent(borderingBlockFace);
		}

		private MeshPart CreateMeshPart(IGraphicsBlockComponent blockGraphics, Vector3Int blockPosition, Face facesToDraw)
		{
			if(facesToDraw == Face.None)
			{
				return null;
			}

			MeshPart meshPart = new MeshPart(blockPosition);
			for(int i = 0; i < 6; i++)
			{
				// Если сторона отсутствует в facesToDraw, то пропускаем её
				Face face = (Face)MathfExtensions.PositivePowOfTwo(i);
				if(!facesToDraw.HasFace(face))
				{
					continue;
				}

				meshPart.AddMeshDataPart(blockGraphics.GetMeshDataPart(face));
				meshPart.AddUV(blockGraphics.GetUV(face));
			}

			return meshPart;
		}

		private void GetWallBounds(Face wall, out Vector3Int updateMeshesFrom, 
			out Vector3Int updateMeshesTo, out Vector3Int cacheBlockGraphicsFrom, 
			out Vector3Int cacheBlockGraphicsTo)
		{
			if(wall == Face.Forward)
			{
				updateMeshesFrom = new Vector3Int(0, 0, ChunkConstantData.ChunkScale.z - 1);
				updateMeshesTo = ChunkConstantData.ChunkScale;
				cacheBlockGraphicsFrom = new Vector3Int(0, 0, ChunkConstantData.ChunkScale.z - 2);
				cacheBlockGraphicsTo = ChunkConstantData.ChunkScale;
			}
			else if(wall == Face.Back)
			{
				updateMeshesFrom = new Vector3Int(0, 0, 0);
				updateMeshesTo = new Vector3Int(ChunkConstantData.ChunkScale.x, ChunkConstantData.ChunkScale.y, 1);
				cacheBlockGraphicsFrom = new Vector3Int(0, 0, 0);
				cacheBlockGraphicsTo =
					new Vector3Int(ChunkConstantData.ChunkScale.x, ChunkConstantData.ChunkScale.y, 2);
			}
			else if(wall == Face.Right)
			{
				updateMeshesFrom = new Vector3Int(ChunkConstantData.ChunkScale.x - 1, 0, 0);
				updateMeshesTo = ChunkConstantData.ChunkScale;
				cacheBlockGraphicsFrom = new Vector3Int(ChunkConstantData.ChunkScale.x - 2, 0, 0);
				cacheBlockGraphicsTo = ChunkConstantData.ChunkScale;
			}
			else if(wall == Face.Left)
			{
				updateMeshesFrom = new Vector3Int(0, 0, 0);
				updateMeshesTo = new Vector3Int(1, ChunkConstantData.ChunkScale.y, ChunkConstantData.ChunkScale.z);
				cacheBlockGraphicsFrom = new Vector3Int(0, 0, 0);
				cacheBlockGraphicsTo =
					new Vector3Int(2, ChunkConstantData.ChunkScale.y, ChunkConstantData.ChunkScale.z);
			}
			else
			{
				throw new ArgumentException($"Unknown wall face {wall}");
			}
		}
	}
}
