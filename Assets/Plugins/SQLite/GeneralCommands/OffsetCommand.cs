using DataBase.Commands;

namespace Assets.Scripts.DataBase.GeneralCommands
{
	public class OffsetCommand : ICommand
	{
		public int Offset;

		public string Command()
		{
			return $"OFFSET {Offset}";
		}
	}
}
