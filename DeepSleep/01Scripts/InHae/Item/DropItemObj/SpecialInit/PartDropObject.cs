using IH.Manager;
using ObjectPooling;
using System;
using UnityEngine;

public class PartDropObject : DropItem, ISpecialInitItem
{
    public PartItemSO partItem;
    [SerializeField] private ObjectType _type;
    public override Enum PoolEnum => _type;

    private Transform _visualTrm;

    private bool _isInit;
    
    public override bool IsCollectAble => InventoryManager.Instance.CanAddItem(partItem);
    
    public override void PickUp(Collider other)
    {
        if (IsCollectAble)
        {
            InventoryManager.Instance.AddInventoryItemWithSo(partItem);
            base.PickUp(other);
        }
    }

    public void SpecialInit(ItemDataSO dataSo)
    {
        itemData = dataSo;
        partItem = dataSo as PartItemSO;
    }

    public void VisualInit()
    {
        if (!_isInit)
            _isInit = true;
        
        _visualTrm = Instantiate(partItem.visual, transform).transform;
        _visualTrm.localPosition = Vector3.zero;
    }

    public override void OnPush()
    {
        base.OnPush();
        if (_isInit)
            Destroy(_visualTrm.gameObject);
    }
}
