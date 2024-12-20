namespace Assets._Scripts.Core.BlocksCore
{
	public interface IBlockContainerComponent
	{
		public bool CanInitializeAsync { get; }

		public void InitializeBlock(Block block);
	}
}
