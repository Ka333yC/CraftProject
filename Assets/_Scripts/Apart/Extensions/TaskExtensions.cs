using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Scripts.Apart.Extensions
{
	public static class TaskExtensions
	{
		public static async void LogException(this Task task)
		{
			try
			{
				await task;
			}
			catch(Exception exception) when(exception is not OperationCanceledException)
			{
				if(!Application.isPlaying)
				{
					return;
				}

				Debug.LogException(exception);
			}
		}

		public static async void LogException(this UniTask task)
		{
			try
			{
				await task;
			}
			catch(Exception exception) when(exception is not OperationCanceledException)
			{
				if(!Application.isPlaying)
				{
					return;
				}

				Debug.LogException(exception);
			}
		}
	}
}
