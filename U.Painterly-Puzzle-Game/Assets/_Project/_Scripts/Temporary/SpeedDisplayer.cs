using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;

namespace Padrox
{
    public class SpeedDisplayer : MonoBehaviour
    {
        [SerializeField] private Rigidbody _plrRb;
        [SerializeField] private TextMeshProUGUI _text;

        private void Update() {
            UpdateText();
        }

        private void UpdateText() {
            _text.SetText(string.Format("Speed: <color=#00FFFF>{0:0.00}</color>", _plrRb.velocity.magnitude));
        }
    }
}
