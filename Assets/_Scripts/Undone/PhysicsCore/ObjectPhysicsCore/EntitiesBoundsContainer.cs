using System;
using System.Collections.Generic;
using _Scripts.Core.Extensions;
using UnityEngine;

namespace _Scripts.Undone.PhysicsCore.ObjectPhysicsCore
{
	public class EntitiesBoundsContainer
	{
		private readonly Queue<List<int>> _listPool = new Queue<List<int>>();
		private readonly Dictionary<Vector3Int, List<int>> _objectPhysicsEntitiesByBlockPosition =
			new Dictionary<Vector3Int, List<int>>();
		private readonly Dictionary<Vector3Int, List<int>> _objectPhysicsEntitiesByGridPosition =
			new Dictionary<Vector3Int, List<int>>();

		public void Add(BoundsInt bounds, int entity)
		{
			foreach(var blockPosition in bounds.GetPositions())
			{
				AddEntityByBlockPosition(blockPosition, entity);
			}

			foreach(var gridPosition in bounds.GetGridPositions())
			{
				AddEntityByGridPosition(gridPosition, entity);
			}
		}

		public void Remove(BoundsInt bounds, int entity)
		{
			foreach(var blockPosition in bounds.GetPositions())
			{
				RemoveEntityByBlockPosition(blockPosition, entity);
			}

			foreach(var gridPosition in bounds.GetGridPositions())
			{
				RemoveEntityByGridPosition(gridPosition, entity);
			}
		}

		public bool TryGetEntitiesByBlockPosition(Vector3Int blockPosition, out IReadOnlyList<int> result)
		{
			if(_objectPhysicsEntitiesByBlockPosition.TryGetValue(blockPosition, out var list))
			{
				result = list;
				return true;
			}

			result = default;
			return false;
		}

		public bool TryGetEntitiesByGridPosition(Vector3Int gridPosition, out IReadOnlyList<int> result)
		{
			if(_objectPhysicsEntitiesByGridPosition.TryGetValue(gridPosition, out var list))
			{
				result = list;
				return true;
			}

			result = default;
			return false;
		}

		private void AddEntityByBlockPosition(Vector3Int blockPosition, int entity)
		{
			if(!_objectPhysicsEntitiesByBlockPosition.TryGetValue(blockPosition, out var entities))
			{
				entities = Rent();
				_objectPhysicsEntitiesByBlockPosition.Add(blockPosition, entities);
			}

			entities.Add(entity);
		}

		private void RemoveEntityByBlockPosition(Vector3Int blockPosition, int entity)
		{
			var entities = _objectPhysicsEntitiesByBlockPosition[blockPosition];
			if(!entities.Remove(entity))
			{
				throw new ArgumentException("Collection doesn't contains this entity by position");
			}

			if(entities.Count > 0)
			{
				return;
			}

			_objectPhysicsEntitiesByBlockPosition.Remove(blockPosition);
			Return(entities);
		}

		private void AddEntityByGridPosition(Vector3Int gridPosition, int entity)
		{
			if(!_objectPhysicsEntitiesByGridPosition.TryGetValue(gridPosition, out var entities))
			{
				entities = Rent();
				_objectPhysicsEntitiesByGridPosition.Add(gridPosition, entities);
			}

			entities.Add(entity);
		}

		private void RemoveEntityByGridPosition(Vector3Int gridPosition, int entity)
		{
			var entities = _objectPhysicsEntitiesByGridPosition[gridPosition];
			if(!entities.Remove(entity))
			{
				throw new ArgumentException("Collection doesn't contains this entity by position");
			}

			if(entities.Count > 0)
			{
				return;
			}

			_objectPhysicsEntitiesByGridPosition.Remove(gridPosition);
			Return(entities);
		}

		private List<int> Rent()
		{
			return _listPool.Count > 0 ? _listPool.Dequeue() : new List<int>();
		}

		private void Return(List<int> list)
		{
			_listPool.Enqueue(list);
		}
	}
}
