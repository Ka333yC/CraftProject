using System.Collections.Generic;
using _Scripts.Apart.Extensions.Ecs.DelHere;
using _Scripts.Core.PhysicsCore.ChunkPhysicsCore.ChunkPhysicsLogic.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.BoundsUpdate.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.BoundsUpdate.Systems;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.GroundCheck.Systems;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.InBlockCheck.Systems;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.InSimulatedChunkCheck.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.InSimulatedChunkCheck.Systems;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.Systems;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.StandardNotification.Components;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.StandardNotification.Systems;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Systems;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore
{
	public static class ObjectPhysicsCoreSystems
	{
		public static IEnumerable<IEcsSystem> GetFixedInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{
				new ObjectPhysicsPositionsContainerCreator(),
				new IntersectionWithBlockBoundsContainerCreator(),
			};
		}

		public static IEnumerable<IEcsSystem> GetFixedSystems()
		{
			return new List<IEcsSystem>()
			{
				new GroundCheckerInitializeSystem(),
				// Обновление текущей позиции
				new DelHereSystem<BlockPositionChangedComponent>(),
				new DelHereSystem<GridPositionChangedComponent>(),
				new ObjectPhysicsPositionsUpdater(),
				new StandardPositionsChangedNotifySystem(),
				new DelHereSystem<BoundsChangedComponent>(),
				new ObjectPhysicsBoundsUpdater(),
				// Применение сопротивления(drag) и остановка движения, если оно незначительное
				new ObjectPhysicsApplyCalculationsSystem(),

				new DelHereSystem<ChunkPhysicsBecomeSimulatedTag>(),
				new ChunkPhysicsSimulatedMarker(),
				new DelHereExcSystem<ChunkPhysicsSimulatedTag, ChunkPhysicsComponent>(),
				new InSimulatedChunkPhysicsMarker(),
				new IntersectionWithBlockBoundsUpdater(),
				new CheckIsObjectPhysicsInBlockMarker(),
				new ObjectPhysicsInBlockCheckerSystem(),
				new PhysicsDeactivationSystem(),

				//new HorizontalMovementHandlerSystem(),
				//new DelHereSystem<MovementDirectionComponent>(),
				//new JumpHandlerSystem(),
				//new DelHereSystem<JumpTag>(),
			};
		}

		public static IEnumerable<IEcsSystem> GetPostStandardDelSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<StandardBlockPositionChangedComponent>(),
				new DelHereSystem<StandardGridPositionChangedComponent>(),
			};
		}
	}
}
