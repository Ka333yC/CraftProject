using PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Core.ChunkPhysicsCore.ChunkPhysicsScripts.Components.Elements.BlockPhysicsGetters
{
	public interface IBlocksPhysicsGetter
	{
		public IPhysicsBlockComponent GetBlockPhysics(Vector3Int blockPosition);
	}
}
