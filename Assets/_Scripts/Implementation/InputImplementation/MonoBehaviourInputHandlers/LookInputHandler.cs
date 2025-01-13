using _Scripts.Core.InputCore.Components;
using Assets._Scripts.Implementation.InputImplementation.MonoBehaviourInputHandlers;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Scripts.Core.InputCore.MonoBehaviourInputHandlers
{
	public class LookInputHandler : MonoBehaviourInputHandler
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

		public override void Initailize(InputActionMap actionMap)
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
