using System;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using static UnityEngine.InputSystem.InputAction;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

namespace _Scripts.Core.InputCore
{
	public class FingerInputAction : IDisposable
	{
		private InputAction _action;

		public FingerInputAction(InputAction action)
		{
			_action = action;
			_action.started += ActionStarted;
			Touch.onFingerUp += ActionCancelled;
		}

		public event Action OnActionStarted;
		public event Action OnActionCancelled;

		public Touch Touch
		{
			get
			{
				return Finger.currentTouch;
			}
		}

		public Finger Finger { get; private set; }

		public bool IsActive
		{
			get
			{
				return Finger != null;
			}
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
			if(Finger != null)
			{
				return;
			}

			double lastTouchStartTime = 0;
			foreach(var finger in Touch.activeFingers)
			{
				var touchStartTime = finger.currentTouch.startTime;
				if(touchStartTime > lastTouchStartTime)
				{
					Finger = finger;
					lastTouchStartTime = touchStartTime;
				}
			}

			OnActionStarted?.Invoke();
		}

		private void ActionCancelled(Finger finger)
		{
			if(Finger == finger)
			{
				Finger = null;
				OnActionCancelled?.Invoke();
			}
		}

		public void Dispose()
		{
			_action.started -= ActionStarted;
			Touch.onFingerUp -= ActionCancelled;
		}
	}
}
