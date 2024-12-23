using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.ChunksContainerScripts;
using ChunkCore;
using Extensions;
using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using PhysicsCore.ObjectPhysics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Components;
using Assets.Scripts.Apart.Extensions;
using Assets.Scripts.Core.ObjectPhysicsCore.InBlockCheck.Components;

namespace Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Systems
{
	public class ObjectPhysicsInBlockCheckerSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private ChunksContainer _chunksContainer;

		private EcsPool<ChunkComponent> _chunkPool;
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<IntersectionWithBlockBoundsComponent> _intersectionWithBlockBoundsPool;
		private EcsPool<CheckIsObjectPhysicsInBlockTag> _needCheckIsObjectPhysicsInBlockPool;
		private EcsPool<ObjectPhysicsInBlockTag> _objectPhysicsInBlockPool;
		private EcsFilter _objectPhysicsToCheckFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunkPool = world.GetPool<ChunkComponent>();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_intersectionWithBlockBoundsPool = world.GetPool<IntersectionWithBlockBoundsComponent>();
			_needCheckIsObjectPhysicsInBlockPool = world.GetPool<CheckIsObjectPhysicsInBlockTag>();
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
				ref var objectPhysics = ref _objectPhysicsPool.Get(objectPhysicsEntity);
				bool isIntersectsWithBlock = IsObjectPhysicsIntersectsWithBlockPhysics(objectPhysicsEntity);
				if(isIntersectsWithBlock && !_objectPhysicsInBlockPool.Has(objectPhysicsEntity))
				{
					_objectPhysicsInBlockPool.Add(objectPhysicsEntity);
				}
				else if(!isIntersectsWithBlock && _objectPhysicsInBlockPool.Has(objectPhysicsEntity))
				{
					_objectPhysicsInBlockPool.Del(objectPhysicsEntity);
				}

				_needCheckIsObjectPhysicsInBlockPool.Del(objectPhysicsEntity);
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
			if(!_chunksContainer.TryGetChunk(gridPosition, out int chunkEntity))
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

			throw new Exception($"{typeof(ChunksContainerComponent).Name} not found");
		}
	}
}
