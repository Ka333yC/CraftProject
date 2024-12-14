using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using System.Threading;

namespace Assets.Scripts.Core.ChunkCore.ChunkLogic.Loading.Saving.Components
{
	public abstract class ChunkSerializer
	{
		protected readonly EcsWorld _world;

		public ChunkSerializer(EcsWorld world)
		{
			_world = world;
		}

		public abstract UniTask Save(int chunkEntity, CancellationToken token);
		/// <returns>Возвращает true, если чанк был заполнен</returns>
		public abstract UniTask<bool> Populate(int chunkEntity, CancellationToken token);
	}
}
