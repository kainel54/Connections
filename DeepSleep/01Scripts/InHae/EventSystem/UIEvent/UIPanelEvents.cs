using System;
using UnityEngine;
using YH.EventSystem;

namespace IH.EventSystem.UIEvent.PanelEvent
{
    public static class UIPanelEvent
    {
        public static WindowPanelToggleEvent WindowPanelToggleEvent = new WindowPanelToggleEvent();
        public static WindowPanelOpenEvent WindowPanelOpenEvent = new WindowPanelOpenEvent();
        public static WindowPanelCloseEvent WindowPanelCloseEvent = new WindowPanelCloseEvent();
        public static WindowPanelLockEvent WindowPanelLockEvent = new WindowPanelLockEvent();
        
        public static UpgradePanelEvent UpgradePanelEvent = new UpgradePanelEvent();
        
        public static ShopDescriptionPanelEvent ShopDescriptionPanelEvent = new ShopDescriptionPanelEvent();
        public static SkillPanelLockEvent SkillPanelLockEvent = new SkillPanelLockEvent();
    }

    public class WindowPanelToggleEvent : GameEvent
    {
        public WindowPanel currentWindow;
    }
        
    public class WindowPanelOpenEvent : GameEvent
    {
        public WindowPanel currentWindow;
    }

    public class WindowPanelCloseEvent : GameEvent
    {
        public WindowPanel currentWindow;
    }

    public class WindowPanelLockEvent : GameEvent
    {
        public bool isOpenLocked;
        public bool isCloseLocked;
    }
    
    public class UpgradePanelEvent : GameEvent
    {
        public bool isPanelActive;
    }
    
    public class ShopDescriptionPanelEvent: GameEvent
    {
        public bool isPanelActive;
        public bool canBuyItem;
        public Action buyItemAction; 
        public ItemDataSO itemDataSo;
        public Color textColor;
    }

    public class SkillPanelLockEvent : GameEvent
    {
        public bool isLocked;
    }
}

