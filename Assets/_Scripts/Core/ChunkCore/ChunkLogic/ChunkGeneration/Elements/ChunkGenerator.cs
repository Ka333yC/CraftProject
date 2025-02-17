using _Scripts.Core.BlocksCore;
using Cysharp.Threading.Tasks;

namespace _Scripts.Core.ChunkCore.ChunkLogic.ChunkGeneration.Elements
{
	public abstract class ChunkGenerator
	{
		protected readonly int _seed;
		protected readonly NoiseSettings _noiseSettings;
		protected readonly BlocksArchetypes _blocksArchetypes;

		public ChunkGenerator(int seed, NoiseSettings noiseSettings, BlocksArchetypes blocksArchetypes)
		{
			_seed = seed;
			_noiseSettings = noiseSettings;
			_blocksArchetypes = blocksArchetypes;
		}

		public abstract UniTask GenerateBlocks(int chunkEntity);
	}
}
