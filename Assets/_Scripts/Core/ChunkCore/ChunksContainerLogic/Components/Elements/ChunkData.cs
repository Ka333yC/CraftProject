using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.ChunkCore.ChunksContainerScripts
{
	public class ChunkData
	{
		private readonly List<int> _priorities = new List<int>(1);

		private int _chunkEntity = 0;

		public IReadOnlyList<int> Priorities => _priorities;
		public int UsersCount => _priorities.Count;
		// Бросит ArumentOutOfRangeException, если чанк ещё cуществует, но у него уже нету User'ов
		// (соответственно и приоритета), поэтому добавляем проверку на Count
		public int Priority => _priorities.Count > 0 ? _priorities[0] : int.MaxValue;
		public bool HasEntity => _chunkEntity != 0;
		public bool HasUsers => UsersCount > 0;

		public int ChunkEntity
		{
			get
			{
				return _chunkEntity;
			}

			set
			{
				if(_chunkEntity != 0)
				{
					throw new ArgumentException("Chunk already exist");
				}

				_chunkEntity = value;
			}
		}

		public void AddUser(int priority)
		{
			_priorities.Add(priority);
			_priorities.Sort();
		}

		public void RemoveUser(int priority)
		{
			if(!_priorities.Remove(priority))
			{
				throw new ArgumentException("Attempting to delete a priority that does not exist");
			}

			_priorities.Sort();
		}

		public void SetDefaultChunkEntity()
		{
			_chunkEntity = 0;
		}
	}
}
