using System.Collections.Generic;
using Assets._Scripts.Core.BlocksCore;
using ChunkCore.BlockData;
using Leopotam.EcsLite;
using MeshCreation.Preset;
using PhysicsCore.ChunkPhysicsCore.BlockPhysics;
using TempScripts;
using UnityEngine;
using Zenject;

namespace Realization.Blocks.Cube.Physics
{
	[CreateAssetMenu(fileName = "BlockPhysicsElement", menuName = "Blocks/Physics/Standart block physics element")]
	public class StandartBlockPhysicsElementContainer : BlockComponentContainer, IPhysicsBlockComponentContainer
	{
		[field: SerializeField] 
		public StandartMeshData MeshData { get; private set; }

		public override bool CanInitializeAsync => true;

		public override void InitializeBlock(Block block)
		{
			block.AddComponent(new StandartBlockPhysics(this));
		}
	}
}
