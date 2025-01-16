using _Scripts.Core.PhysicsCore.ObjectPhysicsCore.Components;
using _Scripts.Core.PlayerCore.Components;
using _Scripts.Implementation.InputImplementation.Components;
using _Scripts.Implementation.PlayerImplementation.Movement;
using Leopotam.EcsLite;
using UnityEngine;

namespace Input.Systems
{
	public class PlayerRotationProcessSystem : IEcsPreInitSystem, IEcsRunSystem
	{
		private EcsPool<RotationInputComponent> _rotationInputPool;
		private EcsPool<MovementParametersComponent> _movementParametersPool;
		private EcsPool<PlayerRotationComponent> _playerRotationPool;
		private EcsFilter _rotationInputFilter;
		private EcsFilter _playersToInitializeFilter;
		private EcsFilter _playersToRotateFilter;

		public void PreInit(IEcsSystems systems)
		{
			var world = systems.GetWorld();
			_rotationInputPool = world.GetPool<RotationInputComponent>();
			_movementParametersPool = world.GetPool<MovementParametersComponent>();
			_playerRotationPool = world.GetPool<PlayerRotationComponent>();
			_rotationInputFilter = world
				.Filter<RotationInputComponent>()
				.End();
			_playersToInitializeFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<MovementParametersComponent>()
				.Exc<PlayerRotationComponent>()
				.End();
			_playersToRotateFilter = world
				.Filter<PlayerComponent>()
				.Inc<ObjectPhysicsComponent>()
				.Inc<MovementParametersComponent>()
				.Inc<PlayerRotationComponent>()
				.End();
		}

		public void Run(IEcsSystems systems)
		{
			foreach(var playerEntity in _playersToInitializeFilter)
			{
				ref var playerRotationComponent = ref _playerRotationPool.Add(playerEntity);
				var playerRotation = 
					_movementParametersPool.Get(playerEntity).RotatableTransform.localRotation.eulerAngles;
				playerRotationComponent.Rotation = playerRotation;
			}
			
			foreach(var inputEntity in _rotationInputFilter)
			{
				var lookInput = _rotationInputPool.Get(inputEntity).PointerDeltaInput;
				HandleRotationInput(lookInput);
			}
		}

		private void HandleRotationInput(Vector3 pointerDeltaInput)
		{
			foreach(var playerEntity in _playersToRotateFilter)
			{
				ref var movementParameters = ref _movementParametersPool.Get(playerEntity);
				ref var rotation = ref _playerRotationPool.Get(playerEntity);
				var actualRotation = CalculateRotation(rotation.Rotation, pointerDeltaInput);
				rotation.Rotation = actualRotation;
				movementParameters.RotatableTransform.localRotation = Quaternion.Euler(actualRotation);
			}
		}
		
		private Vector3 CalculateRotation(Vector3 previousRotation, Vector3 pointerDeltaInput)
		{
			previousRotation.y += pointerDeltaInput.x;
			previousRotation.x += pointerDeltaInput.y;
			previousRotation.x = Mathf.Clamp(previousRotation.x, -89.9f, 89.9f);
			return previousRotation;
		}
	}
}
