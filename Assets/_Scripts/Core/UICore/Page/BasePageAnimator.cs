using System;
using UnityEngine;

namespace _Scripts.Core.UICore.Page
{
	public abstract class BasePageAnimator : MonoBehaviour
	{
		public abstract void Show(Action onComplete = null);
		public abstract void ShowAtEndFrame();
		public abstract void MoveOut(Action onComplete = null);
		public abstract void MoveIn(Action onComplete = null);
		public abstract void Hide(Action onComplete = null);
	}
}
