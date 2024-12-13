using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Assets.Scripts.Implementation.UI.PopUps.Animations
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
