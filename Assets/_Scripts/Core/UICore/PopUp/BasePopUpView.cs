using UnityEngine;
using Zenject;

namespace _Scripts.Core.UICore.PopUp
{
	[RequireComponent(typeof(BasePopUpAnimator), typeof(CanvasGroup))]
	public class BasePopUpView : MonoBehaviour
	{
		protected PopUpViewStack _stack;
		protected BasePopUpAnimator _animator;
		protected CanvasGroup _canvasGroup;

		public PopUpViewStack Stack
		{
			get
			{
				return _stack;
			}

			set
			{
				_stack = value;
				transform.SetParent(value.ParentPage.transform, false);
			}
		}

		[Inject]
		protected ViewFactory ViewFactory { get; private set; }

		private void Awake()
		{
			_animator = GetComponent<BasePopUpAnimator>();
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		public virtual void Open()
		{
			_canvasGroup.interactable = true;
			_animator.Show();
		}

		public virtual void Close()
		{
			_canvasGroup.interactable = false;
			_animator.Hide(() => GameObject.Destroy(gameObject));
		}

		/// <summary>
		/// Метод для вызова на нажатие escape button
		/// </summary>
		public virtual void Escape()
		{
			Stack.CloseLastView();
		}
	}
}
