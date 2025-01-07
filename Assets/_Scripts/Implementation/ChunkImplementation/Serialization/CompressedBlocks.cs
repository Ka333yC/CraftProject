using System.Collections.Generic;
using _Scripts.Core.BlocksCore;
using Newtonsoft.Json;

namespace _Scripts.Implementation.ChunkImplementation.Serialization
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
		///  ���������� ���������� � ������� 0, 0, 0
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
		///  �������� ���������� � ���������� ������������ �������, �� ���� � 15, 15, 15
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
