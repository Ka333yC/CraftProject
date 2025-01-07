using _Scripts.Core.BlocksCore;
using Cysharp.Threading.Tasks;

namespace _Scripts.Core.ChunkCore.ChunkLogic.ChunkGeneration.Elements
{
	public abstract class ChunkGenerator
	{
		protected readonly int _seed;
		protected readonly NoiseSettings _noiseSettings;
		protected readonly BlocksContainers _blocksContainers;

		public ChunkGenerator(int seed, NoiseSettings noiseSettings, BlocksContainers blocksContainers)
		{
			_seed = seed;
			_noiseSettings = noiseSettings;
			_blocksContainers = blocksContainers;
		}

		public abstract UniTask GenerateBlocks(int chunkEntity);
	}
}
