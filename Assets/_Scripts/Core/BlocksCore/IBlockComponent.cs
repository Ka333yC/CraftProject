namespace _Scripts.Core.BlocksCore
{
	public interface IBlockComponent
	{
		public void InitializeBlock(Block block);
		public IBlockComponent Clone();
	}
}
