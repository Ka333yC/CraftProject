using System.Collections.Generic;

namespace Extensions
{
	public static class DictionaryExtensions
	{
		public static void Add<K, V>(this Dictionary<K, V> dictionary, KeyValuePair<K, V> value)
		{
			dictionary.Add(value.Key, value.Value);
		}

		public static void RemoveRange<K, V>(this Dictionary<K, V> dictionary, IEnumerable<K> toRemove)
		{
			foreach(var key in toRemove)
			{
				dictionary.Remove(key);
			}
		}
	}
}
