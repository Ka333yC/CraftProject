﻿using Leopotam.EcsLite;
using Input.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Assets.Scripts.Core.InputCore.MonoBehaviourInputHandlers;
using Zenject;

namespace Input.MonoBehaviourHandlers
{
	public class WalkInputHandler : MonoBehaviour, IInputHandler
	{
		[SerializeField] 
		private string _actionName = "Walk";

		[Inject]
		private EcsWorld _world;

		private EcsPool<WalkInputComponent> _walkInputPool;
		private InputAction _action;
		private bool _isActionStarted;
		private Vector2 _lastPerformedInput;

		public bool HasInput
		{
			get
			{
				return _isActionStarted;
			}
		}

		public Vector2 Input
		{
			get
			{
				return _lastPerformedInput;
			}
		}

		private void Start()
		{
			_walkInputPool = _world.GetPool<WalkInputComponent>();
		}

		private void Update()
		{
			if(!_isActionStarted)
			{
				return;
			}

			var inputEntity = _world.NewEntity();
			ref var walkInput = ref _walkInputPool.Add(inputEntity);
			walkInput.Input = _lastPerformedInput;
		}

		public void Initailize(InputActionMap actionMap)
		{
			_action = actionMap.FindAction(_actionName);
			_action.started += ActionStarted;
			_action.performed += ActionPerformed;
			_action.canceled += ActionCanceled;
		}

		public void Enable()
		{
			_action.Enable();
		}

		public void Disable()
		{
			_action.Disable();
		}

		private void ActionStarted(CallbackContext context)
		{
			_isActionStarted = true;
			_lastPerformedInput = context.ReadValue<Vector2>();
		}

		private void ActionPerformed(CallbackContext context)
		{
			_lastPerformedInput = context.ReadValue<Vector2>();
		}

		private void ActionCanceled(CallbackContext context)
		{
			_isActionStarted = false;
			_lastPerformedInput = Vector2.zero;
		}
	}
}
