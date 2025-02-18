using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace _Scripts.Core.ChunkCore.ChunksContainerLogic.Components.Elements
{
	// Где-то есть баг, из-за которого удаляется ещё не созданный чанк, в идеале переделать весь ChunksContainer
	// т.к. он сам по себе непонятно и неоптимально работает
	public class ChunksContainer
	{
		private readonly Dictionary<Vector3Int, ChunkData> _chunkDataByGridPosition =
			new Dictionary<Vector3Int, ChunkData>();
		private readonly Dictionary<int, ChunkData> _chunkDataByEntity = new Dictionary<int, ChunkData>();
		private readonly HashSet<Vector3Int> _positionsWithoutChunkEntity = new HashSet<Vector3Int>();
		private readonly HashSet<int> _chunkEntitiesWithoutChunkUsers = new HashSet<int>();

		public bool TryGetChunk(Vector3Int gridPosition, out int chunkEntity)
		{
			// В редких случаях может выбрасывать IndexOutOfRangeException, если доступ из потоков,
			// так что ставим try-catch. Впоследствии добавить lock(){}
			try
			{
				if(_chunkDataByGridPosition.TryGetValue(gridPosition, out var chunkData) &&
					chunkData.HasEntity)
				{
					chunkEntity = chunkData.ChunkEntity;
					return true;
				}
			}
			catch(IndexOutOfRangeException)
			{
			}

			chunkEntity = default;
			return false;
		}

		public void AddChunkUser(Vector3Int gridPosition, int priority)
		{
			if(!_chunkDataByGridPosition.TryGetValue(gridPosition, out var chunkData))
			{
				chunkData = new ChunkData();
				_chunkDataByGridPosition.Add(gridPosition, chunkData);
			}

			chunkData.AddUser(priority);
			if(chunkData.HasEntity)
			{
				_chunkEntitiesWithoutChunkUsers.Remove(chunkData.ChunkEntity);
			}
			else
			{
				_positionsWithoutChunkEntity.Add(gridPosition);
			}
		}

		public void RemoveChunkUser(Vector3Int gridPosition, int priority)
		{
			var chunkData = _chunkDataByGridPosition[gridPosition];
			chunkData.RemoveUser(priority);
			if(chunkData.HasUsers)
			{
				return;
			}

			if(chunkData.HasEntity)
			{
				_chunkEntitiesWithoutChunkUsers.Add(chunkData.ChunkEntity);
			}
			else
			{
				_chunkDataByGridPosition.Remove(gridPosition);
				_positionsWithoutChunkEntity.Remove(gridPosition);
			}
		}

		public void SetChunkEntity(Vector3Int gridPosition, int chunkEntity)
		{
			if(!_chunkDataByGridPosition.TryGetValue(gridPosition, out var chunkData))
			{
				chunkData = new ChunkData();
				_chunkDataByGridPosition.Add(gridPosition, chunkData);
			}

			chunkData.ChunkEntity = chunkEntity;
			_chunkDataByEntity.Add(chunkEntity, chunkData);
			_positionsWithoutChunkEntity.Remove(gridPosition);
			if(!chunkData.HasUsers)
			{
				_chunkEntitiesWithoutChunkUsers.Add(chunkData.ChunkEntity);
			}
		}

		public void RemoveChunkEntity(Vector3Int gridPosition)
		{
			var chunkData = _chunkDataByGridPosition[gridPosition];
			_chunkDataByEntity.Remove(chunkData.ChunkEntity);
			_chunkEntitiesWithoutChunkUsers.Remove(chunkData.ChunkEntity);
			if(chunkData.HasUsers)
			{
				_chunkDataByGridPosition[gridPosition].ResetChunkEntity();
				_positionsWithoutChunkEntity.Add(gridPosition);
			}
			else
			{
				_chunkDataByGridPosition.Remove(gridPosition);
			}
		}

		public bool TryGetPositionWithoutChunkEntity(out Vector3Int result)
		{
			var hasResult = false;
			var lowerPriority = int.MaxValue;
			result = default;
			// В редких случаях может выбрасывать IndexOutOfRangeException, т.к. доступ из потоков,
			// так что ставим try-catch
			try
			{
				foreach(var position in _positionsWithoutChunkEntity)
				{
					var priority = _chunkDataByGridPosition[position].Priority;
					if(priority < lowerPriority)
					{
						hasResult = true;
						lowerPriority = priority;
						result = position;
					}
				}
			}
			catch(IndexOutOfRangeException)
			{
			}

			return hasResult;
		}

		public bool TryGetChunkEntityWithoutChunkUsers(out int result)
		{
			using var chunkEntitiesWithoutChunkEnumertor = _chunkEntitiesWithoutChunkUsers.GetEnumerator();
			if(chunkEntitiesWithoutChunkEnumertor.MoveNext())
			{
				result = chunkEntitiesWithoutChunkEnumertor.Current;
				return true;
			}

			result = default;
			return false;
		}

		public int GetChunkWithLowestPriority(EcsFilter chunksFilter)
		{
			var resultChunkEntity = -1;
			var lowerPriority = int.MaxValue;
			foreach(var chunkEntity in chunksFilter)
			{
				var chunkData = _chunkDataByEntity[chunkEntity];
				// Проверка на количество пользователей, т.к. чанк уже может попасть в очередь
				// на удаление и у него не будет приоритета, но он ещё будет существовать
				if(chunkData.UsersCount == 0)
				{
					// Даже если чанк в очереди на удаление, необходимо инициализировать resultChunkEntity, 
					// чтобы результат не был -1
					if(resultChunkEntity == -1)
					{
						resultChunkEntity = chunkEntity;
					}
				}
				else if(chunkData.Priority < lowerPriority)
				{
					resultChunkEntity = chunkEntity;
					lowerPriority = chunkData.Priority;
				}
			}

			return resultChunkEntity;
		}
	}
}
