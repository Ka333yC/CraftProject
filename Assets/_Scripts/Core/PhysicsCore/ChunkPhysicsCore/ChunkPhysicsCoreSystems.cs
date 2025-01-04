using System;
using Leopotam.EcsLite;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Systems;
using System.Collections.Generic;
using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.BlocksUpdating.Systems;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshUpdating.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshUpdating.Systems;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.Systems;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Mark.Components;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Mark.Systems;
using Assets.Scripts.Core.PhysicsCore.ChunkPhysicsCore.MeshPartsContainerUpdating.WallUpdating.Systems;
using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Systems;

namespace Assets.Scripts.Core.PhysicsCore
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
