using UnityEngine;

namespace Assets.Scripts.Core.UICore.Core
{
	[RequireComponent(typeof(Canvas))]
	public class RootCanvas : MonoBehaviour
	{
		public Canvas Canvas
		{
			get;
			private set;
		}

		private void Awake()
		{
			Canvas = GetComponent<Canvas>();
		}
	}
}
