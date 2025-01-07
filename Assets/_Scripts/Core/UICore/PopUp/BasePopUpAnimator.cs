using System;
using UnityEngine;

namespace _Scripts.Core.UICore.PopUp
{
	public abstract class BasePopUpAnimator : MonoBehaviour
	{
		public abstract void Show(Action onComplete = null);
		public abstract void Hide(Action onComplete = null);
	}
}
