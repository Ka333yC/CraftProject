using System;
using System.Collections.Generic;

namespace Zenject
{
    public class ListPool<T> : StaticMemoryPool<List<T>>
    {
        static ListPool<T> _instance = new ListPool<T>();

        public ListPool()
        {
            OnDespawnedMethod = OnDespawned;
        }

        public static ListPool<T> Instance
        {
            get { return _instance; }
        }

		public static List<string> Get()
		{
			throw new NotImplementedException();
		}

		void OnDespawned(List<T> list)
        {
            list.Clear();
        }
    }
}
