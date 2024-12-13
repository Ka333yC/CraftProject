using DataBase.Commands;

namespace Assets.Scripts.DataBase.GeneralCommands
{
	public class LimitCommand : ICommand
	{
		public int Count;

		public string Command()
		{
			return $"LIMIT {Count}";
		}
	}
}
