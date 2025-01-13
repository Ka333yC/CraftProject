using System;
using System.IO;
using System.Threading.Tasks;
using _Scripts.Implementation.DataBaseImplementation.GameWorldDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB;
using _Scripts.Implementation.DataBaseImplementation.GameWorldsDataDB.Tables.GameWorldParametersTable;
using DataBase.DataBase.Commands;
using DataBase.DataBase.Commands.DataBaseCommands;
using DataBaseManagement;

namespace _Scripts.Implementation.UIImplementation.MainMenuSceneUI.CreateNewWorldPage
{
	public class CreateNewWorldModel
	{
		private readonly DataBaseCommandExecutor _commandExecutor;

		public CreateNewWorldModel(GameWorldsDBCommandExecutor gameWorldsDBCommandExecutor)
		{
			_commandExecutor = gameWorldsDBCommandExecutor.CommandExecutor;
		}

		public async Task<int> CreateWorld(string worldName)
		{
			var worldInDatabase = CreateWorldSettings(worldName);
			var insertOrReplaceCommand = GameWorldParameters.InsertOrReplaceCommand;
			insertOrReplaceCommand.Value = worldInDatabase;
			var insertCommandWithRowId = new SeveralCommand(insertOrReplaceCommand,
				new LastInsertRowIdCommand());
			var worldId = await _commandExecutor.ExecuteScalarAsync(insertCommandWithRowId).ConfigureAwait(false);
			return Convert.ToInt32(worldId);
		}

		public string TrimStringFromWrongSymbols(string value) 
		{
			return value.Trim(' ');
		}

		public async Task<bool> HasWorldWithName(string worldName)
		{
			var command = GameWorldParameters.ExistsWhereNameCommand;
			command.Name = worldName;
			bool hasValue = false;
			await _commandExecutor.ExecuteReaderAsync(command, (reader) =>
			{
				if(!reader.Read())
				{
					throw new Exception("The request returned nothing.");
				}

				var rawHasValue = reader.GetInt32(0);
				hasValue = rawHasValue == 1;
			});

			return hasValue;
		}

		private GameWorldParameters CreateWorldSettings(string worldName)
		{
			var result = new GameWorldParameters();
			result.Name = worldName;
			result.Seed = 10000;
			var worldFolderPath = Path.Combine(SaveFilePathes.PathToSaveFolder, worldName);
			result.WorldFolderPath = worldFolderPath;
			if(Directory.Exists(worldFolderPath))
			{
				Directory.Delete(worldFolderPath, true);
			}

			return result;
		}
	}
}
