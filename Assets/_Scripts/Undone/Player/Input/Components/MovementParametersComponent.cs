using System;
using UnityEngine;

namespace _Scripts.Implementation.PlayerImplementation.Movement
{
    [Serializable]
    public struct MovementParametersComponent
    {
        [Header("Horizontal movement")]
        public float HorizontalVelocityUnderControl;
        public float MaxHorizontalVelocity;
        public float HorizontalAcceleration;
        [Header("Vertical movement")]
        public float JumpPower;
        [Header("Rotation movement")]
        // Transform объекта, который будет поворачиваться 
        public Transform RotatableTransform;
    }
}