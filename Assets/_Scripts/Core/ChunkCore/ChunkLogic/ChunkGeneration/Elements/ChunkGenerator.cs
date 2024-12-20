using Assets._Scripts.Core.BlocksCore;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Components.Elements;
using Assets.Scripts.Undone.TerrainGeneration;
using ChunkCore;
using ChunkCore.LifeTimeControl;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace TempScripts.TerrainGeneration
{
	public abstract class ChunkGenerator
	{
		protected readonly NoiseSettings _noiseSettings;
		protected readonly int _seed;
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
