using Assets.Scripts.Core.ChunkCore.Saving.Components;
using Assets.Scripts.Core.DataSave;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zenject;

namespace Assets.Scripts.Core.ChunkCore.Saving.Systems
{
	public class CompressedChunkSerializerCreator : IEcsInitSystem
	{
		[Inject]
		private DiContainer _container;

		public void Init(IEcsSystems systems)
		{
			EcsWorld world = systems.GetWorld();
			var pool = world.GetPool<ChunkSerializerComponent>();
			var entity = world.NewEntity();
			ref var component = ref pool.Add(entity);
			component.ChunkSerializer = _container.Instantiate<CompressedChunksSerializer>();
		}
	}
}
