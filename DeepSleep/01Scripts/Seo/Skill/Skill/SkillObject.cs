using IH.Manager;
using ObjectPooling;
using UnityEngine;

public class SkillObject : DropItem, IPoolable
{
    public SkillItemSO skillItem;
    [HideInInspector] public Transform visual;

    public override void PickUp(Collider other)
    {
        if (InventoryManager.Instance.CanAddItem(skillItem))
        {
            InventoryManager.Instance.AddInventoryItemWithSo(skillItem);
            Destroy(gameObject);
        }
    }

    public void SkillInit(SkillItemSO skillItem)
    {
        itemData = skillItem;
        this.skillItem = skillItem;
    }

    public void VisualInit()
    {
        Transform visualTrm = Instantiate(skillItem.visual, transform).transform;
        visualTrm.localPosition = Vector3.zero;
    }

    public GameObject GameObject { get => gameObject; set { } }
    [field: SerializeField] public PoolingType PoolType { get; set; }

    public void Init()
    {
        itemData = null;
        skillItem = null;
    }
}
