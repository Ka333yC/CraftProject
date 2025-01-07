﻿namespace _Scripts.Core.BlocksCore
{
	public interface ISerializableBlockContainer : IBlockContainer
	{
		public string Serialize(Block block);
		public Block Deserialize(string serializedBlock);
	}
}
