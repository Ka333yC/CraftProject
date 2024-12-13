namespace Assets._Scripts.Core.BlocksCore
{
	public interface ISerializedBlockComponent : IBlockComponent
	{
		public string Serialize();
		public void Populate(string serializedData);
	}
}
