using Assets.Scripts.Core.ChunkCore.Saving;
using DataBase.Commands;
using System;

namespace Assets.Scripts.Core.WorldsCore.WorldSettingsCommands
{
	public class SelectWorldParametersCommand : ICommand
	{
		public string Command()
		{
			return $"SELECT {nameof(WorldParameters.Id)}, {nameof(WorldParameters.Name)}, " +
				$"{nameof(WorldParameters.Seed)}, {nameof(WorldParameters.WorldFolderPath)} " +
				$"FROM {nameof(WorldParameters)}";
		}
	}
}
