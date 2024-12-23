using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using System.Threading;

namespace Assets.Scripts.Core.ChunkCore.ChunkLogic.Loading.Saving.Components
{
	public abstract class ChunkSerializer
	{
		public abstract UniTask Save(int chunkEntity);
		/// <returns>Возвращает true, если чанк был заполнен</returns>
		public abstract UniTask<bool> Populate(int chunkEntity, CancellationToken token);
	}
}
