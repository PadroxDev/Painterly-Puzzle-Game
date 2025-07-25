using System;
using System.Collections.Generic;
using UnityEngine;

namespace Padrox {
    public class  PlayerController : MonoBehaviour {
        private const float MOVE_SPEED_MUL = 10f;

#if UNITY_EDITOR
        [ExposedScriptableObject]
#endif
        [SerializeField] private PlayerData _data;

        [Header("References")]
        [SerializeField] private Transform _groundCheck;
        [SerializeField] private Transform _orientation;

        private Rigidbody _rb;
        private Camera _cam;
        private Vector2 _inputDir;
        private Vector3 _moveDir;
        private bool _isGrounded;
        private bool _isOnSlope;
        private bool _isOnSlopeDirty;
        private bool _movingBackward;
        private bool _movingBackwardDirty;
        private RaycastHit _slopeHit;
        
        public float CurrentSpeed { get; private set; }

        public Vector2 InputDir {
            get { return _inputDir; }
        }

        private void Awake() {
            _rb = GetComponent<Rigidbody>();
            _cam = Helpers.Camera;

            _inputDir = Vector2.zero;

            _isGrounded = false;
            _isOnSlope = false;
            _isOnSlopeDirty = true;
            _movingBackward = false;
            _movingBackwardDirty = true;

            CurrentSpeed = 0;
        }

        private void Start() {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void FixedUpdate() {
            Cleanup();
            GroundCheck();
            EvaluateMoveDir();
            HandleMovement();
            HandleRotation();
            ApplyDrag();
            RestraintSpeed();
            CurrentSpeed = _rb.velocity.magnitude;
        }

        private void GroundCheck() {
            _isGrounded = Physics.Raycast(_groundCheck.position, Vector3.down, _data.GroundCheckDistance, _data.WhatIsGround);
        }

        private void EvaluateMoveDir() {
            _inputDir = InputManager.Instance.GetMoveInput();

            Vector3 camPos = new Vector3(_cam.transform.position.x, transform.position.y, _cam.transform.position.z);
            Vector3 viewDir = (transform.position - camPos).normalized;
            if(viewDir != Vector3.zero) _orientation.forward = viewDir;

            _moveDir = _orientation.forward * _inputDir.y + _orientation.right * _inputDir.x;
        }

        private void HandleMovement() {
            _rb.useGravity = !OnSlope();

            if (_moveDir == Vector3.zero) return;

            Vector3 dir = OnSlope() ? GetSlopeMoveDirection() : _moveDir;
            _rb.AddForce(dir * GetMaxSpeed() * MOVE_SPEED_MUL, ForceMode.Force);
        }

        private void HandleRotation() {
            if (_moveDir == Vector3.zero) return;
            Vector3 rotateTowards = IsMovingBackward() ? _moveDir * -1 : _moveDir;
            transform.forward = Vector3.Slerp(transform.forward, rotateTowards.normalized, Time.fixedDeltaTime * _data.RotationSpeed);
        }

        private void ApplyDrag() {
            _rb.drag = _isGrounded ? _data.GroundDrag : _data.AirDrag;
        }

        private void RestraintSpeed() {
            Vector3 vel;
            if (OnSlope()) {
                vel = _rb.velocity;
            } else { // Flat
                vel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            }

            float speed = vel.magnitude;
            float maxSpeed = GetMaxSpeed();
            if (speed <= GetMaxSpeed()) return;

            Vector3 restraintSpeed = vel.normalized * maxSpeed;
            if (!OnSlope()) { // Apply the current downwards velocity
                restraintSpeed += Vector3.up * _rb.velocity.y;
            }

            _rb.velocity = restraintSpeed;
        }

        private bool OnSlope() {
            if (_isOnSlopeDirty) {
                _isOnSlopeDirty = false;
                Ray ray = new Ray(_groundCheck.position, Vector3.down);
                if (Physics.Raycast(ray, out _slopeHit, _data.SlopeCheckDistance, _data.WhatIsGround)) {
                    float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                    _isOnSlope = angle < _data.MaxSlopeAngle && angle != 0;
                } else {
                    _isOnSlope = false;
                }   
            }

            return _isOnSlope;
        }

        private bool IsMovingBackward() {
            if(_movingBackwardDirty) {
                _movingBackwardDirty = false;
                Vector3 camBack = new Vector3(-_cam.transform.forward.x, 0, -_cam.transform.forward.z);
                float backwardAngle = Vector3.Angle(camBack, _moveDir);
                _movingBackward = backwardAngle < _data.BackwardMinAngle;
            }

            return _movingBackward;
        }

        private Vector3 GetSlopeMoveDirection() {
            return Vector3.ProjectOnPlane(_moveDir, _slopeHit.normal);
        }

        private void Cleanup() {
            _isOnSlopeDirty = true;
            _movingBackwardDirty = true;
        }

        public float GetMaxSpeed() {
            return IsMovingBackward() ? _data.MoveSpeed * _data.BackwardSpeedMultiplier : _data.MoveSpeed;
        }

        private void OnDrawGizmosSelected() {
            if (!_groundCheck) return;
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(_groundCheck.position, _groundCheck.position + Vector3.down * _data.SlopeCheckDistance);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(_groundCheck.position, _groundCheck.position + Vector3.down * _data.GroundCheckDistance);
        }
    }
}