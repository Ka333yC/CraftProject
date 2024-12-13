using Leopotam.EcsLite;
using Input.Components;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
using Assets.Scripts.Core.InputCore.MonoBehaviourInputHandlers;
using TempScripts;
using Zenject;

namespace Input.MonoBehaviourHandlers
{
	public class JumpInputHandler : MonoBehaviour, IInputHandler
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

		public void Initailize(InputActionMap actionMap)
		{
			_action = actionMap.FindAction(_actionName);
			_action.started += ActionStarted;
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
		}

		private void ActionCanceled(CallbackContext context)
		{
			_isActionStarted = false;
		}
	}
}
