using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using _Scripts.Core.ChunkCore.BlockChanging.Components;
using _Scripts.Core.ChunkCore.BlockChanging.FixedNotification.Components;
using _Scripts.Core.ChunkCore.BlockChanging.FixedNotification.Systems;
using _Scripts.Core.ChunkCore.BlockChanging.Systems;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Fixed;
using _Scripts.Core.ChunkCore.ChunkLogic.Components.Standard;
using _Scripts.Core.ChunkCore.ChunkLogic.Pools.Systems;
using _Scripts.Core.ChunkCore.ChunkLogic.Saving.Systems;
using _Scripts.Core.ChunkCore.ChunkLogic.Systems;
using _Scripts.Core.ChunkCore.ChunksContainerLogic.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkCore
{
	public static class ChunkCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetFixedInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{
				new ChunksContainerCreator(),
				new ChunkGameObjectPoolCreator(),
			};
		}

		public static IEnumerable<IEcsSystem> GetStandardInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{

			};
		}

		public static IEnumerable<IEcsSystem> GetFixedSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<FixedChunkCreatedTag>(),
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

		public static IEnumerable<IEcsSystem> GetStandardSystems()
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

		public static IEnumerable<IEcsSystem> GetPostStandardDelSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<StandardChunkCreatedTag>(),
				new DelHereSystem<StandardChunkDestroyedComponent>(),
				new DelHereSystem<StandardChunkInitializedNotificationTag>(),
			};
		}
	}
}
