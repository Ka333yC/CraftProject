namespace _Scripts.Core.BlocksCore
{
	public interface ISerializableBlockComponent : IBlockComponent
	{
		public string Serialize(Block block);
		public void InitializeBlock(Block block, string serializedData);
	}
}