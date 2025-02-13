using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using _Scripts.Core.SettingsCore;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB;
using UnityEngine;
using Zenject;

namespace _Scripts.Implementation.SceneManagement
{
	public class GlobalDataInstaller : MonoInstaller
	{
		[SerializeField]
		private ChunkGameObject _chunkPrefab;
		
		public override void InstallBindings()
		{
			InstallScenesLauncher();
			InstallGameWorldsDBCommandExecutor();
			InstallChunkPrefab();
		}

		private void InstallScenesLauncher()
		{
			Container
				.BindInterfacesAndSelfTo<ScenesLauncher>()
				.FromNew()
				.AsSingle();
		}

		private void InstallGameWorldsDBCommandExecutor()
		{
			Container
				.BindInterfacesAndSelfTo<GameWorldsDBCommandExecutor>()
				.FromNew()
				.AsSingle();
		}

		private void InstallChunkPrefab()
		{
			Container
				.Bind<ChunkGameObject>()
				.FromInstance(_chunkPrefab)
				.AsSingle();
		}
	}
}
