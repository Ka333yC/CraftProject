using _Scripts.Apart.Extensions.Ecs;
using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Implementation.InputImplementation.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Implementation.PlayerImplementation.Movement.Systems
{
	public class PlayerWalkProcessSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<WalkInputComponent> _walkInputPool;
		private EcsPool<MovementParametersComponent> _movementParametersPool;
		private EcsPool<ObjectPhysicsComponent> _objectPhysicsPool;
		private EcsPool<PlayerRotationComponent> _playerRotationPool;
		private EcsFilter _walkInputFilter;
		private EcsFilter _playerToMoveFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_walkInputPool = world.GetPool<WalkInputComponent>();
			_movementParametersPool = world.GetPool<MovementParametersComponent>();
			_objectPhysicsPool = world.GetPool<ObjectPhysicsComponent>();
			_playerRotationPool = world.GetPool<PlayerRotationComponent>();
			_walkInputFilter = world
				.Filter<WalkInputComponent>()
				.End();
			_playerToMoveFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<MovementParametersComponent>()
				.Inc<PlayerRotationComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var walkInputEntity in _walkInputFilter)
			{
				var walkInput = _walkInputPool.Get(walkInputEntity).Input;
				HandleWalkInput(walkInput);
			}
		}

		private void HandleWalkInput(Vector2 walkInput)
		{
			foreach (var playerEntity in _playerToMoveFilter)
			{
				var rigidbody = _objectPhysicsPool.Get(playerEntity).Rigidbody;
				var movementParameters = _movementParametersPool.Get(playerEntity);
				var rotationEuler = _playerRotationPool.Get(playerEntity).Rotation;
				var rotation = Quaternion.Euler(rotationEuler);
				var direction = rotation * Vector3.right * walkInput.x +
					rotation * Vector3.forward * walkInput.y;
				ApplyMove(rigidbody, ref movementParameters, direction);
			}
		}
		
		private void ApplyMove(Rigidbody rigidbody, ref MovementParametersComponent movementParameters, 
			Vector3 direction)
		{
			var horizontalVelocity = rigidbody.velocity;
			horizontalVelocity.y = 0;
			if(horizontalVelocity.magnitude > movementParameters.HorizontalVelocityUnderControl)
			{
				return;
			}

			var horizontalDirection = new Vector3(direction.x, 0, direction.z);
			horizontalDirection.Normalize();
			var velocityToAdd = horizontalDirection * movementParameters.HorizontalAcceleration 
				* Time.fixedDeltaTime;
			var resultVelocity = rigidbody.velocity + velocityToAdd;
			resultVelocity.y = 0;
			if(resultVelocity.magnitude > movementParameters.MaxHorizontalVelocity)
			{
				resultVelocity = resultVelocity.normalized * movementParameters.MaxHorizontalVelocity;
			}

			resultVelocity.y = rigidbody.velocity.y;
			rigidbody.velocity = resultVelocity;
		}
	}
}