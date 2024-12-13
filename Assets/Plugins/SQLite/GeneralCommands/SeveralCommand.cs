using DataBase.Commands;
using System.Collections.Generic;
using System.Text;

namespace DataBase.DataBase.Commands
{
	/// <summary>
	/// Несколько разных команд, которые нужно выполнить последовательно
	/// </summary>
	public class SeveralCommand : ICommand
	{
		public IEnumerable<ICommand> Commands;

		public SeveralCommand(params ICommand[] commands)
		{
			Commands = commands;
		}

		public string Command()
		{
			StringBuilder result = new StringBuilder();
			foreach(var command in Commands)
			{
				result.Append(command.Command());
				result.Append(';');
			}

			return result.ToString();
		}
	}
}
