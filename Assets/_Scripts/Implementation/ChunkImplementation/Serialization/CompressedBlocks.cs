
using Assets._Scripts.Core.BlocksCore;
using ChunkCore.LifeTimeControl;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using TempScripts;

namespace Assets.Scripts.Core.ChunkCore.Saving
{
	public class CompressedBlocks
	{
		private readonly BlocksContainers _blocksContainers;

		[JsonProperty]
		private List<int> _blocksIdWithCount = new List<int>();
		[JsonProperty]
		private int _lastBlockId = -1;
		[JsonProperty]
		private int _lastBlockIdIndex = -2;

		public CompressedBlocks(BlocksContainers blocksContainers)
		{
			_blocksContainers = blocksContainers;
		}

		/// <summary>
		///  ƒобавление начинаетс€ с индекса 0, 0, 0
		/// </summary>
		public void AddLast(Block block) 
		{
			var blockId = block.Id;
			if(_lastBlockId == blockId)
			{
				_blocksIdWithCount[_lastBlockIdIndex + 1]++;
			}
			else
			{
				_blocksIdWithCount.Add(blockId);
				_blocksIdWithCount.Add(1);
				_lastBlockId = blockId;
				_lastBlockIdIndex += 2;
			}
		}

		/// <summary>
		///  ”даление начинаетс€ с последнего добавленного индекса, то есть с 15, 15, 15
		/// </summary>
		public Block PopLast() 
		{
			if(_blocksIdWithCount[_lastBlockIdIndex + 1] == 0)
			{
				_lastBlockIdIndex -= 2;
				_lastBlockId = _blocksIdWithCount[_lastBlockIdIndex];
			}

			_blocksIdWithCount[_lastBlockIdIndex + 1]--;
			return _blocksContainers[_lastBlockId].CreateBlock();
		}
	}
}
