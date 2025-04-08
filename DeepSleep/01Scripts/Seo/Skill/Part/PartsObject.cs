using IH;
using IH.Manager;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Serialization;

public class PartsObject : DropItem, IPoolable
{
    public PartItemSO partItem;
    public GameObject partsObj;

    public override void PickUp(Collider other)
    {
        if (InventoryManager.Instance.CanAddItem(partItem))
        {
            InventoryManager.Instance.AddInventoryItemWithSo(partItem);
            Destroy(gameObject);
        }
    }

    public void PartInit(PartItemSO partItemSO)
    {
        itemData = partItemSO;
        this.partItem = partItemSO;
    }

    public void VisualInit()
    {
        Transform visualTrm = Instantiate(partItem.visual, transform).transform;
        visualTrm.localPosition = Vector3.zero;
    }

    public GameObject GameObject { get => gameObject; set { } }
    [field:SerializeField]public PoolingType PoolType { get; set; }
    
    public void Init()
    {
        itemData = null;
        partItem = null;
    }
}
