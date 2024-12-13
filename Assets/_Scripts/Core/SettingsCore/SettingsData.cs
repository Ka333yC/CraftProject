using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Undone.SettingsCore
{
	public class SettingsData
	{
		[JsonProperty]
		private readonly Dictionary<string, bool> _boolSettings = new Dictionary<string, bool>();
		[JsonProperty]
		private readonly Dictionary<string, int> _intSettings = new Dictionary<string, int>();
		[JsonProperty]
		private readonly Dictionary<string, float> _floatSettings = new Dictionary<string, float>();
		[JsonProperty]
		private readonly Dictionary<string, string> _stringSettings = new Dictionary<string, string>();

		public bool TryGetBool(string settingName, out bool settingValue)
		{
			return _boolSettings.TryGetValue(settingName, out settingValue);
		}

		public bool TryGetInt(string settingName, out int settingValue)
		{
			return _intSettings.TryGetValue(settingName, out settingValue);
		}

		public bool TryGetFloat(string settingName, out float settingValue)
		{
			return _floatSettings.TryGetValue(settingName, out settingValue);
		}

		public bool TryGetString(string settingName, out string settingValue)
		{
			return _stringSettings.TryGetValue(settingName, out settingValue);
		}

		public void SetBool(string settingName, bool settingValue)
		{
			if(_boolSettings.ContainsKey(settingName))
			{
				_boolSettings[settingName] = settingValue;
			}
			else
			{
				_boolSettings.Add(settingName, settingValue);
			}
		}

		public void SetInt(string settingName, int settingValue)
		{
			if(_intSettings.ContainsKey(settingName))
			{
				_intSettings[settingName] = settingValue;
			}
			else
			{
				_intSettings.Add(settingName, settingValue);
			}
		}

		public void SetFloat(string settingName, float settingValue)
		{
			if(_floatSettings.ContainsKey(settingName))
			{
				_floatSettings[settingName] = settingValue;
			}
			else
			{
				_floatSettings.Add(settingName, settingValue);
			}
		}

		public void SetString(string settingName, string settingValue)
		{
			if(_stringSettings.ContainsKey(settingName))
			{
				_stringSettings[settingName] = settingValue;
			}
			else
			{
				_stringSettings.Add(settingName, settingValue);
			}
		}

		public void Reset()
		{
			_boolSettings.Clear();
			_intSettings.Clear();
			_floatSettings.Clear();
			_stringSettings.Clear();
		}
	}
}
