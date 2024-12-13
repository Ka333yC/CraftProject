using System;
using UnityEngine;

namespace Assets.Scripts.Core
{
	public class PauseManager
	{
		private bool _isPaused;

		public event Action OnPaused;
		public event Action OnUnpaused;

		public PauseManager()
		{
			OnPaused += OnPause;
			OnUnpaused += OnUnpause;
		}

		public bool IsPaused
		{
			get
			{
				return _isPaused;
			}

			set
			{
				if(_isPaused == value)
				{
					return;
				}

				_isPaused = value;
				if(_isPaused)
				{
					OnPaused?.Invoke();
				}
				else
				{
					OnUnpaused?.Invoke();
				}
			}
		}

		private void OnPause()
		{
			Time.timeScale = 0;
		}

		private void OnUnpause()
		{
			Time.timeScale = 1;
		}
	}
}
