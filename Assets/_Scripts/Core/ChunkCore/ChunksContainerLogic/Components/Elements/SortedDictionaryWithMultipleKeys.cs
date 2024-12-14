using System.Collections.Generic;

namespace Assets._Code.Core.ChunkCore.ChunksContainerLogic.Components.Elements
{
	public class SortedDictionaryWithMultipleKeys<TKey, TValue>
	{
		private readonly SortedDictionary<TKey, HashSet<TValue>> _dictionary =
			new SortedDictionary<TKey, HashSet<TValue>>();
		private readonly Queue<HashSet<TValue>> _hashSetPool = new Queue<HashSet<TValue>>();

		public bool Add(TKey key, TValue value)
		{
			if(!_dictionary.TryGetValue(key, out var values))
			{
				values = Rent();
				_dictionary.Add(key, values);
			}

			return values.Add(value);
		}

		public bool Remove(TKey key, TValue value)
		{
			if(!_dictionary.TryGetValue(key, out var values) || !values.Remove(value))
			{
				return false;
			}

			if(values.Count == 0)
			{
				_dictionary.Remove(key);
				Return(values);
			}

			return true;
		}

		public bool TryGetFirst(out TValue value)
		{
			using var enumerator = _dictionary.GetEnumerator();
			if(!enumerator.MoveNext())
			{
				value = default;
				return false;
			}

			using var valuesEnumerator = enumerator.Current.Value.GetEnumerator();
			valuesEnumerator.MoveNext();
			value = valuesEnumerator.Current;
			return true;
		}

		private HashSet<TValue> Rent()
		{
			return _hashSetPool.Count == 0 ? new HashSet<TValue>() : _hashSetPool.Dequeue();
		}

		private void Return(HashSet<TValue> hashSet)
		{
			_hashSetPool.Enqueue(hashSet);
		}
	}
}