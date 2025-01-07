using System.Linq;
using _Scripts.Core.UICore.PopUp;
using UnityEngine;
using Zenject;

namespace _Scripts.Core.UICore.Page
{
	[RequireComponent(typeof(BasePageAnimator), typeof(CanvasGroup))]
	public abstract class BasePageView : MonoBehaviour
	{
		public PageViewStack PageStack;

		protected BasePageAnimator _animator;
		protected CanvasGroup _canvasGroup;

		[field: SerializeField]
		public bool NeedShowAdsOnClose { get; private set; } = true;

		public PopUpViewStack PopUpStack { get; private set; }

		[Inject]
		protected ViewFactory ViewFactory { get; private set; }

		protected virtual void Awake()
		{
			PopUpStack = new PopUpViewStack(this);
			_animator = GetComponent<BasePageAnimator>();
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		[Inject]
		public void SetRootCanvas(RootCanvas canvas)
		{
			gameObject.transform.SetParent(canvas.transform, false);
		}

		public virtual void Open(bool playAnimation = true)
		{
			_canvasGroup.interactable = true;
			if(playAnimation)
			{
				_animator.Show();
			}
			else
			{
				_animator.ShowAtEndFrame();
			}
		}

		public virtual void MoveOut()
		{
			_canvasGroup.interactable = false;
			_animator.MoveOut(() => gameObject.SetActive(false));
		}

		public virtual void MoveIn()
		{
			_canvasGroup.interactable = true;
			gameObject.SetActive(true);
			_animator.MoveIn();
		}

		public virtual void Close(bool playAnimation = true)
		{
			_canvasGroup.interactable = false;
			if(playAnimation)
			{
				_animator.Hide(() => GameObject.Destroy(gameObject));
			}
			else
			{
				GameObject.Destroy(gameObject);
			}
		}

		/// <summary>
		/// ����� ��� ������ �� ������� escape button
		/// </summary>
		public virtual void Escape()
		{
			if(PopUpStack.Any())
			{
				PopUpStack.Last().Escape();
				return;
			}

			PageStack.CloseLastView();
		}
	}
}
