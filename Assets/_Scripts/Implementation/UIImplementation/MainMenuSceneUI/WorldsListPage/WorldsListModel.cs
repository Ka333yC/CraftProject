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

		public async Task<List<int>> LoadWorldsId(CancellationToken token)
		{
			List<int> result = new List<int>();
			var selectCommand = GameWorldParameters.SelectCommand;
			await _commandExecutor.ExecuteReaderAsync(selectCommand, (reader) => 
			{
				while(reader.Read())
				{
					result.Add(reader.GetInt32(0));
					token.ThrowIfCancellationRequested();
				}
			});

			return result;
		}
	}
}
