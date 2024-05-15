using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Padrox
{
    [DefaultExecutionOrder(+1)]
    [RequireComponent(typeof(PlayerController))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private PlayerController _controller;
        private Rigidbody _rb;

        private float _moveX;
        private float _moveZ;

        private int _hashMoveX;
        private int _hashMoveY;

        private void Awake() {
            _controller = GetComponent<PlayerController>();
            _rb = GetComponent<Rigidbody>();

            _moveX = 0f;
            _moveZ = 0f;
        }

        private void Start()
        {
            _hashMoveX = Animator.StringToHash("MoveX");
            _hashMoveY = Animator.StringToHash("MoveZ");
        }

        void Update()
        {
            EvaluateAnimatorValues();
            UpdateAnimatorValues();
        }

        private void EvaluateAnimatorValues() {
            Vector2 input = _controller.InputDir.normalized;
            if(input == Vector2.zero) {
                _moveX = 0f;
                _moveZ = 0f;
                return;
            }
            Vector2 posIn = new Vector2(
                Mathf.Abs(input.x),
                Mathf.Abs(input.y)
            );

            Vector2 res;
            if (posIn.x > posIn.y) {
                float h = 1 / (float)Math.Cos(Math.Asin(input.y));

                res.x = input.x > 0 ? 1 : -1;
                res.y = Mathf.Sqrt(Mathf.Pow(h, 2) - 1);
                if (input.y < 0) res.y *= -1;
            } else {
                float h = 1 / (float)Math.Cos(Math.Asin(input.x));

                res.y = input.y > 0 ? 1 : -1;
                res.x = Mathf.Sqrt(Mathf.Pow(h, 2) - 1);
                if (input.x < 0) res.x *= -1;
            }

            res *= Mathf.Clamp01(_controller.CurrentSpeed / _controller.GetMaxSpeed());

            _moveX = res.x;
            _moveZ = res.y;
        }

        private void UpdateAnimatorValues() {
            _animator.SetFloat(_hashMoveX, _moveX);
            _animator.SetFloat(_hashMoveY, _moveZ);
        }
    }
}
