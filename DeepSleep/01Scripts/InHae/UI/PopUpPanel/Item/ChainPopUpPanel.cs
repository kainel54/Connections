using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ChainPopUpPanel : ItemPopUpPanel
{
    [SerializeField] private Transform _slotParent;
    private List<ChainSlotUI> _chainSlots = new List<ChainSlotUI>();

    private int _index;
    
    protected override void Awake()
    {
        base.Awake();
        _chainSlots = _slotParent.GetComponentsInChildren<ChainSlotUI>().ToList();
    }

    public void ChainListPopUp(List<PartInventoryItem> partInventory, PartNodeUIChainCheck uiChainCheck)
    {
        isOnPopUp = true;

        transform.DOScale(Vector3.one, 0.25f).SetUpdate(true);
        UpdatePopUp(partInventory, uiChainCheck);
    }

    public void UpdatePopUp(List<PartInventoryItem> partInventory, PartNodeUIChainCheck uiChainCheck)
    {
        for (int i = 0; i < partInventory.Count; i++)
        {
            _chainSlots[i].UpdateSlot(partInventory[i]);
            _chainSlots[i].Init(uiChainCheck);
        }
    }
    
    public override void EndPopUp()
    {
        base.EndPopUp();
        transform.DOScale(Vector3.zero, 0.25f)
            .OnComplete(()=>_chainSlots.ForEach(x=>x.CleanUpSlot()))
            .SetUpdate(true);
    }
}
