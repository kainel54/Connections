using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChainPopUpPanel : ItemPopUpPanel
{
    [SerializeField] private Transform _slotParent;
    private List<ChainSlotUI> _chainSlots = new List<ChainSlotUI>();

    private bool _isOpen;
    private CanvasGroup _canvasGroup;
    
    protected override void Awake()
    {
        base.Awake();
        _canvasGroup = GetComponent<CanvasGroup>();
        
        _chainSlots = _slotParent.GetComponentsInChildren<ChainSlotUI>().ToList();
    }

    public void ChainListPopUp(List<PartInventoryItem> partInventory, PartNodeChainCheck chainCheck)
    {
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1;
        UpdatePopUp(partInventory, chainCheck);
    }

    public void UpdatePopUp(List<PartInventoryItem> partInventory, PartNodeChainCheck chainCheck)
    {
        for (int i = 0; i < partInventory.Count; i++)
        {
            _chainSlots[i].UpdateSlot(partInventory[i]);
            _chainSlots[i].Init(chainCheck);
        }
    }
    
    public override void EndPopUp()
    {
        _chainSlots.ForEach(x=>x.CleanUpSlot());
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;
    }
}
