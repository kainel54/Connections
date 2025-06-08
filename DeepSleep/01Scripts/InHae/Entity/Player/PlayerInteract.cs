using System;
using UnityEngine;
using YH.Entities;
using YH.Players;

public class PlayerInteract : MonoBehaviour, IEntityComponent
{
    [Header("Interactable Search Value")]
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private float searchSize;
    
    private Player _player;
    private Interactable _currentInteract;

    public void Initialize(Entity entity)
    {
        _player = entity as Player;
    }

    private void OnDestroy()
    {
        _player.PlayerInput.InteractEvent -= InteractEvent;
    }

    private void Update()
    {
        if (SearchInteractable())
        {            
            _currentInteract.EnableDescription();
            if (_player.PlayerInput.InteractEvent == null)
                _player.PlayerInput.InteractEvent += InteractEvent;
        }
        else
        {
            if (_currentInteract != null)
            {
                _currentInteract.DisableDescription();
                _player.PlayerInput.InteractEvent -= InteractEvent;
                _currentInteract = null;
            }
        }
    }
    
    private bool SearchInteractable()
    {
        Collider[] result = Physics.OverlapSphere(transform.position, searchSize, interactableMask);
        
        if (result.Length > 0)
        {
            if (result[0].TryGetComponent(out Interactable interactable))
            {
                _currentInteract = interactable;
            }
            return true;
        }
        return false;
    }

    private void InteractEvent()
    {
        if(_currentInteract==null)
            return;

        _player.PlayerInput.InteractEvent -= InteractEvent;
        _currentInteract.DisableDescription();
        _currentInteract.Interact();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, searchSize);
    }
}
