using System;
using UnityEngine;

namespace _Scripts.Core
{
	public class MonoBehaviourEventsHandler : MonoBehaviour
	{
		public static event Action<bool> OnApplicationFocusEvent;
		public static event Action<bool> OnApplicationPauseEvent;
		public static event Action OnApplicationQuitEvent;

		private void Awake()
		{
			DontDestroyOnLoad(gameObject);
		}

		private void OnApplicationFocus(bool focus)
		{
			OnApplicationFocusEvent?.Invoke(focus);
		}

		private void OnApplicationPause(bool pause)
		{
			OnApplicationPauseEvent?.Invoke(pause);
		}

		private void OnApplicationQuit()
		{
			OnApplicationQuitEvent?.Invoke();
		}
	}
}
