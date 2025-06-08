using ObjectPooling;
using System;
using UnityEngine;
using YH.Players;

public class Coin : DropItem, IPoolable
{
    public int value { get; set; }
    [SerializeField] private PlayerManagerSO _playerManagerSO;

    public override void PickUp(Collider other)
    {
        _playerManagerSO.AddCoin(value);
        gameObject.SetActive(false);
    }

    [field: SerializeField] public PoolingKey PoolKey { get; set; }
    public GameObject GameObject { get => gameObject; set { } }
    [SerializeField] private ObjectType _type;
    public Enum PoolEnum { get => _type; set { } }

    public void Init()
    {
        
    }

    public void OnPop()
    {

    }

    public void OnPush()
    {

    }
}
