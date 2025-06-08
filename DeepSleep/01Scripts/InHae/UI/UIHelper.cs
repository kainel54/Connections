using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIHelper : MonoSingleton<UIHelper>
{
    [SerializeField] private PlayerStatusPopUpUI _playerStatusPopUpUI;
    [SerializeField] private SkillStatPopUpUI _skillStatPopUpUI;
    
    [SerializeField] private Transform _dragItemParent;
    private Dictionary<DragItemType, DragItem> _dragItems = new Dictionary<DragItemType, DragItem>();
    
    [SerializeField] private Transform _popUpPanelParent;
    private Dictionary<ItemPopUpItemType, ItemPopUpPanel> _popUpPanels = new();
    
    private void Awake()
    {
        _dragItemParent.GetComponentsInChildren<DragItem>(true).ToList()
            .ForEach(x => _dragItems.Add(x.dragItemType, x));
        
        _popUpPanelParent.GetComponentsInChildren<ItemPopUpPanel>(true).ToList()
            .ForEach(x => _popUpPanels.Add(x.popUpType, x));
    }

    public DragItem GetDragItem(DragItemType type) => _dragItems[type];
    public ItemPopUpPanel GetPopUpPanel(ItemPopUpItemType type) => _popUpPanels[type];
    
    public PlayerStatusPopUpUI GetPlayerStatusTooltip() => _playerStatusPopUpUI;
    public SkillStatPopUpUI GetSkillStatPopUpUI() => _skillStatPopUpUI;
}
