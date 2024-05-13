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
    }
}
