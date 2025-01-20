using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Core
{
	public class MainThreadEventExecutor : MonoBehaviour
	{
		private static readonly object _lock = new object();
		private static readonly List<Action> _actionsQueue = new List<Action>();
		private static volatile bool _actionsQueueEmpty = true;

		public static void ExecuteInUpdate(Action action)
		{
			lock(_lock)
			{
				_actionsQueue.Add(action);
				_actionsQueueEmpty = false;
			}
		}

		private void Update()
		{
			if(_actionsQueueEmpty)
			{
				return;
			}

			Action[] actionsToExecute;
			lock(_lock)
			{
				var actionsCount = _actionsQueue.Count;
				actionsToExecute = System.Buffers.ArrayPool<Action>.Shared.Rent(actionsCount);
				for(int i = 0; i < actionsCount; i++)
				{
					actionsToExecute[i] = _actionsQueue[i];
				}
				
				_actionsQueue.Clear();
				_actionsQueueEmpty = true;
			}

			foreach(Action action in actionsToExecute)
			{
				action?.Invoke();
			}
			
			System.Buffers.ArrayPool<Action>.Shared.Return(actionsToExecute);
		}
	}
}
