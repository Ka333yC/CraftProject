using DataBase.Commands;
using System;

namespace Assets.Scripts.Core.WorldsCore.WorldSettingsTable.WorldSettingsCommands
{
	public class SelectWorldParametersWhereIdCommand : ICommand
	{
		public int Id;

		public string Command()
		{
			return $"SELECT {nameof(WorldParameters.Name)}, {nameof(WorldParameters.Seed)}, " +
				$"{nameof(WorldParameters.WorldFolderPath)} " +
				$"FROM {nameof(WorldParameters)} " +
				$"WHERE {nameof(WorldParameters.Id)} = {Id}";
		}
	}
}
