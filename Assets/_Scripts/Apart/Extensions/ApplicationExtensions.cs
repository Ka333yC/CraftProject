using System;
using UnityEngine;

namespace Extensions
{
	public static class ApplicationExtensions
	{
		public static void ThrowOperationCanceledExceptionIfNotPlaying()
		{
			if(!Application.isPlaying)
			{
				throw new OperationCanceledException();
			}
		}
	}
}
