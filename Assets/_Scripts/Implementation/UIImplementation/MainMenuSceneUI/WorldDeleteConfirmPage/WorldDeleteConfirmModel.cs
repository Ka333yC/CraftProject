using System;
using System.IO;
using System.Threading.Tasks;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using DataBaseManagement;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.WorldDeleteConfirmPage
{
	public class WorldDeleteConfirmModel
	{
		private readonly DataBaseCommandExecutor _commandExecutor;

		public WorldDeleteConfirmModel(GameWorldsDBCommandExecutor gameWorldsDBCommandExecutor)
		{
			_commandExecutor = gameWorldsDBCommandExecutor.CommandExecutor;
		}

		public async Task DeleteWorld(int worldId)
		{
			var worldFolderPath = await GetWorldFolderPath(worldId);
			Directory.Delete(worldFolderPath, true);
			await DeleteRecordFromDatabase(worldId);
		}

		private async Task<string> GetWorldFolderPath(int worldId)
		{
			var command = GameWorldParameters.SelectWhereIdCommand;
			command.Id = worldId;
			string worldFolderPath = null;
			await _commandExecutor.ExecuteReaderAsync(command, (reader) => 
			{
				if(!reader.Read())
				{
					throw new ArgumentException("The request returned nothing.");
				}

				worldFolderPath = reader.GetString(2);
			});

			return worldFolderPath;
		}

		private async Task DeleteRecordFromDatabase(int worldId)
		{
			var command = GameWorldParameters.DeleteCommand;
			command.Id = worldId;
			await _commandExecutor.ExecuteNonQueryAsync(command);
		}
	}
}
