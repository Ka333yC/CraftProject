using Assets._Scripts.Core.BlocksCore;
using Assets.Scripts.Core.ChunkCore.ChunkLogic.Loading.TerrainGeneration.Components;
using Assets.Scripts.Core.ChunkCore.Saving.Components;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempScripts;
using TempScripts.TerrainGeneration;
using UnityEngine.Rendering.VirtualTexturing;
using Zenject;

namespace Assets.Scripts.Implementation.Chunk.Loading.TerrainGeneration.Systems
{
	public class ChunkGeneratorCreator : IEcsInitSystem
	{
		[Inject]
		private BlocksContainers _blocksContainers;

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			var pool = world.GetPool<ChunkGeneratorComponent>();
			var entity = world.NewEntity();
			ref var component = ref pool.Add(entity);
			component.ChunkGenerator = new OverworldChunkGenerator(systems.GetWorld(),
				0, Singleton.Instance.NoiseSettings, _blocksContainers);
			//component.ChunkGenerator = new ClassicFlatChunkGenerator(Singleton.Instance.NoiseSettings);
		}
	}
}
