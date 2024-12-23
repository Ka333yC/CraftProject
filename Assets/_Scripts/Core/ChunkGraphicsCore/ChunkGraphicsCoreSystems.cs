using System;
using GraphicsCore.ChunkGraphicsCore.Cache.ChunkGraphicsMeshFilterPoolScripts.Systems;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Systems;
using Leopotam.EcsLite;
using System.Collections.Generic;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.Systems;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshUpdating.Systems;
using Extensions.Ecs;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.WallUpdating.Mark.Systems;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.WallUpdating.Systems;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.BlocksUpdating.Systems;
using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.ChunkCore.LifeTimeControl.Components;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.WallUpdating.Mark.Components;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.WallUpdating.Components;
using GraphicsCore.ChunkGraphicsCore.LifeTimeControl.Components;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshPartsContainerUpdating.Components;
using Assets.Scripts.Core.GraphicsCore.ChunkGraphicsCore.MeshUpdating.Components;

namespace Assets.Scripts.Core.GraphicsCore
{
	public static class ChunkGraphicsCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetStandartInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{
				new ChunkGraphicsGameObjectPoolCreator(),
			};
		}

		public static IEnumerable<IEcsSystem> GetStandartSystems()
		{
			return new List<IEcsSystem>()
			{
				// Управление временем жизни ChunkGraphics
				new ChunkGraphicsCreator(),
				new ChunkGraphicsDestroyer(),
				// Инициализация
				new DelHereExcSystem<MeshPartsContainerInitializedTag, ChunkGraphicsComponent>(),
				new MeshPartsContainerInitializer(),
				// Обновление пограничных блоков для инициализированного чанка
				new DelHereExcSystem<DirtyWallsComponent, ChunkGraphicsComponent>(),
				new DelHereExcSystem<BehindWallsMarkedDirtyTag, ChunkGraphicsComponent>(),
				new MarkBehindWallsDirtySystem(),
				new MeshPartsContainerWallUpdater(),
				// Обновление графики при изменении блока
				new GraphicsUpdaterOnBlockChanged(),
				// Перестройка Mesh'а
				new DelHereExcSystem<ChunkGraphicsDirtyMeshComponent, ChunkGraphicsComponent>(),
				new ChunkGraphicsMeshGenerator(),
			};
		}
	}
}
