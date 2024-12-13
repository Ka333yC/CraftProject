using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Leopotam.EcsLite;
using Input.Components;
using static UnityEngine.InputSystem.InputAction;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Linq;
using System.Threading.Tasks;
using Assets.Scripts.Core.InputCore.MonoBehaviourInputHandlers;
using UnityEngine.InputSystem.LowLevel;
using TempScripts;
using Zenject;

namespace Input.MonoBehaviourHandlers
{
	public class LookInputHandler : MonoBehaviour, IInputHandler
	{
		[SerializeField] 
		private string _actionName = "Look";

		[Inject]
		private EcsWorld _world;

		private EcsPool<LookInputComponent> _lookInputPool;
		private FingerInputAction _fingerAction;
		private Vector2 _lastFrameFingerPosition;

		private void Start()
		{
			_lookInputPool = _world.GetPool<LookInputComponent>();
		}

		private void Update()
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
			ref var lookInput = ref _lookInputPool.Add(inputEntity);
			delta.y *= -1;
			lookInput.PointerDeltaInput = delta;
			_lastFrameFingerPosition = fingerScreenPosition;
		}

		private void OnDestroy()
		{
			_fingerAction.Dispose();
		}

		public void Initailize(InputActionMap actionMap)
		{
			var action = actionMap.FindAction(_actionName);
			_fingerAction = new FingerInputAction(action);
			_fingerAction.OnActionStarted += ActionStarted;
		}

		public void Enable()
		{
			_fingerAction.Enable();
		}

		public void Disable()
		{
			_fingerAction.Disable();
		}

		private void ActionStarted()
		{
			_lastFrameFingerPosition = _fingerAction.Finger.screenPosition;
		}
	}
}
