namespace _Scripts.Core.BlocksCore
{
	public interface ISerializableBlockArchetype : IBlockArchetype
	{
		public string Serialize(Block block);
		public Block CreateBlock(string serializedBlock);
	}
}
