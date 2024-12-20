using DataBase.Commands;
using System;

namespace Assets.Scripts.Core.WorldsCore.WorldSettingsTable.WorldSettingsCommands
{
	public class ExistsWorldParametersWhereNameCommand : ICommand
	{
		public string Name;

		public string Command()
		{
			var subRequest = $"SELECT * " +
				$"FROM {nameof(WorldParameters)} " +
				$"WHERE {nameof(WorldParameters.Name)} = '{Name}'";
			return $"SELECT EXISTS({subRequest})";
		}
	}
}
