using ObjectPooling;
using System;
using UnityEngine;
using YH.Players;

public class HealingPotion : DropItem, IPoolable
{
    [SerializeField] private int _value;

    public GameObject GameObject { get => gameObject; set { } }
    [SerializeField] private ObjectType _type;
    public Enum PoolEnum { get => _type; set { } }

    public override void PickUp(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.GetCompo<EntityHealth>().ApplyRecovery(_value);
            this.Push();
        }
    }
    public void OnPop()
    {
        _alreadyCollected = false;
    }

    public void OnPush()
    {
    }

    
}
