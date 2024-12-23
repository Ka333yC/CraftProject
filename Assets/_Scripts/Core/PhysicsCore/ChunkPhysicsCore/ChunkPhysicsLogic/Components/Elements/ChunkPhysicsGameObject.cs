using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkPhysicsCore.Cache
{
	[RequireComponent(typeof(MeshCollider))]
	public class ChunkPhysicsGameObject : MonoBehaviour
	{
		public MeshCollider MeshCollider { get; private set; }

		private void Awake()
		{
			MeshCollider = GetComponent<MeshCollider>();
		}
	}
}
