using System;
using _Scripts.Core.UICore.Animation;
using _Scripts.Core.UICore.PopUp;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Implementation.UIImplementation.Animations.PopUpAnimations
{
	[RequireComponent(typeof(CanvasGroup))]
	public class PopUpVisibilityAnimator : BasePopUpAnimator
	{
		[SerializeField] 
		private PopUpVisibilityAnimatorParameters _animatorParameters;
		
		private Sequence _nowAnimation;
		private CanvasGroup _canvasGroup;

		private void Awake()
		{
			_nowAnimation = DOTween.Sequence();
			// сразу "убиваю" анимацию, потому что по умолчанию она IsPlaying = true
			_nowAnimation.Kill(true);
			_canvasGroup = GetComponent<CanvasGroup>();
		}

		private void OnDestroy()
		{
			_nowAnimation.Kill();
		}

		public override void Show(Action onComplete = null)
		{
			var showAnimationParameter = _animatorParameters.ShowAnimationParameter;
			_canvasGroup.alpha = showAnimationParameter.AnimationStartAlpha;
			_nowAnimation = StartAnimation(showAnimationParameter);
			_nowAnimation.onComplete += () => onComplete?.Invoke();
		}

		public override void Hide(Action onComplete = null)
		{
			_nowAnimation.Kill();
			_nowAnimation = StartAnimation(_animatorParameters.HideAnimationParameter);
			_nowAnimation.onComplete += () => onComplete?.Invoke();
		}

		private Sequence StartAnimation(VisibilityAnimationParameter animationParameters)
		{
			float duration = GetDuration(animationParameters);
			Tween animationCanvasGroup = _canvasGroup
				.DOFade(animationParameters.AnimationEndAlpha, duration);
			Sequence result = DOTween.Sequence();
			result.Append(animationCanvasGroup);
			return result;
		}

		private float GetDuration(VisibilityAnimationParameter animationParameters)
		{
			float startAlpha = animationParameters.AnimationStartAlpha;
			float endAlpha = animationParameters.AnimationEndAlpha;
			float fromStartAlphaToEndMagnitude = Mathf.Abs(endAlpha - startAlpha);
			float fromNowAlphaToEndMagnitude = Mathf.Abs(endAlpha - _canvasGroup.alpha);
			float duration = fromNowAlphaToEndMagnitude / fromStartAlphaToEndMagnitude * animationParameters.AbsolutDuration;
			return duration;
		}
	}
}