using System;
using IH.Manager;
using ObjectPooling;
using UnityEngine;

public class NodeAbilityDropObject : DropItem, IPoolable, ISpecialInitItem
{
    public NodeAbilityItemSO nodeAbilityItemSo;

    public override void PickUp(Collider other)
    {
        if (InventoryManager.Instance.CanAddItem(nodeAbilityItemSo))
        {
            InventoryManager.Instance.AddInventoryItemWithSo(nodeAbilityItemSo);
            Destroy(gameObject);
        }
    }

    public void SpecialInit(ItemDataSO dataSo)
    {
        itemData = dataSo;
        nodeAbilityItemSo = dataSo as NodeAbilityItemSO;
    }

    public void VisualInit()
    {
        Transform visualTrm = Instantiate(nodeAbilityItemSo.visual, transform).transform;
        visualTrm.localPosition = Vector3.zero;
    }

    public GameObject GameObject { get => gameObject; set { } }
    public Enum PoolEnum { get => _type; set { } }
    [SerializeField] private ObjectType _type;
    
    public void Init()
    {
        itemData = null;
        nodeAbilityItemSo = null;
    }

    public void OnPop()
    {

    }

    public void OnPush()
    {

    }
}
