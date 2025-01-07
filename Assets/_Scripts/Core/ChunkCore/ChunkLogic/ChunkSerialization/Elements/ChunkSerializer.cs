using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Scripts.Core.ChunkCore.ChunkLogic.ChunkSerialization.Elements
{
	public abstract class ChunkSerializer
	{
		public abstract UniTask Save(int chunkEntity);
		/// <returns>Возвращает true, если чанк был заполнен</returns>
		public abstract UniTask<bool> Populate(int chunkEntity, CancellationToken token);
	}
}
