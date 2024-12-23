using System;
using System.Buffers;
using Assets.Scripts.Apart.Extensions;
using Assets.Scripts.PhysicsCore;
using Assets.Scripts.Realization.Blocks.BlockDataPresentation;
using TempScripts;
using UnityEngine;

namespace Assets.Scripts.Realization.Blocks.CubePhysics
{
	[CreateAssetMenu(fileName = "PhysicsPlaceableChecker", menuName = "Blocks/Physics/Physics placeable checker")]
	public class PhysicsBlockPlaceableChecker : BlockPlaceableChecker
	{
		[SerializeField] 
		private Vector3 _boxSizeToCheck;

		public override bool IsPlaceable(Vector3Int worldPosition)
		{
			var boxSizeToCheckHalfSize = _boxSizeToCheck / 2;
			Vector3 center = new Vector3(worldPosition.x + boxSizeToCheckHalfSize.x,
				worldPosition.y, worldPosition.z + boxSizeToCheckHalfSize.z);
			Vector3 halfExtents = new Vector3(boxSizeToCheckHalfSize.x - Physics.defaultContactOffset, 0, 
				boxSizeToCheckHalfSize.z - Physics.defaultContactOffset);
			var raycastHits = ArrayPool<RaycastHit>.Shared.Rent(16);
			var count = Physics.BoxCastNonAlloc(center, halfExtents, Vector3.up, raycastHits, 
				Quaternion.identity, _boxSizeToCheck.y - Physics.defaultContactOffset * 2);
			bool isPlaceable = true;
			for(int i = 0; i < count; i++)
			{
				var layer = raycastHits[i].collider.gameObject.layer;
				if(!Singleton.Instance.PhysicsSettings.GroundLayer.Has(layer) &&
					!Singleton.Instance.PhysicsSettings.IgnoreAllLayer.Has(layer))
				{
					isPlaceable = false;
					break;
				}
			}

			ArrayPool<RaycastHit>.Shared.Return(raycastHits);
			return isPlaceable;
		}
	}
}
