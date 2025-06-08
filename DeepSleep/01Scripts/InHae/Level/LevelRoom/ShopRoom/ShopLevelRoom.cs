using UnityEngine;

public class ShopLevelRoom : LevelRoom
{
    [SerializeField] private DropListSO _itemList;
    [SerializeField] private Transform _itemListParent;

    private bool _initialized = false;
    
    private void Start()
    {
        LevelClear();
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

            if (dropItem is ISpecialInitItem specialInitItem)
            {
                ItemDataSO dataSo = null;

                if (dropItem as PartDropObject)
                    dataSo = _itemList.RandSkillPart();
                if (dropItem as SkillDropObject)
                    dataSo = _itemList.RandSkill();
                if(dropItem as NodeAbilityDropObject)
                    dataSo = _itemList.RandNodeAbility();
                
                specialInitItem.SpecialInit(dataSo);
            }
            
            ShopItem shopItem = _itemListParent.GetChild(i).GetComponent<ShopItem>();
            shopItem.Init(dropItem);
        }
    }
}
