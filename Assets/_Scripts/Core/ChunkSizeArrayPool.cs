using System.Collections.Generic;
using System.Linq;

namespace ChunkCore
{
	public class ChunkSizeArrayPool<T>
	{
		public static readonly ChunkSizeArrayPool<T> Shared = new ChunkSizeArrayPool<T>();

		private readonly object _lock = new object();
		private readonly Queue<T[,,]> _freeArrays = new Queue<T[,,]>();

		public T[,,] Rent()
		{
			lock(_lock)
			{
				return _freeArrays.Any() ? _freeArrays.Dequeue() : Create();
			}
		}

		public void Return(T[,,] array, bool clearArray = false)
		{
			if(clearArray)
			{
				ClearArray(array);
			}

			lock(_lock)
			{
				_freeArrays.Enqueue(array);
			}
		}

		private void ClearArray(T[,,] array)
		{
			for(int x = 0; x < ChunkConstantData.ChunkScale.x; x++)
			{
				for(int y = 0; y < ChunkConstantData.ChunkScale.y; y++)
				{
					for(int z = 0; z < ChunkConstantData.ChunkScale.z; z++)
					{
						array[x, y, z] = default;
					}
				}
			}
		}

		private T[,,] Create()
		{
			return new T[ChunkConstantData.ChunkScale.x, ChunkConstantData.ChunkScale.y, 
				ChunkConstantData.ChunkScale.z];
		}
	}
}
