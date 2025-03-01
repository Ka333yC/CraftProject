using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using DataBaseManagement;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldsListPage
{
	public class WorldsListModel
	{
		private readonly DataBaseCommandExecutor _commandExecutor;

		public WorldsListModel(GameWorldsDBCommandExecutor gameWorldsDBCommandExecutor)
		{
			_commandExecutor = gameWorldsDBCommandExecutor.CommandExecutor;
		}

		public async Task<List<GameWorldParameters>> LoadWorlds(CancellationToken token)
		{
			List<GameWorldParameters> result = new List<GameWorldParameters>();
			var selectCommand = GameWorldParameters.SelectCommand;
			await _commandExecutor.ExecuteReaderAsync(selectCommand, (reader) => 
			{
				while(reader.Read())
				{
					var gameWorldParameters = new GameWorldParameters();
					gameWorldParameters.Id = reader.GetInt32(0);
					gameWorldParameters.Name = reader.GetString(1);
					gameWorldParameters.Seed = reader.GetInt32(2);
					gameWorldParameters.WorldFolderPath = reader.GetString(3);
					result.Add(gameWorldParameters);
					token.ThrowIfCancellationRequested();
				}
			});

			return result;
		}
	}
}
