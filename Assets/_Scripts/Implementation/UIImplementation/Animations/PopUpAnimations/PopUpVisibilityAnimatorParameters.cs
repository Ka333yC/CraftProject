using _Scripts.Core.UICore.Animation;
using UnityEngine;

namespace _Scripts.Implementation.UIImplementation.Animations.PopUpAnimations
{
	[CreateAssetMenu(fileName = "AnimationParameters", menuName = "Animation parameters/Pop up/Visibility animation parameters")]
	public class PopUpVisibilityAnimatorParameters : ScriptableObject
	{
		[field: SerializeField]
		public VisibilityAnimationParameter ShowAnimationParameter
		{
			get;
			private set;
		}

		[field: SerializeField]
		public VisibilityAnimationParameter HideAnimationParameter
		{
			get;
			private set;
		}
	}
}