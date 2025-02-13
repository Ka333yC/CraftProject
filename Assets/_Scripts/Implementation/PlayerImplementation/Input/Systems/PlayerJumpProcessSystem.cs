using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Implementation.InputImplementation.Components;
using _Scripts.Implementation.PlayerImplementation.Input.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Implementation.PlayerImplementation.Input.Systems
{
	public class PlayerJumpProcessSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<MovementParametersComponent> _movementParametersPool;
		private EcsPool<PlayerJumpVelocityComponent> _playerJumpVelocityPool;
		private EcsFilter _jumpInputFilter;
		private EcsFilter _playerToInitializeFilter;
		private EcsFilter _playerToJumpFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_movementParametersPool = world.GetPool<MovementParametersComponent>();
			_playerJumpVelocityPool = world.GetPool<PlayerJumpVelocityComponent>();
			_jumpInputFilter = world
				.Filter<JumpInputTag>()
				.End();
			_playerToInitializeFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<MovementParametersComponent>()
				.Exc<PlayerJumpVelocityComponent>()
				.End();
			_playerToJumpFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<MovementParametersComponent>()
				.Inc<PlayerJumpVelocityComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var playerEntity in _playerToInitializeFilter)
			{
				var movementParameters = _movementParametersPool.Get(playerEntity);
				// (v0)^2 = 2gh
				var jumpVelocity = Mathf.Sqrt(2 * -Physics.gravity.y * movementParameters.JumpHeight);
				// Добавляю "небольшую" дополнительную скорость, чтобы подогнать значения. Подробно:
				// после окончания физического кадра к velocity применяется сила тяжести,
				// даже если объект не сдвинулся. Поэтому добавляем небольшую(величиной в один кадр) скорость
				jumpVelocity += jumpVelocity * Time.fixedDeltaTime;
				ref var playerJumpVelocityComponent = 
					ref _playerJumpVelocityPool.Add(playerEntity);
				playerJumpVelocityComponent.JumpVelocity = jumpVelocity;
			}
			
			foreach(var inputEntity in _jumpInputFilter)
			{
				HandleJumpInput();
			}
		}

		private void HandleJumpInput()
		{
			foreach(var playerEntity in _playerToJumpFilter)
			{
				ref var objectPhysics = ref _objectPhysicsPool.Get(playerEntity);
				if (!objectPhysics.GroundChecker.IsGrounded())
				{
					continue;
				}
				
				var jumpVelocity = _playerJumpVelocityPool.Get(playerEntity).JumpVelocity;
				var currentVelocity = objectPhysics.Rigidbody.velocity;
				objectPhysics.Rigidbody.velocity = new Vector3(currentVelocity.x, jumpVelocity, currentVelocity.z);
			}
		}
	}
}