using DG.Tweening;
using IH.EventSystem.InteractEvent;
using IH.EventSystem.LevelEvent;
using UnityEngine;
using YH.Core;
using YH.EventSystem;

public class Totem : Interactable
{
    [SerializeField] private GameEventChannelSO _interactEventChannelSO;
    [SerializeField] private GameEventChannelSO _destroyTotemEvent;

    [SerializeField] private DropListSO _itemList;
    [SerializeField] private DropItem _dropItem = null;

    [SerializeField] private LayerMask _layerMask;

    private bool _canInteract;

    private float _originYPos = 0.0f;
    private float _targetYPos = 10.0f;
    private float _itemYDelta = 13.0f;

    private float _tweenTime = 1.2f;
    private float _shakingPower = 0.8f;

    public void RaiseTotem()
    {
        CameraManager.Instance.ShakeCamera(_shakingPower, _shakingPower, _tweenTime);

        transform.DOMoveY(_targetYPos, _tweenTime).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                SpawnObjects();
            });
    }

    public void LowerTotem()
    {
        if (_dropItem != null && _dropItem.HasTagged) 
            return;
        
        _canInteract = false;
        
        CameraManager.Instance.ShakeCamera(_shakingPower, _shakingPower, _tweenTime);
        transform.DOMoveY(_originYPos, _tweenTime).SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    Clear();
                });
    }

    public void Clear()
    {
        _dropItem = null;
    }

    public void SpawnObjects()
    {
        _dropItem = PoolManager.Instance.Pop(_itemList.RandItem().PoolEnum) as DropItem;
        _dropItem.HasTagged = false;
        _dropItem.transform.position = transform.position;
        _dropItem.transform.parent = transform;

        if (_dropItem is ISpecialInitItem specialInitItem)
        {
            ItemDataSO dataSo = null;

            if (_dropItem as PartDropObject)
                dataSo = _itemList.RandSkillPart();
            if (_dropItem as SkillDropObject)
                dataSo = _itemList.RandSkill();
            if (_dropItem as NodeAbilityDropObject)
                dataSo = _itemList.RandNodeAbility();

            specialInitItem.SpecialInit(dataSo);
            specialInitItem.VisualInit();
        }
        
        _dropItem.SetItemDropPosition(new Vector3(transform.position.x, _itemYDelta, transform.position.z), _layerMask);
        _canInteract= true;
    }

    public override void Interact()
    {
        if (_dropItem == null || !_canInteract) 
            return;

        base.Interact();
        _dropItem.HasTagged = true;
        _canInteract = false;

        var evt = LevelEvents.DestroyTotemEvent;
        _destroyTotemEvent.RaiseEvent(evt);
    }

    public override void EnableDescription()
    {
        if(!_canInteract)
            return;
        
        var skillInteractDescriptionPanel = InteractEvents.SkillInteractDescriptionPanelEvent;
        skillInteractDescriptionPanel.isPanelActive = true;
        skillInteractDescriptionPanel.position = _dropItem.transform.position;
        skillInteractDescriptionPanel.itemDataSo = _dropItem.itemData;
        
        _interactEventChannelSO.RaiseEvent(skillInteractDescriptionPanel);
    }

    public override void DisableDescription()
    {
        var skillInteractDescriptionPanel = InteractEvents.SkillInteractDescriptionPanelEvent;
        skillInteractDescriptionPanel.isPanelActive = false;
        
        _interactEventChannelSO.RaiseEvent(skillInteractDescriptionPanel);
    }
}
