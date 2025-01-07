using System.Collections.Generic;
using _Scripts.Implementation.ChunkImplementation.Generation.Systems;
using _Scripts.Implementation.ChunkImplementation.Serialization.Systems;
using Leopotam.EcsLite;

namespace _Scripts.Implementation.ChunkImplementation
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
