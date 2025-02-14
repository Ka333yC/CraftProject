using System;
using System.Threading;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkGeneration.Elements;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.PhysicsCore.Presets;
using _Scripts.Implementation.BlocksImplementation;
using UnityEngine;
using Zenject;

namespace _Scripts.TempScripts
{
	public class Singleton : MonoBehaviour
	{
		public static Singleton Instance { get; private set; }

		[field: SerializeField]
		public BlockArchetype BlockToSpawn { get; private set; }

		[field: SerializeField]
		public NoiseSettings NoiseSettings { get; private set; }

		private void Awake()
		{
			Instance = this;
			var count = Environment.ProcessorCount;
			ThreadPool.SetMinThreads(count, count);
			ThreadPool.SetMaxThreads(count, count);
#if !UNITY_EDITOR && UNITY_ANDROID
			QualitySettings.vSyncCount = 0;
			Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif
		}
	}
}
