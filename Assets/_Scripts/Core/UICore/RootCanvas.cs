using UnityEngine;

namespace _Scripts.Core.UICore
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
