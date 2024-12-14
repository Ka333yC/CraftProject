using Assets._Scripts.Core.BlocksCore;
using Assets.Scripts.Undone.TerrainGeneration;
using ChunkCore.LifeTimeControl;
using UnityEngine;

namespace TempScripts.TerrainGeneration
{
	public abstract class ChunkGenerator
	{
		protected readonly NoiseSettings _noiseSettings;

		public virtual int Seed { get; set; }

		public ChunkGenerator(NoiseSettings noiseSettings)
		{
			_noiseSettings = noiseSettings;
		}

		public abstract Block GetRandomBlock(ref Vector3Int position);
	}
}
