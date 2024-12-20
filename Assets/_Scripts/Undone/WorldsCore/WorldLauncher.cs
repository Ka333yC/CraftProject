using Cysharp.Threading.Tasks;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Zenject;

namespace Assets.Scripts.Core.WorldsCore
{
	public class WorldLauncher
	{
		[Inject]
		private WorldsDataDBCommandExecutor _commandExecutor;

		public WorldParameters WorldParameters { get; private set; }

		public async UniTask LaunchWorld(int worldId)
		{
			WorldParameters = await GetWorldSettings(worldId);
			CreateWorldDirectory();
			await LoadScene();
		}

#if UNITY_EDITOR
		public void SetWorldParameters(WorldParameters worldParameters)
		{
			WorldParameters = worldParameters;
			CreateWorldDirectory();
		}
#endif

		private async Task<WorldParameters> GetWorldSettings(int worldId)
		{
			var commandExecutor = _commandExecutor.CommandExecutor;
			var command = WorldParameters.SelectWhereIdCommand;
			command.Id = worldId;
			var worldParameters = new WorldParameters();
			await commandExecutor.ExecuteReaderAsync(command, (reader) =>
			{
				if(!reader.Read())
				{
					throw new ArgumentException("Запрос ничего не вернул.");
				}

				worldParameters.Id = worldId;
				worldParameters.Name = reader.GetString(0);
				worldParameters.Seed = reader.GetInt32(1);
				worldParameters.WorldFolderPath = reader.GetString(2);
			});

			return worldParameters;
		}

		private void CreateWorldDirectory()
		{
			if(!Directory.Exists(WorldParameters.WorldFolderPath))
			{
				Directory.CreateDirectory(WorldParameters.WorldFolderPath);
			}
		}

		private async UniTask LoadScene() 
		{
			var asyncOperation = SceneManager.LoadSceneAsync(ScenesIndexes.GameWorldSceneIndex);
			await asyncOperation.ToUniTask();
		}
	}
}
