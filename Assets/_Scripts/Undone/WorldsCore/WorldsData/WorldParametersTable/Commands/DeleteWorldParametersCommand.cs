using DataBase.Commands;
using System;

namespace Assets.Scripts.Core.WorldsCore.WorldSettingsTable.WorldSettingsCommands
{
	public class DeleteWorldParametersCommand : ICommand
	{
		public int Id;

		public string Command()
		{
			return $"DELETE FROM {nameof(WorldParameters)} " +
				$"WHERE {nameof(WorldParameters.Id)} = {Id}";
		}
	}
}
