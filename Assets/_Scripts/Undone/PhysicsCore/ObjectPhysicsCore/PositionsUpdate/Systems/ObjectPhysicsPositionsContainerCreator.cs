using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components;
using _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Components.Elements;
using Leopotam.EcsLite;

namespace _Scripts.Undone.PhysicsCore.ObjectPhysicsCore.PositionsUpdate.Systems
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
