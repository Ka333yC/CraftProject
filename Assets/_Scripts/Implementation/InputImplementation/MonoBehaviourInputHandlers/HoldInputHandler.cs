using System;
using System.Threading;
using System.Threading.Tasks;
using _Scripts.Implementation.InputImplementation.Components;
using Leopotam.EcsLite;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _Scripts.Implementation.InputImplementation.MonoBehaviourInputHandlers
{
	public class HoldInputHandler : MonoBehaviourInputHandler
	{
		[SerializeField] 
		private string _actionName = "Interaction";
		[SerializeField] 
		private float _holdTime = 0.5f;
		[SerializeField] 
		private float _maxPressOffset = 1;

		[Inject]
		private EcsWorld _world;

		private EcsPool<HoldInputComponent> _holdInputPool;
		private FingerInputAction _fingerAction;
		private bool _isPressHoldedAtOnePosition;
		private CancellationTokenSource _holdCancellationToken;

		public Vector2? Input { get; private set; }

		private void Start()
		{
			_holdInputPool = _world.GetPool<HoldInputComponent>();
		}

		private void FixedUpdate()
		{
			if(!_isPressHoldedAtOnePosition || !_fingerAction.IsActive)
			{
				return;
			}

			var inputEntity = _world.NewEntity();
			ref var holdInput = ref _holdInputPool.Add(inputEntity);
			var touchPosition = _fingerAction.Finger.screenPosition;
			holdInput.ScreenPointerPositionInput = touchPosition;
			Input = touchPosition;
		}

		private void OnDestroy()
		{
			_fingerAction.Dispose();
		}

		public override void Initialize(InputActionMap actionMap)
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

		private async void ActionStarted()
		{
			var cancellationToken = new CancellationTokenSource();
			_holdCancellationToken = cancellationToken;
			try
			{
				var token = cancellationToken.Token;
				await Task.Delay(TimeSpan.FromSeconds(_holdTime), token);
				token.ThrowIfCancellationRequested();
				var touch = _fingerAction.Finger.currentTouch;
				var deltaFromNowToStartPosition = touch.screenPosition - touch.startScreenPosition;
				_isPressHoldedAtOnePosition = deltaFromNowToStartPosition.magnitude < _maxPressOffset;
			}
			catch(OperationCanceledException)
			{
			}
			finally
			{
				cancellationToken.Dispose();
				if(_holdCancellationToken == cancellationToken)
				{
					_holdCancellationToken = null;
				}
			}
		}

		private void ActionCanceled()
		{
			_isPressHoldedAtOnePosition = false;
			_holdCancellationToken?.Cancel();
			Input = null;
		}
	}
}
