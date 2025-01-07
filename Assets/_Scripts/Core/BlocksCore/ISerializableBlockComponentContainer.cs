namespace _Scripts.Core.BlocksCore
{
	public interface ISerializableBlockComponentContainer : IBlockComponentContainer
	{
		public string Serialize(Block block);
		public void InitializeBlock(Block block, string serializedData);
	}
}
