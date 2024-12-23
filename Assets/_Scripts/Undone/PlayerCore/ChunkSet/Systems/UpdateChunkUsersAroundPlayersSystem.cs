
using Assets.Scripts.Implementation.ObjectPhysics.PositionsUpdate.Components;
using Assets.Scripts.Implementation.Settings.GraphicsSettings;
using ChunkCore.ChunksContainerScripts;
using ChunkCore.ChunksContainerScripts.Components;
using ChunkCore.LifeTimeControl.Components;
using Leopotam.EcsLite;
using PhysicsCore.ObjectPhysics.Components;
using PhysicsCore.ObjectPhysics.PositionUpdater.Components;
using System;
using System.Collections.Generic;
using TempScripts;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.PlayerCore.ChunkSet.Systems
{
	class UpdateChunkUsersAroundPlayersSystem : IEcsPreInitSystem, IEcsInitSystem, IEcsRunSystem
	{
		[Inject]
		private GraphicsSettingsSystem _graphicsSettingsSystem;

		private ChunksContainer _chunksContainer;

		private EcsPool<ChunkUsersAroundInitializedTag> _chunkUsersAroundInitializedPool;
		private EcsPool<ObjectPhysicsPositionsComponent> _objectPhysicsPositionsPool;
		private EcsPool<GridPositionChangedComponent> _gridPositionChangedPool;
		private EcsFilter _playersWithNoChunksAroundFilter;
		private EcsFilter _playersChangedPositionFilter;

		public void PreInit(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			_chunkUsersAroundInitializedPool = world.GetPool<ChunkUsersAroundInitializedTag>();
			_objectPhysicsPositionsPool = world.GetPool<ObjectPhysicsPositionsComponent>();
			_gridPositionChangedPool = world.GetPool<GridPositionChangedComponent>();
			_playersWithNoChunksAroundFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<ObjectPhysicsPositionsComponent>()
				.Exc<ChunkUsersAroundInitializedTag>()
				.End();
			_playersChangedPositionFilter = world
				.Filter<GridPositionChangedComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<PlayerComponent>()
				.End();
		}

		public void Init(IEcsSystems systems)
		{
			_chunksContainer = GetChunksContainer(systems.GetWorld());
		}

		public void Run(IEcsSystems systems)
		{
			AddChunkUsersAroundPlayerWithoutChunks();
			UpdateChunkUsersAroundPlayerChangedPosition();
		}

		private void AddChunkUsersAroundPlayerWithoutChunks()
		{
			foreach(var playerEntity in _playersWithNoChunksAroundFilter)
			{
				var playerGridPosition = _objectPhysicsPositionsPool.Get(playerEntity).GridPosition;
				var chunksInRange =
					GetChunksInRange(playerGridPosition, _graphicsSettingsSystem.LoadingRange);
				AddChunkUsers(playerGridPosition, chunksInRange);
				_chunkUsersAroundInitializedPool.Add(playerEntity);
			}
		}

		// Как идея оптимизации -вынести в таск, чтобы не нагружало главный поток,
		// т.к. вычеслений действительно много
		private void UpdateChunkUsersAroundPlayerChangedPosition()
		{
			foreach(var playerEntity in _playersChangedPositionFilter)
			{
				var playerNowGridPosition = _objectPhysicsPositionsPool.Get(playerEntity).GridPosition;
				var chunksInRange =
					GetChunksInRange(playerNowGridPosition, _graphicsSettingsSystem.LoadingRange);
				AddChunkUsers(playerNowGridPosition, chunksInRange);

				var previousGridPosition =
					_gridPositionChangedPool.Get(playerEntity).PreviousGridPosition;
				var chunksInPreviousPositionRange =
					GetChunksInRange(previousGridPosition, _graphicsSettingsSystem.LoadingRange);
				RemoveChunkUsers(previousGridPosition, chunksInPreviousPositionRange);
			}
		}

		private IEnumerable<Vector3Int> GetChunksInRange(Vector3Int gridPosition, int range)
		{
			for(int x = -range; x <= range; x++)
			{
				for(int z = -range; z <= range; z++)
				{
					yield return new Vector3Int(gridPosition.x + x, 0, gridPosition.z + z);
				}
			}

			yield break;
		}

		private void AddChunkUsers(Vector3Int playerGridPosition, IEnumerable<Vector3Int> positionsToAddChunkUser)
		{
			foreach(var gridPosition in positionsToAddChunkUser)
			{
				var chunkPriority = GetChunkPriority(playerGridPosition, gridPosition);
				_chunksContainer.AddChunkUser(gridPosition, chunkPriority);
			}
		}

		private void RemoveChunkUsers(Vector3Int previousPlayerGridPosition, 
			IEnumerable<Vector3Int> positionsToRemoveChunkUser)
		{
			foreach(var gridPosition in positionsToRemoveChunkUser)
			{
				var chunkPriority = GetChunkPriority(previousPlayerGridPosition, gridPosition);
				_chunksContainer.RemoveChunkUser(gridPosition, chunkPriority);
			}
		}

		private int GetChunkPriority(Vector3Int playerGridPosition, Vector3Int chunkPosition)
		{
			Vector3Int distanceDelta = playerGridPosition - chunkPosition;
			return Mathf.Abs(distanceDelta.x) + Mathf.Abs(distanceDelta.z);
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
