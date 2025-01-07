using System;
using UnityEngine;

namespace _Scripts.Core.UICore.Animation
{
	[Serializable]
	public class VisibilityAnimationParameter
	{
		[field: SerializeField]
		public float AnimationStartAlpha
		{
			get;
			private set;
		}

		[field: SerializeField]
		public float AnimationEndAlpha
		{
			get;
			private set;
		}

		[field: SerializeField, Min(0)]
		public float AbsolutDuration
		{
			get;
			private set;
		}
	}
}
