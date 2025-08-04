using System;
using IH.Manager;
using ObjectPooling;
using UnityEngine;

public class NodeAbilityDropObject : DropItem, ISpecialInitItem
{
    public NodeAbilityItemSO nodeAbilityItemSo;
    [SerializeField] private ObjectType _type;
    public override Enum PoolEnum => _type;

    private Transform _visualTrm;
    private bool _isInit;
    
    public override bool IsCollectAble => InventoryManager.Instance.CanAddItem(nodeAbilityItemSo);
    
    public override void PickUp(Collider other)
    {
        if (IsCollectAble)
        {
            InventoryManager.Instance.AddInventoryItemWithSo(nodeAbilityItemSo);
            base.PickUp(other);
        }
    }

    public void SpecialInit(ItemDataSO dataSo)
    {
        itemData = dataSo;
        nodeAbilityItemSo = dataSo as NodeAbilityItemSO;
    }

    public void VisualInit()
    {
        if (!_isInit)
            _isInit = true;
        
        _visualTrm = Instantiate(nodeAbilityItemSo.visual, transform).transform;
        _visualTrm.localPosition = Vector3.zero;
    }
    
    public override void OnPush()
    {
        base.OnPush();
        if (_isInit)
            Destroy(_visualTrm.gameObject);
    }
}
