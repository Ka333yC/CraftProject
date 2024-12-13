using DataBase.Commands;

namespace DataBase.DataBase.Commands.DataBaseCommands
{
	// Использовать через SeveralCommand
	public class LastInsertRowIdCommand : ICommand
	{
		public string Command()
		{
			return "SELECT last_insert_rowid()";
		}
	}
}
