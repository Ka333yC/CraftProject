namespace Assets._Scripts.Core.BlocksCore
{
	public interface IBlockComponentContainer
	{
		public bool CanInitializeAsync { get; }

		public void InitializeBlock(Block block);
	}
}
