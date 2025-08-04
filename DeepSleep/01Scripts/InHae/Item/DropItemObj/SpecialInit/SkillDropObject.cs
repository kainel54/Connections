using IH.Manager;
using ObjectPooling;
using System;
using UnityEngine;

public class SkillDropObject : DropItem, ISpecialInitItem
{
    public SkillItemSO skillItem;

    [SerializeField] private ObjectType _type;
    public override Enum PoolEnum => _type;

    private Transform _visualTrm;
    private bool _isInit;

    public override bool IsCollectAble => InventoryManager.Instance.CanAddItem(skillItem);

    public override void PickUp(Collider other)
    {
        if (IsCollectAble)
        {
            InventoryManager.Instance.AddInventoryItemWithSo(skillItem);
            base.PickUp(other);
        }
    }

    public void SpecialInit(ItemDataSO dataSo)
    {
        itemData = dataSo;
        skillItem = dataSo as SkillItemSO;
    }

    public void VisualInit()
    {
        if (!_isInit)
            _isInit = true;
        
        _visualTrm = Instantiate(skillItem.visual, transform).transform;
        _visualTrm.localPosition = Vector3.zero;
    }
    
    public override void OnPush()
    {
        base.OnPush();
        if (_isInit)
            Destroy(_visualTrm.gameObject);
    }
}
