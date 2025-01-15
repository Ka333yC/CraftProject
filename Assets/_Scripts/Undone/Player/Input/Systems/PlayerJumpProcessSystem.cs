using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Implementation.InputImplementation.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Implementation.PlayerImplementation.Movement.Systems
{
	public class PlayerJumpProcessSystem: IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<MovementParametersComponent> _movementParametersPool;
		private EcsFilter _jumpInputFilter;
		private EcsFilter _playerToJumpFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_movementParametersPool = world.GetPool<MovementParametersComponent>();
			_jumpInputFilter = world
				.Filter<JumpInputTag>()
				.End();
			_playerToJumpFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<MovementParametersComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var jumpEntity in _jumpInputFilter)
			{
				HandleJumpInput();
			}
		}

		private void HandleJumpInput()
		{
			foreach(var playerEntity in _playerToJumpFilter)
			{
				ref var objectPhysics = ref _objectPhysicsPool.Get(playerEntity);
				if (objectPhysics.GroundChecker.IsGrounded())
				{
					var movementParameters = _movementParametersPool.Get(playerEntity);
					objectPhysics.Rigidbody.AddForce(-Physics.gravity * movementParameters.JumpPower, 
						ForceMode.Acceleration);
				}
			}
		}
	}
}