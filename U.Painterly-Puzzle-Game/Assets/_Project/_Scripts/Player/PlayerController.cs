using System.Collections.Generic;
using UnityEngine;

namespace Padrox {
    public class  PlayerController : MonoBehaviour {
        [ExposedScriptableObject, SerializeField]
        private PlayerData _data;

        private Rigidbody _rb;
        private Vector2 _inputDir;
        private Vector3 _moveDir;
        private bool _isOnSlope;
        private RaycastHit _slopeHit;

        private void Awake() {
            _rb = GetComponent<Rigidbody>();

            _inputDir = Vector2.zero;
        }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void FixedUpdate() {
            //EvaluateMoveDir
            //HandleMovement();
            //HandleRotation();
            //ApplyDrag();
            //RestraintSpeed();
        }
    }
}