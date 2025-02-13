using _Scripts.Core.UICore.Animation;
using UnityEngine;

namespace _Scripts.Implementation.UIImplementation.Animations.PageAnimations
{
	[CreateAssetMenu(fileName = "AnimationParameters", menuName = "Animation parameters/Page/Position animation parameters")]
	public class PagePositionAnimatorParameters : ScriptableObject
	{
		[field: SerializeField]
		public PositionAnimationParameter ShowAnimationParameter
		{
			get;
			private set;
		}

		[field: SerializeField]
		public PositionAnimationParameter MoveOutAnimationParameter
		{
			get;
			private set;
		}

		[field: SerializeField]
		public PositionAnimationParameter MoveInAnimationParameter
		{
			get;
			private set;
		}

		[field: SerializeField]
		public PositionAnimationParameter HideAnimationParameter
		{
			get;
			private set;
		}
	}
}
