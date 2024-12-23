using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Components;
using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Core.ObjectPhysicsCore.PositionsUpdate.Components;

namespace Assets.Scripts.Core.ObjectPhysicsCore.PositionsUpdate.Systems
{
	public class ObjectPhysicsPositionsContainerCreator : IEcsInitSystem
	{
		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			var pool =
				world.GetPool<ObjectPhysicsPositionsContainerComponent>();
			var entity = world.NewEntity();
			ref var component = ref pool.Add(entity);
			component.Container = new ObjectPhysicsPositionsContainer();
		}
	}
}
