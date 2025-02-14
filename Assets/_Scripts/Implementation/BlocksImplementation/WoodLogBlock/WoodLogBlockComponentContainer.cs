using System;
using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.WoodLogBlock
{
	[Serializable]
	public class WoodLogBlockComponentContainer : ISerializableBlockComponentContainer
	{
		public void InitializeBlock(Block block)
		{
			var component = new WoodLogBlockComponent();
			block.AddComponent(component);
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