using IH.Manager;
using ObjectPooling;
using System;
using UnityEngine;

public class SkillDropObject : DropItem, IPoolable, ISpecialInitItem
{
    public SkillItemSO skillItem;

    public GameObject GameObject { get => gameObject; set { } }
    [SerializeField] private ObjectType _type;
    public Enum PoolEnum { get => _type; set { } }

    public override void PickUp(Collider other)
    {
        if (InventoryManager.Instance.CanAddItem(skillItem))
        {
            InventoryManager.Instance.AddInventoryItemWithSo(skillItem);
            Destroy(gameObject);
        }
    }

    public void SpecialInit(ItemDataSO dataSo)
    {
        itemData = dataSo;
        this.skillItem = dataSo as SkillItemSO;
    }

    public void VisualInit()
    {
        Transform visualTrm = Instantiate(skillItem.visual, transform).transform;
        visualTrm.localPosition = Vector3.zero;
    }

    public void VisualInit(Transform parent)
    {
        Transform visualTrm = Instantiate(skillItem.visual, parent).transform;
        visualTrm.localPosition = Vector3.zero;
    }

    public void Init()
    {
        itemData = null;
        skillItem = null;
    }

    public void OnPop()
    {
    }

    public void OnPush()
    {
    }
}
