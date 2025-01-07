using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshGeneration.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshGeneration.Systems;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.Systems;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.WallUpdate.Components;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.MeshPartsContainerLogic.WallUpdate.Systems;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ChunkPhysicsCore
{
	public static class ChunkPhysicsCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetFixedInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{
			};
		}

		public static IEnumerable<IEcsSystem> GetFixedSystems()
		{
			return new List<IEcsSystem>()
			{
				// Управление временем жизни ChunkPhysics
				new ChunkPhysicsCreator(),
				new ChunkPhysicsDestroyer(),
				// Инициализация
				new DelHereExcSystem<MeshPartsContainerInitializedTag, ChunkPhysicsComponent>(),
				new MeshPartsContainerInitializer(),
				// Обновление пограничных блоков для инициализированного чанка
				new DelHereExcSystem<DirtyWallsComponent, ChunkPhysicsComponent>(),
				new DelHereExcSystem<BehindWallsMarkedDirtyTag, ChunkPhysicsComponent>(),
				new MarkBehindWallsDirtySystem(),
				new MeshPartsContainerWallUpdater(),
				new PhysicsUpdaterOnBlockChanged(),
				// Перестройка Mesh'а
				new DelHereExcSystem<ChunkPhysicsDirtyMeshTag, ChunkPhysicsComponent>(),
				new DelHereSystem<ChunkPhysicsMeshGeneratedNotificationTag>(),
				new ChunkPhysicsMeshGenerator(),
			};
		}
	}
}
