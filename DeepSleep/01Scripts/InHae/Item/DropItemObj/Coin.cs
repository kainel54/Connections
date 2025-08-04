using ObjectPooling;
using System;
using UnityEngine;
using YH.Players;

public class Coin : DropItem
{
    public int value { get; set; }
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    
    [SerializeField] private ObjectType _type;
    public override Enum PoolEnum => _type;

    public override void PickUp(Collider other)
    {
        _playerManagerSO.AddCoin(value);
        base.PickUp(other);
    }
}
