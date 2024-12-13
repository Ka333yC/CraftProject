using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Assets.Scripts.Core.UICore.PopUp
{
	public abstract class BasePopUpAnimator : MonoBehaviour
	{
		public abstract void Show(Action onComplete = null);
		public abstract void Hide(Action onComplete = null);
	}
}
