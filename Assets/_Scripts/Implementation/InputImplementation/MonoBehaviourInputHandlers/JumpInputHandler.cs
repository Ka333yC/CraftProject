using _Scripts.Implementation.InputImplementation.Components;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using static UnityEngine.InputSystem.InputAction;

namespace _Scripts.Implementation.InputImplementation.MonoBehaviourInputHandlers
{
	public class JumpInputHandler : MonoBehaviourInputHandler
	{
		[SerializeField] 
		private string _actionName = "Jump";

		[Inject]
		private EcsWorld _world;

		private EcsPool<JumpInputTag> _jumpInputPool;
		private EcsFilter _jumpInputFilter;
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
			_jumpInputFilter = _world
				.Filter<JumpInputTag>()
				.End();
		}

		private void FixedUpdate()
		{
			if(!_isActionStarted || _jumpInputFilter.GetEntitiesCount() > 0)
			{
				return;
			}

			var inputEntity = _world.NewEntity();
			_jumpInputPool.Add(inputEntity);
		}

		public override void Initialize(InputActionMap actionMap)
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
