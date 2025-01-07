using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Core
{
	public class MainThreadEventExecutor : MonoBehaviour
	{
		private static volatile bool _adEventsQueueEmpty = true;
		private static List<Action> _adEventsQueue = new List<Action>();

		public static void ExecuteInUpdate(Action action)
		{
			lock(_adEventsQueue)
			{
				_adEventsQueue.Add(action);
				_adEventsQueueEmpty = false;
			}
		}

		private void Update()
		{
			if(_adEventsQueueEmpty)
			{
				return;
			}

			List<Action> list = new List<Action>();
			lock(_adEventsQueue)
			{
				list.AddRange(_adEventsQueue);
				_adEventsQueue.Clear();
				_adEventsQueueEmpty = true;
			}

			foreach(Action item in list)
			{
				if(item.Target != null)
				{
					item();
				}
			}
		}
	}
}
