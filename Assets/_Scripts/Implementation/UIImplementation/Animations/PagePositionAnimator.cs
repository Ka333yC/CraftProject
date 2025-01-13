using System;
using _Scripts.Core.UICore.Animation;
using _Scripts.Core.UICore.Page;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Implementation.UIImplementation.Animations
{
	public class PagePositionAnimator : BasePageAnimator
	{
		[SerializeField] 
		private PositionAnimatorParameters _animatorParameters;

		private Sequence _nowAnimation;
		private RectTransform _transform;
		private Vector2? _canvasSize;

		private Vector2 CanvasSize
		{
			get
			{
				if(!_canvasSize.HasValue)
				{
					var canvas = GetComponentInParent<Canvas>();
					_canvasSize = ((RectTransform)canvas.transform).sizeDelta;
				}

				return _canvasSize.Value;
			}
		}

		private void Awake()
		{
			_nowAnimation = DOTween.Sequence();
			// сразу "убиваю" анимацию, потому что по умолчанию она IsPlaying = true
			_nowAnimation.Kill(true);
			_transform = GetComponent<RectTransform>();
		}

		private void OnDestroy()
		{
			_nowAnimation.Kill();
		}

		public override void Show(Action onComplete = null)
		{
			var showAnimationParameter = _animatorParameters.ShowAnimationParameter;
			_transform.localPosition = showAnimationParameter.AnimationStartPosition * CanvasSize;
			_nowAnimation = StartAnimation(showAnimationParameter);
			_nowAnimation.onComplete += () => onComplete?.Invoke();
		}

		public override void ShowAtEndFrame()
		{
			_transform.localPosition = _animatorParameters.ShowAnimationParameter.AnimationEndPosition * CanvasSize;
		}

		public override void MoveOut(Action onComplete = null)
		{
			_nowAnimation.Kill();
			_nowAnimation = StartAnimation(_animatorParameters.MoveOutAnimationParameter);
			_nowAnimation.onComplete += () => onComplete?.Invoke();
		}

		public override void MoveIn(Action onComplete = null)
		{
			_nowAnimation.Kill();
			_nowAnimation = StartAnimation(_animatorParameters.MoveInAnimationParameter);
			_nowAnimation.onComplete += () => onComplete?.Invoke();
		}

		public override void Hide(Action onComplete = null)
		{
			_nowAnimation.Kill();
			_nowAnimation = StartAnimation(_animatorParameters.HideAnimationParameter);
			_nowAnimation.onComplete += () => onComplete?.Invoke();
		}

		private Sequence StartAnimation(PositionAnimationParameter animationParameters)
		{
			Vector2 endPosition = animationParameters.AnimationEndPosition * CanvasSize;
			float duration = GetDuration(animationParameters);
			Tween animationByX = _transform
				.DOLocalMoveX(endPosition.x, duration);
			Tween animationByY = _transform
				.DOLocalMoveY(endPosition.y, duration);
			Sequence result = DOTween.Sequence();
			result.Append(animationByX);
			result.Append(animationByY);
			return result;
		}

		private float GetDuration(PositionAnimationParameter animationParameters)
		{
			Vector2 startPosition = animationParameters.AnimationStartPosition * CanvasSize;
			Vector2 endPosition = animationParameters.AnimationEndPosition * CanvasSize;
			float fromEndToStartPositionMagnitude = (endPosition - startPosition).magnitude;
			float fromEndToNowPositionMagnitude = (endPosition - (Vector2)_transform.localPosition).magnitude;
			// Если начальная и конечная точки совпадают, то длительность анимации = 0
			if(Mathf.Approximately(fromEndToStartPositionMagnitude, 0))
			{
				return 0;
			}

			float duration = fromEndToNowPositionMagnitude / fromEndToStartPositionMagnitude 
				* animationParameters.AbsolutDuration;
			return duration;
		}
	}
}
