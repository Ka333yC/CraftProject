﻿using _Scripts.Implementation.InputImplementation.Components;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Scripts.Implementation.InputImplementation.MonoBehaviourInputHandlers
{
	public class RotationInputHandler : MonoBehaviourInputHandler
	{
		[SerializeField] 
		private string _actionName = "Look";

		[Inject]
		private EcsWorld _world;

		private EcsPool<RotationInputComponent> _rotationInputPool;
		private FingerInputAction _fingerAction;
		private Vector2 _lastFrameFingerPosition;

		private void Start()
		{
			_rotationInputPool = _world.GetPool<RotationInputComponent>();
		}

		private void FixedUpdate()
		{
			if(!_fingerAction.IsActive)
			{
				return;
			}

			var fingerScreenPosition = _fingerAction.Finger.screenPosition;
			var delta = fingerScreenPosition - _lastFrameFingerPosition;
			// Если finger не сдвинулся
			if(delta.magnitude < 1)
			{
				return;
			}

			var inputEntity = _world.NewEntity();
			ref var rotationInput = ref _rotationInputPool.Add(inputEntity);
			delta.y *= -1;
			rotationInput.PointerDeltaInput = delta;
			_lastFrameFingerPosition = fingerScreenPosition;
		}

		private void OnDestroy()
		{
			_fingerAction.Dispose();
		}

		public override void Initialize(InputActionMap actionMap)
		{
			_fingerAction = new FingerInputAction(actionMap, _actionName);
			_fingerAction.OnActionStarted += ActionStarted;
		}

		public override void Enable()
		{
			_fingerAction.Enable();
		}

		public override void Disable()
		{
			_fingerAction.Disable();
		}

		private void ActionStarted()
		{
			_lastFrameFingerPosition = _fingerAction.Finger.screenPosition;
		}
	}
}
