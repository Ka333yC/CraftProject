using System;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace _Scripts.Core.SettingsCore
{
	public class SettingsFileLoader
	{
		private const string FileName = "settings.json";

		private readonly string _settingsFilePath;

		public SettingsFileLoader()
		{
			_settingsFilePath = Path.Combine(Application.persistentDataPath, FileName);
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
			var serializedSettings = JsonConvert.SerializeObject(settingsData, Formatting.Indented);
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
