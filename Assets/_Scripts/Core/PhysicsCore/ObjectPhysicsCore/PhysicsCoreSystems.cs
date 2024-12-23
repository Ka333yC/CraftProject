using System;
using Leopotam.EcsLite;
using System.Collections.Generic;
using PhysicsCore.ObjectPhysics.PositionUpdater.StandartNotification.Components;
using PhysicsCore.ObjectPhysics.PositionUpdater.Components;
using Extensions.Ecs;
using PhysicsCore.ObjectPhysics.PositionUpdater.Systems;
using PhysicsCore.ObjectPhysics.PositionUpdater.StandartNotification.Systems;
using PhysicsCore.ObjectPhysics.Drag.Systems;
using Assets.Scripts.Core.PhysicsCore.ObjectPhysics.Systems;
using Assets.Scripts.Apart.Extensions.Ecs;
using Assets.Scripts.Core.PhysicsCore.ObjectPhysics.PhysicsDeactivating.Systems;
using Assets.Scripts.Core.PhysicsCore.ObjectPhysics.PhysicsDeactivating.Components;
using PhysicsCore.ChunkPhysicsCore.LifeTimeControl.Components;
using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Systems;
using Assets.Scripts.Implementation.ObjectPhysics.GroundCheck.Systems;
using Assets.Scripts.Core.ObjectPhysicsCore.BoundsUpdate.Systems;
using Assets.Scripts.Core.ObjectPhysicsCore.BoundsUpdate.Components;
using Assets.Scripts.Core.ObjectPhysicsCore.PositionsUpdate.Systems;
using Assets.Scripts.Core.ObjectPhysicsCore.InSimulatedChunkCheck.Components;
using Assets.Scripts.Core.ObjectPhysicsCore.PhysicsDeactivation.Systems;

namespace PhysicsCore.ObjectPhysics
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
				new StandartPositionsChangedNotifySystem(),
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

		public static IEnumerable<IEcsSystem> GetPostStandartDelSystems()
		{
			return new List<IEcsSystem>()
			{
				new DelHereSystem<StandartBlockPositionChangedComponent>(),
				new DelHereSystem<StandartGridPositionChangedComponent>(),
			};
		}
	}
}
