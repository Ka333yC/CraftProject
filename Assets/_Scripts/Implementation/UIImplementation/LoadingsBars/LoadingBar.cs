using UnityEngine;

namespace _Scripts.Implementation.UIImplementation.LoadingsBars
{
	public abstract class LoadingBar : MonoBehaviour
	{
		public abstract void UpdateProgress(float value);
	}
}
