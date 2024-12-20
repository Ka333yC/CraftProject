using Assets.Scripts.Core.ChunkCore.Saving;
using Assets.Scripts.Core.WorldsCore;
using System;
using System.IO;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.SceneManagement.GameWorldScene
{
	public class DebugWorldLauncherInstaller : MonoInstaller
	{
		[SerializeField]
		private string _name = "debug_world";
		[SerializeField]
		private int _seed;

		[Inject]
		private WorldLauncher _worldLauncher;

		public override void InstallBindings()
		{
#if UNITY_EDITOR
			if(!gameObject.activeSelf || _worldLauncher.WorldParameters != null)
			{
				return;
			}

			var worldParameters = new WorldParameters();
			worldParameters.Id = -1;
			worldParameters.Name = _name;
			worldParameters.Seed = _seed;
			worldParameters.WorldFolderPath = Path.Combine(Application.persistentDataPath, worldParameters.Name);
			_worldLauncher.SetWorldParameters(worldParameters);
#endif
		}
	}
}
