using System;
using _Scripts.Core.BlocksCore;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.WoodLogBlock
{
	[Serializable]
	public class WoodLogBlockComponent : ISerializableBlockComponent
	{
		[HideInInspector]
		public WoodLogRotation Rotation;
		
		public void InitializeBlock(Block block)
		{
			block.AddComponent(this);
		}

		public IBlockComponent Clone()
		{
			return new WoodLogBlockComponent();
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