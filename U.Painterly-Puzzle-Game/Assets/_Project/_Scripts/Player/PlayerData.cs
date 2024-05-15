using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Padrox
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Player/Player Data")]
    public class PlayerData : ScriptableObject
    {
        [Header("Movement")]
        public float MoveSpeed;
        public float RotationSpeed;

        [Space]
        public float BackwardSpeedMultiplier;
        [Tooltip("Angle between MoveDir and Camera back vector to determine if the player is moving backwards")]
        public float BackwardMinAngle;

        [Space]
        public float GroundDrag;
        public float AirDrag;

        [Header("Ground Check & Slope")]
        public float GroundCheckDistance;
        public float SlopeCheckDistance;
        [Range(0, 180)] public float MaxSlopeAngle;

        [Header("Layers")]
        public LayerMask WhatIsGround;
    }
}