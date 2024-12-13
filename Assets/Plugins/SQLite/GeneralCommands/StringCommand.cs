using DataBase.Commands;

namespace DataBase.DataBase.Commands
{
	public class StringCommand : ICommand
	{
		public string CommandText;

		public string Command()
		{
			return CommandText;
		}
	}
}
