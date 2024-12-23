using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.ObjectPhysicsCore.PositionsUpdate
{
	public class ObjectPhysicsPositionsContainer
	{
		private readonly Queue<HashSet<int>> _hashSetPool = new Queue<HashSet<int>>();
		private readonly Dictionary<Vector3Int, HashSet<int>> _objectPhysicsEntitiesByBlockPosition =
			new Dictionary<Vector3Int, HashSet<int>>();
		private readonly Dictionary<Vector3Int, HashSet<int>> _objectPhysicsEntitiesByGridPosition =
			new Dictionary<Vector3Int, HashSet<int>>();

		public void AddBlockPosition(Vector3Int blockPosition, int entity)
		{
			if(!_objectPhysicsEntitiesByBlockPosition.TryGetValue(blockPosition, out var entities))
			{
				entities = Rent();
				_objectPhysicsEntitiesByBlockPosition.Add(blockPosition, entities);
			}

			entities.Add(entity);
		}

		public void RemoveBlockPosition(Vector3Int blockPosition, int entity)
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

		public void AddGridPosition(Vector3Int gridPosition, int entity)
		{
			if(!_objectPhysicsEntitiesByGridPosition.TryGetValue(gridPosition, out var entities))
			{
				entities = Rent();
				_objectPhysicsEntitiesByGridPosition.Add(gridPosition, entities);
			}

			entities.Add(entity);
		}

		public void RemoveGridPosition(Vector3Int gridPosition, int entity)
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

		public bool TryGetEntitiesByBlockPosition(Vector3Int blockPosition, out IReadOnlyCollection<int> result)
		{
			if(_objectPhysicsEntitiesByBlockPosition.TryGetValue(blockPosition, out var hashSet))
			{
				result = hashSet;
				return true;
			}

			result = default;
			return false;
		}

		public bool TryGetEntitiesByGridPosition(Vector3Int gridPosition, out IReadOnlyCollection<int> result)
		{
			if(_objectPhysicsEntitiesByGridPosition.TryGetValue(gridPosition, out var hashSet))
			{
				result = hashSet;
				return true;
			}

			result = default;
			return false;
		}

		private HashSet<int> Rent()
		{
			return _hashSetPool.Count > 0 ? _hashSetPool.Dequeue() : new HashSet<int>();
		}

		private void Return(HashSet<int> hashSet)
		{
			_hashSetPool.Enqueue(hashSet);
		}
	}
}
