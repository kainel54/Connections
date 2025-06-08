using IH.Manager;
using ObjectPooling;
using System;
using UnityEngine;

public class PartDropObject : DropItem, IPoolable, ISpecialInitItem
{
    public PartItemSO partItem;

    public override void PickUp(Collider other)
    {
        if (InventoryManager.Instance.CanAddItem(partItem))
        {
            InventoryManager.Instance.AddInventoryItemWithSo(partItem);
            Destroy(gameObject);
        }
    }

    public void SpecialInit(ItemDataSO dataSo)
    {
        itemData = dataSo;
        partItem = dataSo as PartItemSO;
    }

    public void VisualInit()
    {
        Transform visualTrm = Instantiate(partItem.visual, transform).transform;
        visualTrm.localPosition = Vector3.zero;
    }

    public GameObject GameObject { get => gameObject; set { } }
    public Enum PoolEnum { get => _type; set { } }
    [SerializeField] private ObjectType _type;
    public void Init()
    {
        itemData = null;
        partItem = null;
    }

    public void OnPop()
    {

    }

    public void OnPush()
    {

    }
}
