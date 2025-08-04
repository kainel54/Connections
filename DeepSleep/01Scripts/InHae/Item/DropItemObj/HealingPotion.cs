using ObjectPooling;
using System;
using UnityEngine;
using YH.Players;

public class HealingPotion : DropItem
{
    [SerializeField] private int _value;
    [SerializeField] private ObjectType _type;
    public override Enum PoolEnum => _type;

    public override void PickUp(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            player.GetCompo<EntityHealth>().ApplyRecovery(_value);
            base.PickUp(other);
        }
    }
    
    public override void OnPop()
    {
        base.OnPop();
        _alreadyCollected = false;
    }
}
