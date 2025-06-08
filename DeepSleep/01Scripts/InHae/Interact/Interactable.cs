using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public event Action HandleInteractAction;

    public virtual void Interact()
    {
        HandleInteractAction?.Invoke();
    }

    public abstract void EnableDescription();
    public abstract void DisableDescription();
}
