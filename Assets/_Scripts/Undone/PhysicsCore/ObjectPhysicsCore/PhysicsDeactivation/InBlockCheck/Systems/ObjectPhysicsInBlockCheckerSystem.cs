using System;
using _Scripts.Apart.Extensions;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.Extensions;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.InBlockCheck.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.InBlockCheck.Systems
{
	public class ObjectPhysicsInBlockCheckerSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<IntersectionWithBlockBoundsComponent> _intersectionWithBlockBoundsPool;
		private EcsPool<CheckIsObjectPhysicsInBlockTag> _checkIsObjectPhysicsInBlockPool;
		private EcsPool<ObjectPhysicsInBlockTag> _objectPhysicsInBlockPool;
		private EcsFilter _objectPhysicsToCheckFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_intersectionWithBlockBoundsPool = world.GetPool<IntersectionWithBlockBoundsComponent>();
			_checkIsObjectPhysicsInBlockPool = world.GetPool<CheckIsObjectPhysicsInBlockTag>();
			_objectPhysicsInBlockPool = world.GetPool<ObjectPhysicsInBlockTag>();
			_objectPhysicsToCheckFilter = world
				.Filter<ObjectPhysicsComponent>()
				.Inc<IntersectionWithBlockBoundsComponent>()
				.Inc<CheckIsObjectPhysicsInBlockTag>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var objectPhysicsEntity in _objectPhysicsToCheckFilter)
			{
				bool isIntersectsWithBlock = IsObjectPhysicsIntersectsWithBlockPhysics(objectPhysicsEntity);
				if(isIntersectsWithBlock && !_objectPhysicsInBlockPool.Has(objectPhysicsEntity))
				{
					_objectPhysicsInBlockPool.Add(objectPhysicsEntity);
				}
				else if(!isIntersectsWithBlock && _objectPhysicsInBlockPool.Has(objectPhysicsEntity))
				{
					_objectPhysicsInBlockPool.Del(objectPhysicsEntity);
				}

				_checkIsObjectPhysicsInBlockPool.Del(objectPhysicsEntity);
			}
		}

		private bool IsObjectPhysicsIntersectsWithBlockPhysics(int objectPhysicsEntity)
		{
			var objectPhysicsBlockBounds =
					_intersectionWithBlockBoundsPool.Get(objectPhysicsEntity);
			var inBlockBounds = objectPhysicsBlockBounds.BlockBounds;
			var roundedBlockBounds = objectPhysicsBlockBounds.RoundedBlockBounds;
			foreach(var blockPosition in roundedBlockBounds.GetPositions())
			{
				if(IsBoundsIntersectsWithBlockPhysics(inBlockBounds, blockPosition))
				{
					return true;
				}
			}

			return false;
		}

		private bool IsBoundsIntersectsWithBlockPhysics(Bounds inBlockBounds, Vector3Int blockWorldPosition)
		{
			var gridPosition = ChunkConstantData.WorldToGridPosition(blockWorldPosition);
			var blockPositionInChunk = ChunkConstantData.WorldToBlockPositionInChunk(blockWorldPosition);
			if(blockPositionInChunk.y >= ChunkConstantData.ChunkScale.y ||
			   blockPositionInChunk.y < 0 ||
			   !_chunksContainer.TryGetChunk(gridPosition, out int chunkEntity))
			{
				return false;
			}

			var blocks = _chunkPool.Get(chunkEntity).Blocks;
			if(!blocks[blockPositionInChunk].TryGetComponent(out IPhysicsBlockComponent blockPhysics))
			{
				return false;
			}

			return IsBoundsIntersectsWithBlockPhysicsFullFaces(inBlockBounds, blockWorldPosition, blockPhysics);
		}

		private bool IsBoundsIntersectsWithBlockPhysicsFullFaces(Bounds inBlockBounds,
			Vector3Int blockWorldPosition, IPhysicsBlockComponent blockPhysics)
		{
			for(int i = 0; i < 6; i++)
			{
				// Проходимся по всем сторонам блока и проверяем физичен ли блок с этой стороны и
				// пересекаются ли границы objectPhysics с этой стороной
				Face face = (Face)MathfExtensions.PositivePowOfTwo(i);
				if(blockPhysics.IsFull(face) &&
					inBlockBounds.Intersects(GetBlockFaceBounds(blockWorldPosition, face)))
				{
					return true;
				}
			}

			return false;
		}

		private Bounds GetBlockFaceBounds(Vector3Int block, Face face)
		{
			Bounds blockFaceBounds = face.ToBounds();
			blockFaceBounds.SetMinMax(block + blockFaceBounds.min, block + blockFaceBounds.max);
			return blockFaceBounds;
		}

		private ChunksContainer GetChunksContainer(EcsWorld world)
		{
			var pool = world.GetPool<ChunksContainerComponent>();
			var filter = world
				.Filter<ChunksContainerComponent>()
				.End();
			foreach(var entity in filter)
			{
				return pool.Get(entity).ChunksContainer;
			}

			throw new Exception($"{nameof(ChunksContainerComponent)} not found");
		}
	}
}
