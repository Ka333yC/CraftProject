using System;
using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkCore.BlockChanging.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using _Scripts.Core.Extensions;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Implementation.InputImplementation.Components;
using _Scripts.Implementation.InventoryImplementation.Block;
using _Scripts.Implementation.PlayerImplementation.Input.Components;
using _Scripts.Implementation.PlayerImplementation.PlayerInventory.Components;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace _Scripts.Implementation.PlayerImplementation.Input.Systems
{
	public class PlayerInteractionProcessSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		private const int HitDelay = 300;
		
		[Inject]
		private BlocksArchetype _blocksContainers;
		
		private ChunksContainer _chunksContainer;
		
		private EcsWorld _world;
		private EcsPool<ChunkComponent> _chunkDataPool;
		private EcsPool<ChunkInitializedTag> _chunkInitializedPool;
		private EcsPool<PlayerInventoryComponent> _playerInventoryPool;
		private EcsPool<PlayerComponent> _playerPool;
		private EcsPool<PlayerInteractionComponent> _playerInteractionPool;
		private EcsPool<HoldInputComponent> _holdInputPool;
		private EcsPool<TapInputComponent> _tapInputPool;
		private EcsFilter _holdInputFilter;
		private EcsFilter _tapInputFilter;
		private EcsFilter _playersToInitializeFilter;
		private EcsFilter _playersFilter;
		
		public void PreInit(IEcsSystems systems)
		{
			_world = systems.GetWorld();
			_chunkDataPool = _world.GetPool<ChunkComponent>();
			_chunkInitializedPool = _world.GetPool<ChunkInitializedTag>();
			_playerInventoryPool = _world.GetPool<PlayerInventoryComponent>();
			_playerPool = _world.GetPool<PlayerComponent>();
			_playerInteractionPool = _world.GetPool<PlayerInteractionComponent>();
			_holdInputPool = _world.GetPool<HoldInputComponent>();
			_tapInputPool = _world.GetPool<TapInputComponent>();
			_holdInputFilter = _world
				.Filter<HoldInputComponent>()
				.End();
			_tapInputFilter = _world
				.Filter<TapInputComponent>()
				.End();
			_playersToInitializeFilter = _world
				.Filter<PlayerComponent>()
				.Inc<PlayerInventoryComponent>()
				.Exc<PlayerInteractionComponent>()
				.End();
			_playersFilter = _world
				.Filter<PlayerComponent>()
				.Inc<PlayerInventoryComponent>()
				.Inc<PlayerInteractionComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_chunksContainer = GetChunksContainer(world);
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var playerEntity in _playersToInitializeFilter)
			{
				ref var playerInteraction = ref _playerInteractionPool.Add(playerEntity);
				playerInteraction.HitDelayTask = UniTask.CompletedTask;
			}
			
			foreach(var inputEntity in _holdInputFilter)
			{
				ref var holdInput = ref _holdInputPool.Get(inputEntity);
				HandleHoldInput(holdInput.ScreenPointerPositionInput);
			}
			
			foreach(var inputEntity in _tapInputFilter)
			{
				ref var tapInput = ref _tapInputPool.Get(inputEntity);
				HandleTapInput(tapInput.ScreenPointerPositionInput);
			}
		}

		private void HandleHoldInput(Vector2 screenPointerPosition)
		{
			foreach(var playerEntity in _playersFilter)
			{
				ref var playerInteraction = ref _playerInteractionPool.Get(playerEntity);
				if(playerInteraction.HitDelayTask.Status == UniTaskStatus.Pending)
				{
					continue;
				}
				
				var camera = _playerPool.Get(playerEntity).Camera;
				var ray = GetRayTowardPointer(camera, screenPointerPosition);
				var blockPositions = BlocksOnDirectionCalculator.AllBlockPositionsOnLine(ray, 5);
				Vector3Int? blockPositionOnDirection = null;
				foreach(var blockPosition in blockPositions)
				{
					if(GetBlock(blockPosition) != _blocksContainers.Air)
					{
						blockPositionOnDirection = blockPosition;
						break;
					}
				}

				if(!blockPositionOnDirection.HasValue)
				{
					return;
				}

				SetBlock(blockPositionOnDirection.Value, _blocksContainers.Air);
				playerInteraction.HitDelayTask = UniTask.Delay(HitDelay);
			}
		}

		private void HandleTapInput(Vector2 screenPointerPosition)
		{
			foreach(var playerEntity in _playersFilter)
			{
				var camera = _playerPool.Get(playerEntity).Camera;
				var ray = GetRayTowardPointer(camera, screenPointerPosition);
				var blockPositions = BlocksOnDirectionCalculator.AllBlockPositionsOnLine(ray, 5);
				bool isRayHitSolidBlock = false;
				Vector3Int? preventBlockPositionOnDirection = null;
				foreach(var blockPosition in blockPositions)
				{
					if(GetBlock(blockPosition) != _blocksContainers.Air)
					{
						isRayHitSolidBlock = true;
						break;
					}

					preventBlockPositionOnDirection = blockPosition;
				}

				if(!isRayHitSolidBlock || !preventBlockPositionOnDirection.HasValue)
				{
					return;
				}

				UseBlock(preventBlockPositionOnDirection.Value);
			}
		}

		private Ray GetRayTowardPointer(Camera camera, Vector2 screenPointerPosition)
		{
			Vector3 position = screenPointerPosition;
			position.z = camera.nearClipPlane;
			var ray = camera.ScreenPointToRay(position);
			return ray;
		}
		
		private Block GetBlock(Vector3Int worldPosition)
		{
			if(worldPosition.y < 0 || worldPosition.y >= ChunkConstantData.ChunkScale.y)
			{
				return _blocksContainers.Air;
			}

			var chunkGridPosition = ChunkConstantData.WorldToGridPosition(worldPosition);
			if(!_chunksContainer.TryGetChunk(chunkGridPosition, out int chunkEntity) ||
			   !_chunkInitializedPool.Has(chunkEntity))
			{
				return _blocksContainers.Air;
			}

			Vector3Int blockPosition = ChunkConstantData.WorldToBlockPositionInChunk(worldPosition);
			return _chunkDataPool.Get(chunkEntity).Blocks[blockPosition];
		}

		private void SetBlock(Vector3Int worldPosition, Block block)
		{
			var setBlockPool = _world.GetPool<SetBlockComponent>();
			var entity = _world.NewEntity();
			ref var setBlock = ref setBlockPool.Add(entity);
			setBlock.WorldPosition = worldPosition;
			setBlock.Block = block;
		}

		private void UseBlock(Vector3Int worldPosition) 
		{
			foreach(var playerEntity in _playersFilter)
			{
				ref var inventory = ref _playerInventoryPool.Get(playerEntity);
				ref var player = ref _playerPool.Get(playerEntity);
				var item = inventory.Toolbar.ActiveSlot.Item;
				if(item is BlockInventoryItem blockInventoryItem)
				{
					blockInventoryItem.Use(worldPosition);
				}
			}
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