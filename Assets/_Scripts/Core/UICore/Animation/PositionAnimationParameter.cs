using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Assets.Scripts.Implementation.UI.Pages.Animations
{
	[Serializable]
	public class PositionAnimationParameter
	{
		[field: SerializeField]
		public Vector2 AnimationStartPosition
		{
			get;
			private set;
		}

		[field: SerializeField]
		public Vector2 AnimationEndPosition
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
