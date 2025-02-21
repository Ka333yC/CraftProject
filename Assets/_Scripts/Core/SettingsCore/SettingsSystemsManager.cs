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
		
		public void Initialize(SerializableDataContainer dataContainer)
		{
			_settingsData = new SettingsData(dataContainer);
			foreach(var settingsSystem in _settingsSystems)
			{
				settingsSystem.Initialize(_settingsData);
			}

			_isInitialized = true;
		}

		public void WriteTo(SerializableDataContainer dataContainer)
		{
			foreach(var settingsSystem in _settingsSystems)
			{
				settingsSystem.WriteTo(_settingsData);
			}
		}

		public void AddSystem(ISettingsSystem settingsSystem) 
		{
			_settingsSystems.Add(settingsSystem);
			if(_isInitialized)
			{
				settingsSystem.Initialize(_settingsData);
			}
		}
	}
}
