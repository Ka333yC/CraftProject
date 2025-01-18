using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Core.Extensions
{
	public class BlocksOnDirectionCalculator
{
	public static IEnumerable<Vector3Int> AllBlockPositionsOnLine(Ray ray, int length)
	{
		return AllBlockPositionsOnLine(ray.origin, ray.direction, length);
	}

	public static IEnumerable<Vector3Int> AllBlockPositionsOnLine(Vector3 origin, Vector3 direction, int length)
	{
		Vector3 currentBlockPosition = origin;
		while((currentBlockPosition - origin).magnitude < length)
		{
			Vector3 preventPosition = currentBlockPosition;
			currentBlockPosition = GetNextBlockPosition(currentBlockPosition, direction);
			Vector3 positionEqualsToBlock = (currentBlockPosition + preventPosition) / 2;
			Vector3Int blockPosition = ChunkConstantData.WorldToBlockWorldPosition(positionEqualsToBlock);
			yield return blockPosition;
		}

		yield break;
	}

	private static Vector3 GetNextBlockPosition(Vector3 origin, Vector3 direction)
	{
		Vector3 nextBlockPosition = Vector3.zero;
		float smallestMagnitude = float.MaxValue;
		for(int axis = 0; axis < 3; axis++)
		{
			if(Mathf.Approximately(direction[axis], 0))
			{
				continue;
			}

			// От origin позиции по направлению direction я округяю 3 оси
			// Пример origin = (0, 0, 0) direction = (0.1, -0.7, 0.2)
			// По итогу будет 3 потенциальные позиции: (1, 0, 0), (0, -1, 0) и (0, 0, 1)
			// и для оставшихся осей будут рассчитаны x, y и z соответсвенно уравнению прямой в пространстве
			var roundedValue = (float)(direction[axis] > 0 ?
				Math.Floor(origin[axis] + 1) : Math.Ceiling(origin[axis] - 1));
			var positionOnLine = LineEquation(origin, direction, roundedValue, axis);
			float magnitudesByAxis = (origin - positionOnLine).magnitude;
			if(smallestMagnitude > magnitudesByAxis)
			{
				nextBlockPosition = positionOnLine;
				smallestMagnitude = magnitudesByAxis;
			}
		}

		return nextBlockPosition;
	}

	private static Vector3 LineEquation(Vector3 origin, Vector3 direction, float value, int axis)
	{
		var result = Vector3.zero;
		int axis1 = (axis + 1) % 3;
		int axis2 = (axis + 2) % 3;
		// (x - x0) / d1 = (y - y0) / d2 = (z - z0) / d3 - каноническое уравнение прямой,
		// где M(x0, y0, z0) - известная точка пространства, а Dv(d1, d2, d3) - направляющий вектор
		float canonicEquationValue = (value - origin[axis]) / direction[axis];
		result[axis] = value;
		result[axis1] = origin[axis1] + direction[axis1] * canonicEquationValue;
		result[axis2] = origin[axis2] + direction[axis2] * canonicEquationValue;
		return result;
	}
}
}