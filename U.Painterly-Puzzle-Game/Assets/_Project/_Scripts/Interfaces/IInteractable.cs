using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Padrox
{
    public interface IInteractable
    {
        public bool CanInteract() { return true; }

        public void Interact();
    }
}
