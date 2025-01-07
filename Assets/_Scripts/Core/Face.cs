using System;
using UnityEngine;

namespace _Scripts.Core
{
	public enum Face : byte
	{
		None = 0,
		Forward = 1,
		Back = 2,
		Up = 4,
		Down = 8,
		Right = 16,
		Left = 32,
		All = 63,
	}

	public static class FaceExtensions
	{
		public static bool HasFace(this Face faces, Face faceToCheck)
		{
			return (faces | faceToCheck) == faces;
		}

		public static void AddFace(ref this Face faces, Face faceToAdd)
		{
			faces |= faceToAdd;
		}

		public static Face Reverse(this Face face)
		{
			return face switch
			{
				Face.Forward => Face.Back,
				Face.Back => Face.Forward,
				Face.Up => Face.Down,
				Face.Down => Face.Up,
				Face.Right => Face.Left,
				Face.Left => Face.Right,

				_ => throw new ArgumentException($"Unknown face \"{face}\"")
			};
		}

		public static Vector3Int ToVector(this Face face)
		{
			return face switch
			{
				Face.Forward => Vector3Int.forward,
				Face.Back => Vector3Int.back,
				Face.Up => Vector3Int.up,
				Face.Down => Vector3Int.down,
				Face.Right => Vector3Int.right,
				Face.Left => Vector3Int.left,

				_ => throw new ArgumentException($"Unknown face \"{face}\"")
			};
		}

		public static Bounds ToBounds(this Face face)
		{
			Vector3 min;
			Vector3 max;
			switch(face)
			{
				case Face.Forward:
					min = new Vector3(0, 0, 1);
					max = new Vector3(1, 1, 1);
					break;
				case Face.Back:
					min = new Vector3(0, 0, 0);
					max = new Vector3(1, 1, 0);
					break;
				case Face.Up:
					min = new Vector3(0, 1, 0);
					max = new Vector3(1, 1, 1);
					break;
				case Face.Down:
					min = new Vector3(0, 0, 0);
					max = new Vector3(1, 0, 1);
					break;
				case Face.Right:
					min = new Vector3(1, 0, 0);
					max = new Vector3(1, 1, 1);
					break;
				case Face.Left:
					min = new Vector3(0, 0, 0);
					max = new Vector3(0, 1, 1);
					break;
				default:
					throw new ArgumentException($"Unknown face \"{face}\"");
			}

			var bounds = new Bounds();
			bounds.SetMinMax(min, max);
			return bounds;
		}
	}
}
