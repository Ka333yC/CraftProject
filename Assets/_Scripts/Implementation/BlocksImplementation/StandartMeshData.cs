using System;
using System.Collections.Generic;
using _Scripts.Core;
using _Scripts.Core.MeshWrap;
using UnityEngine;

namespace _Scripts.Implementation.BlocksImplementation
{
	[CreateAssetMenu(fileName = "MeshData", menuName = "Blocks/Standart mesh data")]
	public class StandartMeshData : ScriptableObject
	{
		public bool HasFullForwardSide;
		public bool HasFullBackSide;
		public bool HasFullUpSide;
		public bool HasFullDownSide;
		public bool HasFullRightSide;
		public bool HasFullLeftSide;

		public MeshDataPart[] OtherMeshParts;

		public MeshDataPart GetSide(Face face)
		{
			return face switch
			{
				Face.Forward => HasFullForwardSide ? MeshDataPart.FullForwardSide : null,
				Face.Back => HasFullBackSide ? MeshDataPart.FullBackSide : null,
				Face.Up => HasFullUpSide ? MeshDataPart.FullUpSide : null,
				Face.Down => HasFullDownSide ? MeshDataPart.FullDownSide : null,
				Face.Right => HasFullRightSide ? MeshDataPart.FullRightSide : null,
				Face.Left => HasFullLeftSide ? MeshDataPart.FullLeftSide : null,
				_ => throw new ArgumentException($"Unknown face \"{face}\"")
			};
		}

		public bool HasSide(Face face)
		{
			return face switch
			{
				Face.Forward => HasFullForwardSide,
				Face.Back => HasFullBackSide,
				Face.Up => HasFullUpSide,
				Face.Down => HasFullDownSide,
				Face.Right => HasFullRightSide,
				Face.Left => HasFullLeftSide,
				_ => throw new ArgumentException($"Unknown face \"{face}\"")
			};
		}

		public IEnumerable<MeshDataPart> GetAllSides()
		{
			if(HasFullForwardSide)
			{
				yield return MeshDataPart.FullForwardSide;
			}

			if(HasFullBackSide)
			{
				yield return MeshDataPart.FullBackSide;
			}

			if(HasFullUpSide)
			{
				yield return MeshDataPart.FullUpSide;
			}

			if(HasFullDownSide)
			{
				yield return MeshDataPart.FullDownSide;
			}

			if(HasFullRightSide)
			{
				yield return MeshDataPart.FullRightSide;
			}

			if(HasFullLeftSide)
			{
				yield return MeshDataPart.FullLeftSide;
			}

			foreach(var otherMeshPart in OtherMeshParts)
			{
				yield return otherMeshPart;
			}

			yield break;
		}
	}
}
