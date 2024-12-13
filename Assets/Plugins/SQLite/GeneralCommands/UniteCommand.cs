using DataBase.Commands;
using System.Collections.Generic;
using System.Text;

namespace Assets.Scripts.DataBase.GeneralCommands
{
	/// <summary>
	/// Несколько команд, которые нужно объединить: Пример Select и Limit
	/// </summary>
	public class UniteCommand : ICommand
	{
		public IEnumerable<ICommand> Commands;

		public string Command()
		{
			StringBuilder result = new StringBuilder();
			foreach(var command in Commands)
			{
				result.Append(command.Command());
				result.Append(' ');
			}

			return result.ToString();
		}
	}
}
