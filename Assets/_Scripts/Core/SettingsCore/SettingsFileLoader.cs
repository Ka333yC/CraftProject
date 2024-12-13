using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Undone.SettingsCore
{
	public class SettingsFileLoader
	{
		private const string _fileName = "settings.json";

		private readonly string _settingsFilePath;

		public SettingsFileLoader()
		{
			_settingsFilePath = Path.Combine(Application.persistentDataPath, _fileName);
		}

		public async UniTask Write(SettingsData settingsData) 
		{
			await UniTask.RunOnThreadPool(() => WriteInternal(settingsData));
		}

		public UniTask<SettingsData> Read() 
		{
			return UniTask.RunOnThreadPool(ReadInternal);
		}

		private void WriteInternal(SettingsData settingsData) 
		{
			var serializedSettings = JsonConvert.SerializeObject(settingsData);
			File.WriteAllText(_settingsFilePath, serializedSettings);
		}

		private SettingsData ReadInternal() 
		{
			try
			{
				if(!File.Exists(_settingsFilePath))
				{
					return null;
				}

				var serializedSettings = File.ReadAllText(_settingsFilePath);
				return JsonConvert.DeserializeObject<SettingsData>(serializedSettings);
			}
			catch(Exception)
			{
				return null;
			}			
		}
	}
}
