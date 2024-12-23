using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkGraphicsCore.Cache
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
	public class ChunkGraphicsGameObject : MonoBehaviour
	{
		public MeshFilter MeshFilter { get; private set; }

		private void Awake()
		{
			MeshFilter = GetComponent<MeshFilter>();
		}
	}
}
