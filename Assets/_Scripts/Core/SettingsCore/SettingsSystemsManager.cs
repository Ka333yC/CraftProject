using Assets.Scripts.Core;
using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Playables;

namespace Assets.Scripts.Undone.SettingsCore
{
	public class SettingsSystemsManager
	{
		private readonly List<ISettingsSystem> _settingsSystems = new List<ISettingsSystem>();
		private readonly SettingsFileLoader _settingsFileLoader = new SettingsFileLoader();

		private SettingsData _settingsData;
		private bool _isLoaded;

		public async UniTaskVoid Initialize()
		{
			_settingsData = await _settingsFileLoader.Read();
			if(_settingsData == null)
			{
				_settingsData = new SettingsData();
			}

			foreach(var settingsSystem in _settingsSystems)
			{
				settingsSystem.GetFrom(_settingsData);
			}

			_isLoaded = true;
		}

		public void AddSystem(ISettingsSystem settingsSystem) 
		{
			_settingsSystems.Add(settingsSystem);
			if(_isLoaded)
			{
				settingsSystem.GetFrom(_settingsData);
			}
		}

		public async UniTaskVoid SaveData()
		{
			foreach(var settingsSystem in _settingsSystems)
			{
				settingsSystem.SetTo(_settingsData);
			}

			await _settingsFileLoader.Write(_settingsData);
		}
	}
}
