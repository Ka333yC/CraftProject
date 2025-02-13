using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.InBlockCheck.Components;
using Leopotam.EcsLite;

namespace _Scripts.Core.PhysicsCore.ObjectPhysicsCore.PhysicsDeactivation.InBlockCheck.Systems
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
