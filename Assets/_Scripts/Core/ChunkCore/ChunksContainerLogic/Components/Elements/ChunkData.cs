using System;
using System.Collections.Generic;

namespace Assets.Scripts.Core.ChunkCore.ChunksContainerScripts
{
	public class ChunkData
	{
		private const int _defaultChunkEntityValue = -1;

		private readonly List<int> _priorities = new List<int>(1);

		private int _chunkEntity = _defaultChunkEntityValue;

		public IReadOnlyList<int> Priorities => _priorities;
		public int Priority => _priorities[0];
		public bool HasUsers => _priorities.Count > 0;
		public int UsersCount => _priorities.Count;
		public bool HasEntity => _chunkEntity != _defaultChunkEntityValue;
		public int ChunkEntity
		{
			get
			{
				return _chunkEntity;
			}

			set
			{
				if(HasEntity)
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

		public void ResetChunkEntity()
		{
			_chunkEntity = _defaultChunkEntityValue;
		}
	}
}
