using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Padrox
{
    public class PointOfView : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _viewCamera;

        private void OnTriggerEnter(Collider other) {
            _viewCamera.Priority = 20;
        }

        private void OnTriggerExit(Collider other) {
            _viewCamera.Priority = 0;
        }
    }
}
