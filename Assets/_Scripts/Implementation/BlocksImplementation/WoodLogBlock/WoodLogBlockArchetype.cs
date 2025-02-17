using System;
using _Scripts.Core.BlocksCore;
using _Scripts.Implementation.BlocksImplementation.StandardBlock;
using _Scripts.Implementation.BlocksImplementation.StandardBlock.Physics;
using _Scripts.Implementation.BlocksImplementation.WoodLogBlock.Graphics;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation.WoodLogBlock
{
	[CreateAssetMenu(fileName = "BlockArchetype", menuName = "Blocks/Archetypes/Wood log block archetype")]
	public class WoodLogBlockArchetype : UniqueBlockArchetype
	{
		private void OnValidate()
		{
			ValidateBlockComponents();
			ValidateBlockPlaceableCheckers();
		}

		public Block CreateBlock(WoodLogRotation rotation)
		{
			var block = CreateBlock();
			var woodLogComponent = block.GetComponent<WoodLogBlockComponent>();
			woodLogComponent.Rotation = rotation;
			return block;
		}

		private void ValidateBlockComponents()
		{
			if(_blockComponents.Length != 3)
			{
				Array.Resize(ref _blockComponents, 3);
			}

			if(_blockComponents[0] is not WoodLogBlockComponent)
			{
				_blockComponents[0] = new WoodLogBlockComponent();
			}

			if(_blockComponents[1] is not WoodLogBlockGraphicsComponent)
			{
				_blockComponents[1] = new WoodLogBlockGraphicsComponent();
			}

			if(_blockComponents[2] is not StandardBlockPhysicsComponent)
			{
				_blockComponents[2] = new StandardBlockPhysicsComponent();
			}
		}

		private void ValidateBlockPlaceableCheckers()
		{
			if(_blockPlaceableCheckers.Length != 1)
			{
				Array.Resize(ref _blockPlaceableCheckers, 1);
			}

			if(_blockPlaceableCheckers[0] is not PhysicsBlockPlaceableChecker)
			{
				_blockPlaceableCheckers[0] = new PhysicsBlockPlaceableChecker();
			}
		}
	}
}