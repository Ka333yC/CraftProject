using Assets.Scripts.Core.ChunkCore.Saving;
using Assets.Scripts.Undone;
using Assets.Scripts.Undone.TerrainGeneration;
using ChunkCore.LifeTimeControl;
using Cysharp.Threading.Tasks;
using DataBaseManagement;
using System.Threading;
using System;
using System.Threading.Tasks;
using TempScripts.TerrainGeneration;
using UnityEngine;
using Zenject;
using Leopotam.EcsLite;
using Assets.Scripts.PhysicsCore;
using Assets._Scripts.Implementation.BlocksImplementation;
using Assets._Scripts.Core.ChunkCore.ChunkLogic.Components;

namespace TempScripts
{
	public class Singleton : MonoBehaviour
	{
		//[Inject]
		//private GameWorldDBCommandExecutor _commandExecutor;
		//[Inject]
		//private WorldLauncher _worldLauncher;
		[Inject]
		private PhysicsPresets _physicsPresets;
		[Inject]
		private EcsWorld _ecsWorld;

		public static Singleton Instance { get; private set; }

		[field: SerializeField]
		public ChunkGameObject ChunkPrefab { get; private set; } // В DI

		[field: SerializeField]
		public BlockContainer BlockToSpawn { get; private set; } // В DI

		//[field: SerializeField]
		//public EcsGameStartup EcsGameStartup { get; private set; } // В DI

		[field: SerializeField]
		public NoiseSettings NoiseSettings { get; private set; }

		public PhysicsPresets PhysicsSettings => _physicsPresets;

		public EcsWorld EcsWorld => _ecsWorld;

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
