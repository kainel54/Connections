using UnityEngine;

public class ShopLevelRoom : LevelRoom
{
    [SerializeField] private DropListSO _itemList;
    [SerializeField] private Transform _itemListParent;

    private bool _initialized = false;
    
    private void Start()
    {
        isClear = true;
        ConnectDoorDisable();
        
        ItemInit();
    }

    private void ItemInit()
    {
        if(_initialized)
            return;

        _initialized = true;
        
        _itemListParent.rotation = Quaternion.Euler(0, -90, 0);
        
        for (int i = 0; i < _itemListParent.childCount; i++)
        {
            DropItem dropItem = _itemList.RandItem();

            if (dropItem is SkillObject skillObject)
                skillObject.SkillInit(_itemList.RandSkill());

            if (dropItem is PartsObject partsObject)
                partsObject.PartInit(_itemList.RandSkillParts());
            
            ShopItem shopItem = _itemListParent.GetChild(i).GetComponent<ShopItem>();
            Debug.Log($"{_itemListParent.GetChild(i).gameObject.name}");
            shopItem.Init(dropItem);
        }
    }
}
