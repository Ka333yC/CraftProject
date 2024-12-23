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
using static Assets._Scripts.Core.BlocksCore.Block;

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

		public override void Initialize()
		{
			_sharedBlock = BlockPool.Shared.Rent(true);
			_sharedBlock.Container = this;
			foreach(var componentContainer in _blockComponentContainers)
			{
				componentContainer.InitializeBlock(_sharedBlock);
			}
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

		public override bool TryGetComponentContainer<T>(out T result)
		{
			foreach(var componentContainer in _blockComponentContainers)
			{
				if(componentContainer is T resultContainer)
				{
					result = resultContainer;
					return true;
				}
			}

			result = default;
			return false;
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
	}
}
