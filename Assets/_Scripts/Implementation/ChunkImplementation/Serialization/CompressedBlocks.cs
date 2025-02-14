using System.Collections.Generic;
using _Scripts.Core.BlocksCore;
using Newtonsoft.Json;

namespace _Scripts.Implementation.ChunkImplementation.Serialization
{
	/// <summary>
	/// Работает по прицнипу LIFO(стека)
	/// </summary>
	public class CompressedBlocks
	{
		private readonly BlocksContainers _blocksContainers;

		[JsonProperty]
		private List<int> _blocksIdWithCount = new List<int>();
		[JsonProperty]
		private int _lastBlockId = -1;
		[JsonProperty]
		private int _lastBlockIdIndex = -2;
		[JsonProperty]
		private Stack<string> _serializedBlocksData = new Stack<string>();

		public CompressedBlocks(BlocksContainers blocksContainers)
		{
			_blocksContainers = blocksContainers;
		}

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

			if(block.Container is ISerializableBlockContainer serializableContainer)
			{
				var serializedData = serializableContainer.Serialize(block);
				_serializedBlocksData.Push(serializedData);
			}
		}

		public Block PopLast() 
		{
			if(_blocksIdWithCount[_lastBlockIdIndex + 1] == 0)
			{
				_lastBlockIdIndex -= 2;
				_lastBlockId = _blocksIdWithCount[_lastBlockIdIndex];
			}

			_blocksIdWithCount[_lastBlockIdIndex + 1]--;
			var blockContainer = _blocksContainers[_lastBlockId];
			if(blockContainer is ISerializableBlockContainer serializableContainer)
			{
				var serializedData = _serializedBlocksData.Pop();
				return serializableContainer.CreateBlock(serializedData);
			}
			
			return blockContainer.CreateBlock();
		}
	}
}
