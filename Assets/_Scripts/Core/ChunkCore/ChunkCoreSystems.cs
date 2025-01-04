using System.Collections.Generic;
using Leopotam.EcsLite;
using ChunkCore.LifeTimeControl.Components.Fixed;
using ChunkCore.LifeTimeControl.Components.Standart;
using ChunkCore.Loading.Components;
using ChunkCore.OnBlockChanged.Components;
using ChunkCore.OnBlockChanged.FixedNotification.Components;
using ChunkCore.OnBlockChanged.Systems;
using ChunkCore.OnBlockChanged.FixedNotification.Systems;
using ChunkCore.Loading.Systems;
using Assets.Scripts.Core.ChunkCore.Saving.Systems;
using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Systems;
using ChunkCore.ChunksContainerScripts.Systems;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using PhysicsCore.ChunkPhysicsCore.Cache.ChunkPhysicsMeshColliderPoolScripts.Systems;

namespace ChunkCore
{
	public static class ChunkCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetFixedInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{
				new ChunksContainerCreator(),
				new ChunkGameObjectPoolCreator(),
				// new BlockContainersInitializer(),
			};
		}

		public static IEnumerable<IEcsSystem> GetStandartInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{

			};
		}

		public static IEnumerable<IEcsSystem> GetFixedSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<FixedChunkСreatedTag>(),
				new ChunkCreator(),
				new DelHereSystem<FixedChunkDestroyedComponent>(),
				new ChunkDestroyer(),
				// Генерация
				new DelHereSystem<FixedChunkInitializedNotificationTag>(),
				new ChunkInitializer(),
				new DelHereExcSystem<ChunkInitializedTag, ChunkComponent>(),
				// Сохранение
				new MarkChangedChunksNeedToSaveSystem(),
				new ChunkSaveSystem(),
			};
		}

		public static IEnumerable<IEcsSystem> GetStandartSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<BlocksChangedComponent>(),
				new BlockChangeSystem(),
				new FixedBlocksChangedNotificationSystem(),
			};
		}

		public static IEnumerable<IEcsSystem> GetPostFixedDelSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<FixedBlocksChangedComponent>(),
			};
		}

		public static IEnumerable<IEcsSystem> GetPostStandartDelSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<StandartChunkСreatedTag>(),
				new DelHereSystem<StandartChunkDestroyedComponent>(),
				new DelHereSystem<StandartChunkInitializedNotificationTag>(),
			};
		}
	}
}
