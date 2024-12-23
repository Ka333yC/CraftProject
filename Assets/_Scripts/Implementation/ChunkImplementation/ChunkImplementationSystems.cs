using Assets.Scripts.Core.ChunkCore.Saving.Systems;
using Assets.Scripts.Implementation.Chunk.Loading.TerrainGeneration.Systems;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Implementation.Chunk
{
	public static class ChunkImplementationSystems
	{
		public static IEnumerable<IEcsSystem> GetFixedInitCreatorSystems()
		{
			return new List<IEcsSystem>()
			{
				new CompressedChunkSerializerCreator(),
				new ChunkGeneratorCreator(),
			};
		}
	}
}
