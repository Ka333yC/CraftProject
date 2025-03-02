using System.Collections.Generic;
using _Scripts.Core.GameProgressCore;
using Cysharp.Threading.Tasks;

namespace _Scripts.Core.SettingsCore
{
	public class SettingsSystemsManager : ISerializableDataSystem
	{
		private readonly List<ISettingsSystem> _settingsSystems = new List<ISettingsSystem>();

		private SettingsData _settingsData;
		private bool _isInitialized;
		
		public void LoadFrom(SerializableDataContainer dataContainer)
		{
			_settingsData = new SettingsData(dataContainer);
			foreach(var settingsSystem in _settingsSystems)
			{
				settingsSystem.LoadFrom(_settingsData);
			}

			_isInitialized = true;
		}

		public void SaveTo(SerializableDataContainer dataContainer)
		{
			foreach(var settingsSystem in _settingsSystems)
			{
				settingsSystem.SaveTo(_settingsData);
			}
		}

		public void AddSystem(ISettingsSystem settingsSystem) 
		{
			_settingsSystems.Add(settingsSystem);
			if(_isInitialized)
			{
				settingsSystem.LoadFrom(_settingsData);
			}
		}
	}
}
