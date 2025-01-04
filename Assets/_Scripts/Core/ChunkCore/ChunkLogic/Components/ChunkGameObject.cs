using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts.Core.ChunkCore.ChunkLogic.Components
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshCollider))]
	public class ChunkGameObject : MonoBehaviour
	{
		public MeshFilter MeshFilter { get; private set; }
		public MeshCollider MeshCollider { get; private set; }

		private void Awake()
		{
			MeshFilter = GetComponent<MeshFilter>();
			MeshCollider = GetComponent<MeshCollider>();
		}
	}
}
