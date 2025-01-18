using System;
using _Scripts.Implementation.InputImplementation.Components;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Scripts.Implementation.InputImplementation.MonoBehaviourInputHandlers
{
	public class TapInputHandler : MonoBehaviourInputHandler
	{
		[SerializeField] 
		private string _actionName = "Interaction";
		[SerializeField] 
		private double _useDuration = 0.16;

		[Inject]
		private EcsWorld _world;

		private EcsPool<TapInputComponent> _tapInputPool;
		private FingerInputAction _fingerAction;
		private double _actionStartTime;
		private Vector2 _startPressPosition;
		private Vector2? _inputToNotify;

		public Vector2? Input { get; private set; }

		private void Start()
		{
			_tapInputPool = _world.GetPool<TapInputComponent>();
		}

		private void FixedUpdate()
		{
			Input = null;
			if(!_inputToNotify.HasValue)
			{
				return;
			}
			
			var useInputEntity = _world.NewEntity();
			ref var tapInput = ref _tapInputPool.Add(useInputEntity);
			tapInput.ScreenPointerPositionInput = _inputToNotify.Value;
			Input = _inputToNotify.Value;
			_inputToNotify = null;
		}

		private void OnDestroy()
		{
			_fingerAction.Dispose();
		}

		public override void Initailize(InputActionMap actionMap)
		{
			_fingerAction = new FingerInputAction(actionMap, _actionName);
			_fingerAction.OnActionStarted += ActionStarted;
			_fingerAction.OnActionCancelled += ActionCanceled;
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
			_actionStartTime = Time.realtimeSinceStartup;
			_startPressPosition = _fingerAction.Touch.startScreenPosition;
		}

		private void ActionCanceled()
		{
			if(Time.realtimeSinceStartup - _actionStartTime < _useDuration)
			{
				_inputToNotify = _startPressPosition;
			}

			_startPressPosition = Vector2.zero;
			_actionStartTime = 0;
		}
	}
}
