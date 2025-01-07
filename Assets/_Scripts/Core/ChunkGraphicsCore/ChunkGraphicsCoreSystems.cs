using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshGeneration.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshGeneration.Systems;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshPartsContainerLogic.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshPartsContainerLogic.Systems;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshPartsContainerLogic.WallUpdate.Components;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.MeshPartsContainerLogic.WallUpdate.Systems;
using _Scripts.Core.ChunkGraphicsCore.ChunkGraphicsLogic.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Core.ChunkGraphicsCore
{
	public static class ChunkGraphicsCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetStandartInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{
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
