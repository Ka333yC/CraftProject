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
		private EcsFilter _playersToMoveFilter;

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
			_playersToMoveFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<MovementParametersComponent>()
				.Inc<PlayerRotationComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach (var inputEntity in _walkInputFilter)
			{
				var walkInput = _walkInputPool.Get(inputEntity).Input;
				HandleWalkInput(walkInput);
			}
		}

		private void HandleWalkInput(Vector2 walkInput)
		{
			foreach (var playerEntity in _playersToMoveFilter)
			{
				var rigidbody = _objectPhysicsPool.Get(playerEntity).Rigidbody;
				var movementParameters = _movementParametersPool.Get(playerEntity);
				var rotationEuler = _playerRotationPool.Get(playerEntity).Rotation;
				var rotation = Quaternion.Euler(rotationEuler);
				var direction = rotation * Vector3.right * walkInput.x +
					rotation * Vector3.forward * walkInput.y;
				direction.y = 0;
				ApplyMove(rigidbody, direction, ref movementParameters);
			}
		}	
		
		private void ApplyMove(Rigidbody rigidbody, Vector3 direction, 
			ref MovementParametersComponent movementParameters)
		{
			var horizontalVelocity = rigidbody.velocity;
			horizontalVelocity.y = 0;
			if(horizontalVelocity.magnitude > movementParameters.HorizontalVelocityUnderControl)
			{
				return;
			}

			var velocityToAdd = movementParameters.HorizontalAcceleration  * Time.fixedDeltaTime
				* direction.normalized;
			var resultHorizontalVelocity = rigidbody.velocity + velocityToAdd;
			resultHorizontalVelocity.y = 0;
			if(resultHorizontalVelocity.magnitude > movementParameters.MaxHorizontalVelocity)
			{
				resultHorizontalVelocity = resultHorizontalVelocity.normalized * movementParameters.MaxHorizontalVelocity;
			}

			resultHorizontalVelocity.y = rigidbody.velocity.y;
			rigidbody.velocity = resultHorizontalVelocity;
		}
	}
}