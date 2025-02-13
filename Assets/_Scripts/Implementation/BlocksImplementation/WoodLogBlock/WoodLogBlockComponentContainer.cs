using System;
using _Scripts.Core.BlocksCore;

namespace _Scripts.Implementation.BlocksImplementation.WoodLogBlock
{
	public class WoodLogBlockComponentContainer : ISerializableBlockComponentContainer
	{
		public void InitializeBlock(Block block)
		{
			block.AddComponent(new WoodLogBlockComponent());
		}

		public string Serialize(Block block)
		{
			var component = block.GetComponent<WoodLogBlockComponent>();
			return ((int)component.Rotation).ToString();
		}

		public void InitializeBlock(Block block, string serializedData)
		{
			var component = new WoodLogBlockComponent();
			block.AddComponent(component);
			component.Rotation = (WoodLogRotation)Convert.ToInt32(serializedData);
		}
	}
}