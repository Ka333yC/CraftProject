using Assets._Scripts.Core.BlocksCore;
using Assets._Scripts.Implementation.BlocksImplementation;
using Assets.Scripts.Realization.Blocks.BlockDataPresentation;
using ChunkCore.LifeTimeControl;
using GraphicsCore.ChunkGraphicsCore.BlockGraphics;
using Leopotam.EcsLite;
using System.ComponentModel;
using System.Linq;
using TempScripts;
using UnityEngine;
using Zenject;

namespace ChunkCore.BlockData
{
	[CreateAssetMenu(fileName = "SharedBlockContainer", menuName = "Blocks/Shared block container")]
	public class SharedBlockContainer : BlockContainer
	{
		[SerializeField]
		private BlockComponentContainer[] _blockComponentContainers;
		[SerializeField]
		private BlockPlaceableChecker[] _blockPlaceableCheckers;

		private Block _sharedBlock;

		public override int Id { get; set; }
		public override bool CanCreateBlockAsync => true;
		public override IBlockComponentContainer[] BlockComponentContainers
		{
			get
			{
				return _blockComponentContainers;
			}
		}

		public override void Initialize()
		{
			_sharedBlock = CreateSharedBlock();
		}

		public override Block CreateBlock()
		{
			return _sharedBlock;
		}

		public override bool IsPlaceable(Vector3Int worldPosition)
		{
			foreach(var placeableChecker in _blockPlaceableCheckers)
			{
				if(!placeableChecker.IsPlaceable(worldPosition))
				{
					return false;
				}
			}

			return true;
		}

		[Inject]
		private void InjectDataContainers(DiContainer container)
		{
			foreach(var componentContainer in _blockComponentContainers)
			{
				container.Inject(componentContainer);
			}

			foreach(var placeableChecker in _blockPlaceableCheckers)
			{
				container.Inject(placeableChecker);
			}
		}

		private Block CreateSharedBlock()
		{
			var result = new Block(true);
			result.Initialize(this);
			return result;
		}
	}
}
