using ObjectPooling;
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

    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }
    public void Init()
    {
        
    }
}
