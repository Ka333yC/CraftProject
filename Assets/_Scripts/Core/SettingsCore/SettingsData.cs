using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace _Scripts.Core.SettingsCore
{
	public class SettingsData
	{
		[JsonConverter(typeof(TypedDictionaryConverter))]
		[JsonProperty]
		private readonly Dictionary<string, object> _data = new Dictionary<string, object>();
		[JsonProperty]
		private readonly Dictionary<string, Type> _dataTypes = new Dictionary<string, Type>();

		/// <summary>
		/// Throws InvalidCastException if the types do not match
		/// </summary>
		public bool TryGet<T>(string dataName, out T dataValue)
		{
			if(_data.TryGetValue(dataName, out object savedDataValue))
			{
				dataValue = (T)savedDataValue;
				return true;
			}

			dataValue = default;
			return false;
		}

		public void Set<T>(string dataName, T dataValue)
		{
			if(_data.ContainsKey(dataName))
			{
				if(_dataTypes[dataName] != typeof(T))
				{
					throw new InvalidCastException();
				}
				
				_data[dataName] = dataValue;
			}
			else 
			{
				_data.Add(dataName, dataValue);
				_dataTypes.Add(dataName, typeof(T));
			}
		}

		public void Reset(string dataName)
		{
			if(_data.Remove(dataName))
			{
				_dataTypes.Remove(dataName);
			}
		}

		public void ResetAll()
		{
			_data.Clear();
			_dataTypes.Clear();
		}
	}
}
