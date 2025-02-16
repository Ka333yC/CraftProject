using System;
using System.Collections.Generic;
using _Scripts.Core.SettingsCore;
using UnityEngine;

namespace _Scripts.Implementation.SettingsImplementation.GraphicsSettings
{
	public class GraphicsSettingsSystem : ISettingsSystem
	{
		private int _loadingRange = 0;
		private List<int> _someObject = null;

		public int LoadingRange
		{
			get => _loadingRange;
			set
			{
				if(value < 2)
				{
					throw new ArgumentException("Loading Range cannot be less then 2");
				}

				_loadingRange = value;
			}
		}

		public GraphicsSettingsSystem(SettingsSystemsManager settingsSystemsManager)
		{
			settingsSystemsManager.AddSystem(this);
		}

		public void GetFrom(SettingsData settingsData)
		{
			if(settingsData.TryGet(nameof(LoadingRange), out int loadingRange))
			{
				LoadingRange = loadingRange;
			}
			else
			{
				LoadingRange = 8;
			}

			if(settingsData.TryGet(nameof(_someObject), out _someObject))
			{
				_someObject.Add(UnityEngine.Random.Range(0, 110));
				foreach(var value in _someObject)
				{
					Debug.Log(value);
				}
			}
			else
			{
				_someObject = new List<int>();
				_someObject.Add(UnityEngine.Random.Range(0, 110));
			}
		}

		public void SetTo(SettingsData settingsData)
		{
			settingsData.Set(nameof(LoadingRange), LoadingRange);
			settingsData.Set(nameof(_someObject), _someObject);
		}
	}
}
