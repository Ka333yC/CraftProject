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
		private readonly BlocksArchetypes _blocksArchetypes;

		[JsonProperty]
		private List<int> _blocksIdWithCount = new List<int>();
		[JsonProperty]
		private int _lastBlockId = -1;
		[JsonProperty]
		private int _lastBlockIdIndex = -2;
		[JsonProperty]
		private Stack<string> _serializedBlocksData = new Stack<string>();

		public CompressedBlocks(BlocksArchetypes blocksArchetypes)
		{
			_blocksArchetypes = blocksArchetypes;
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

			if(block.Archetype is ISerializableBlockArchetype serializableContainer)
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
			var blockArchetype = _blocksArchetypes[_lastBlockId];
			if(blockArchetype is ISerializableBlockArchetype serializableArchetype)
			{
				var serializedData = _serializedBlocksData.Pop();
				return serializableArchetype.CreateBlock(serializedData);
			}
			
			return blockArchetype.CreateBlock();
		}
	}
}
