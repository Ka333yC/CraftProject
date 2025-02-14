using System;
using _Scripts.Core;
using _Scripts.Core.BlocksCore;
using _Scripts.Core.ChunkCore.ChunkLogic.ChunkGeneration.Elements;
using _Scripts.Core.ChunkCore.ChunkLogic.Components;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Implementation.ChunkImplementation.Generation
{
	public class OverworldChunkGenerator : ChunkGenerator
	{
		private readonly EcsPool<ChunkComponent> _chunksPool;

		private readonly FastNoiseLite _noise = new FastNoiseLite();
		private readonly int _baseHeight = 90;
		private readonly int _maxDeviation = 4;

		public OverworldChunkGenerator(EcsWorld world, int seed, NoiseSettings noiseSettings, 
			BlocksArchetype blocksContainers) : base(seed, noiseSettings, blocksContainers)
		{
			_chunksPool = world.GetPool<ChunkComponent>();

			_noise.SetNoiseType(noiseSettings.NoiseType);
			_noise.SetFractalType(noiseSettings.FractalType);
			_noise.SetFrequency(noiseSettings.Frequency);
			_noise.SetFractalOctaves(noiseSettings.Octaves);
			_noise.SetFractalLacunarity(noiseSettings.Lacunarity);
			_noise.SetFractalGain(noiseSettings.Gain);
			_noise.SetFractalWeightedStrength(noiseSettings.WeightedStrength);
		}

		public override async UniTask GenerateBlocks(int chunkEntity)
		{
			var chunk = _chunksPool.Get(chunkEntity);
			await UniTask.RunOnThreadPool(() => GenerateBlocks(chunk));
		}

		private void GenerateBlocks(ChunkComponent chunk) 
		{
			Vector3Int worldChunksPosition = ChunkConstantData.GridToWorldPosition(chunk.GridPosition);
			var blocks = chunk.Blocks;
			for(int y = 0; y < ChunkConstantData.ChunkScale.y; y++)
			{
				for(int x = 0; x < ChunkConstantData.ChunkScale.x; x++)
				{
					for(int z = 0; z < ChunkConstantData.ChunkScale.z; z++)
					{
						Vector3Int blockPosition = new Vector3Int(x, y, z);
						Vector3Int worldBlockPosition = blockPosition + worldChunksPosition;
						Block newBlock = GetRandomBlock(ref worldBlockPosition);
						blocks[x, y, z] = newBlock;
					}
				}
			}
		}

		private Block GetRandomBlock(ref Vector3Int position)
		{
			int y = position.y;
			if(y > ChunkConstantData.ChunkScale.y || y < 0)
			{
				throw new ArgumentException("Over world bounds");
			}

			if(y == 0)
			{
				return _blocksContainers[2].CreateBlock();
			}

			var noiseValue = _noise.GetNoise(position.x, position.z);
			int maxHeightByY = _baseHeight + Mathf.FloorToInt(_maxDeviation * noiseValue);
			if(y > maxHeightByY)
			{
				return _blocksContainers.Air;
			}
			else if(y == maxHeightByY)
			{
				return _blocksContainers[4].CreateBlock();
			}
			else if(y < maxHeightByY)
			{
				return _blocksContainers[1].CreateBlock();
			}

			throw new ArgumentException("Unknown state");
		}
	}
}
