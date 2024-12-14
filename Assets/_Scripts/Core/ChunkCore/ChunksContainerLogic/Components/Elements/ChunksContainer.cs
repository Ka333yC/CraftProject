using Assets._Code.Core.ChunkCore.ChunksContainerLogic.Components.Elements;
using Assets.Scripts.Core.ChunkCore.ChunksContainerScripts;
using ChunkCore.LifeTimeControl;
using Leopotam.EcsLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ChunkCore.ChunksContainerScripts
{
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
			// так что ставим try-catch
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
			OnChunkDataChanged(gridPosition, chunkData.ChunkEntity);
		}

		public void RemoveChunkUser(Vector3Int gridPosition, int priority)
		{
			var chunkData = _chunkDataByGridPosition[gridPosition];
			chunkData.RemoveUser(priority);
			OnChunkDataChanged(gridPosition, chunkData.ChunkEntity);
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
			OnChunkDataChanged(gridPosition, chunkEntity);
		}

		public void RemoveChunkEntity(Vector3Int gridPosition)
		{
			var chunkEntity = _chunkDataByGridPosition[gridPosition].ChunkEntity;
			_chunkDataByGridPosition[gridPosition].SetDefaultChunkEntity();
			OnChunkDataChanged(gridPosition, chunkEntity);
		}

		public bool TryGetPositionWithoutChunkEntity(out Vector3Int result)
		{
			using var enumertor = _positionsWithoutChunkEntity.GetEnumerator();
			if(enumertor.MoveNext())
			{
				result = enumertor.Current;
				return true;
			}

			result = default;
			return false;
		}

		public bool TryGetChunkEntityWithoutChunkUsers(out int result)
		{
			using var enumertor = _chunkEntitiesWithoutChunkUsers.GetEnumerator();
			if(enumertor.MoveNext())
			{
				result = enumertor.Current;
				return true;
			}

			result = default;
			return false;
		}

		/// <returns>Вернёт entity чанка с высшим приоритетом. Если в фильтре нет сущностей, вернёт -1</returns>
		public int GetChunkWithHighlyPriority(EcsFilter chunksFilter)
		{
			var resultChunkEntity = -1;
			var highlyPriority = int.MaxValue;
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

					continue;
				}

				var chunkPriority = chunkData.Priority;
				if(chunkPriority < highlyPriority)
				{
					resultChunkEntity = chunkEntity;
					highlyPriority = chunkPriority;
				}
			}

			return resultChunkEntity;
		}

		private void OnChunkDataChanged(Vector3Int gridPosition, int previousChunkEntity)
		{
			var chunkData = _chunkDataByGridPosition[gridPosition];
			if(chunkData.HasUsers)
			{
				if(chunkData.HasEntity)
				{
					_chunkEntitiesWithoutChunkUsers.Remove(chunkData.ChunkEntity);
					_positionsWithoutChunkEntity.Remove(gridPosition);
				}
				else
				{
					_positionsWithoutChunkEntity.Add(gridPosition);
				}
			}
			else
			{
				if(chunkData.HasEntity)
				{
					_chunkEntitiesWithoutChunkUsers.Add(chunkData.ChunkEntity);
					_positionsWithoutChunkEntity.Remove(gridPosition);
				}
				else
				{
					_chunkEntitiesWithoutChunkUsers.Remove(previousChunkEntity);
					_positionsWithoutChunkEntity.Remove(gridPosition);
					_chunkDataByEntity.Remove(previousChunkEntity);
					_chunkDataByGridPosition.Remove(gridPosition);
				}
			}
		}
	}
}
