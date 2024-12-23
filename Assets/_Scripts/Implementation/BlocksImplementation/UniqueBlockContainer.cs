using Assets._Scripts.Core.BlocksCore;
using Assets.Scripts.Realization.Blocks.BlockDataPresentation;
using ChunkCore.BlockData;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;
using static Assets._Scripts.Core.BlocksCore.Block;

namespace Assets._Scripts.Implementation.BlocksImplementation
{
	[CreateAssetMenu(fileName = "UniqueBlockContainer", menuName = "Blocks/Unique block container")]
	public class UniqueBlockContainer : BlockContainer
	{
		[SerializeField]
		private BlockComponentContainer[] _blockComponentContainers;
		[SerializeField]
		private BlockPlaceableChecker[] _blockPlaceableCheckers;

		private bool _canCreateBlockAsync;

		public override int Id { get; set; }
		public override bool CanCreateBlockAsync => _canCreateBlockAsync;
		public override IBlockComponentContainer[] BlockComponentContainers
		{
			get
			{
				return _blockComponentContainers;
			}
		}

		public override void Initialize()
		{
			_canCreateBlockAsync = CanInitializeComponentContainersAsync();
		}

		public override Block CreateBlock()
		{
			var result = BlockPool.Shared.Rent(false);
			result.Initialize(this);
			foreach(var blockComponentContainer in _blockComponentContainers)
			{
				blockComponentContainer.InitializeBlock(result);
			}

			return result;
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
		private void Inject(DiContainer container)
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

		private bool CanInitializeComponentContainersAsync()
		{
			foreach(var blockComponentContainer in _blockComponentContainers)
			{
				if(!blockComponentContainer.CanInitializeAsync)
				{
					return false;
				}
			}

			return true;
		}
	}
}
