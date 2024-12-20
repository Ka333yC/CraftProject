namespace Assets._Scripts.Core.BlocksCore
{
	public interface ISerializableBlockComponent : IBlockComponent
	{
		public string Serialize();
		public void Populate(string serializedData);
	}
}
