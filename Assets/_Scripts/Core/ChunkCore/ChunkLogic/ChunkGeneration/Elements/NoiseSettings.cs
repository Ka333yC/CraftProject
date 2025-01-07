using System;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.ChunkLogic.ChunkGeneration.Elements
{
	[Serializable]
	public class NoiseSettings
	{
		[field: SerializeField]
		public FastNoiseLite.NoiseType NoiseType { get; private set; }

		[field: SerializeField]
		public FastNoiseLite.FractalType FractalType { get; private set; }

		[field: SerializeField]
		public float Frequency { get; private set; } = 0.01f;

		[field: SerializeField]
		public int Octaves { get; private set; } = 3;

		[field: SerializeField]
		public float Lacunarity { get; private set; } = 2.0f;

		[field: SerializeField]
		public float Gain { get; private set; } = 0.5f;

		[field: SerializeField]
		public float WeightedStrength { get; private set; } = 0.0f;
	}
}
