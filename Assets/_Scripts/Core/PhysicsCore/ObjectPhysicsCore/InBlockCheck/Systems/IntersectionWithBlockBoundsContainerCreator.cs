using Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Components;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Implementation.ObjectPhysics.InBlockCheck2.Systems
{
	public class IntersectionWithBlockBoundsContainerCreator : IEcsInitSystem
	{
		public void Init(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			var pool =
				world.GetPool<IntersectionWithBlockBoundsContainerComponent>();
			var entity = world.NewEntity();
			ref var component = ref pool.Add(entity);
			component.Container = new EntitiesBoundsContainer();
		}
	}
}
