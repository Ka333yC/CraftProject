using _Scripts.Apart.Extensions;
using _Scripts.Core.PhysicsCore.Presets;
using _Scripts.Implementation.BlocksImplementation;
using UnityEngine;
using Zenject;

namespace _Scripts.Implementation.Blocks.StandardBlock.Physics
{
	[CreateAssetMenu(fileName = "PhysicsPlaceableChecker", menuName = "Blocks/Physics/Physics placeable checker")]
	public class PhysicsBlockPlaceableChecker : BlockPlaceableChecker
	{
		[SerializeField] 
		private Vector3 _boxSizeToCheck;

		[Inject]
		private PhysicsPresets _physicsPresets;

		public override bool IsPlaceable(Vector3Int worldPosition)
		{
			var boxSizeToCheckHalfSize = _boxSizeToCheck / 2;
			Vector3 center = new Vector3(worldPosition.x + boxSizeToCheckHalfSize.x,
				worldPosition.y, worldPosition.z + boxSizeToCheckHalfSize.z);
			Vector3 halfExtents = new Vector3(boxSizeToCheckHalfSize.x - UnityEngine.Physics.defaultContactOffset, 0, 
				boxSizeToCheckHalfSize.z - UnityEngine.Physics.defaultContactOffset);
			var raycastHits = System.Buffers.ArrayPool<RaycastHit>.Shared.Rent(16);
			var count = UnityEngine.Physics.BoxCastNonAlloc(center, halfExtents, Vector3.up, raycastHits, 
				Quaternion.identity, _boxSizeToCheck.y - UnityEngine.Physics.defaultContactOffset * 2);
			bool isPlaceable = true;
			for(int i = 0; i < count; i++)
			{
				var layer = raycastHits[i].collider.gameObject.layer;
				if(!_physicsPresets.GroundLayer.Has(layer) &&
					!_physicsPresets.IgnoreAllLayer.Has(layer))
				{
					isPlaceable = false;
					break;
				}
			}

			System.Buffers.ArrayPool<RaycastHit>.Shared.Return(raycastHits);
			return isPlaceable;
		}
	}
}
