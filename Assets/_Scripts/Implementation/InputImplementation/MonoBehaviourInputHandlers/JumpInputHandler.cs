﻿using _Scripts.Core.InputCore.Components;
using Assets._Scripts.Implementation.InputImplementation.MonoBehaviourInputHandlers;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace _Scripts.Core.InputCore.MonoBehaviourInputHandlers
{
	public class JumpInputHandler : MonoBehaviourInputHandler
	{
		[SerializeField] 
		private string _actionName = "Jump";

		[Inject]
		private EcsWorld _world;

		private EcsPool<JumpInputTag> _jumpInputPool;
		private InputAction _action;
		private bool _isActionStarted;

		public bool Input
		{
			get 
			{
				return _isActionStarted;
			}
		}

		private void Start()
		{
			_jumpInputPool = _world.GetPool<JumpInputTag>();
		}

		private void Update()
		{
			if(!_isActionStarted)
			{
				return;
			}

			var inputEntity = _world.NewEntity();
			_jumpInputPool.Add(inputEntity);
		}

		public override void Initailize(InputActionMap actionMap)
		{
			_action = actionMap.FindAction(_actionName);
			_action.started += ActionStarted;
			_action.canceled += ActionCanceled;
		}

		public override void Enable()
		{
			_action.Enable();
		}

		public override void Disable()
		{
			_action.Disable();
		}

		private void ActionStarted(CallbackContext context)
		{
			_isActionStarted = true;
		}

		private void ActionCanceled(CallbackContext context)
		{
			_isActionStarted = false;
		}
	}
}
