using System;
using YH.EventSystem;

public static class UIEvents
{
    public static ShopDescriptionEvent ShopDescriptionEvent = new ShopDescriptionEvent();
    public static UpgradePanelEvent UpgradePanelEvent = new UpgradePanelEvent();
    public static SkillHudEvent SkillHudEvent = new SkillHudEvent();
    public static WindowPanelOpenEvent WindowPanelOpenEvent = new WindowPanelOpenEvent();
    public static WindowPanelCloseEvent WindowPanelCloseEvent = new WindowPanelCloseEvent();
    public static UpgradeSelectSkillEvent UpgradeSelectSkillEvent = new UpgradeSelectSkillEvent();
}

public class SkillHudEvent : GameEvent
{
    public Skill skill;
    public SkillItemSO SkillItemData;
    public int skillIdx;
}

public class WindowPanelOpenEvent : GameEvent
{
    public WindowPanel currentWindow;
}

public class WindowPanelCloseEvent : GameEvent { }

public class ShopDescriptionEvent: GameEvent
{
    public bool isPanelActive;
    public bool canBuyItem;
    public Action buyItemAction; 
    public ItemDataSO itemDataSo;
}

public class UpgradePanelEvent : GameEvent
{
    public bool isPanelActive;
}

public class UpgradeSelectSkillEvent : GameEvent
{
    public SkillInventoryItem item;
}
