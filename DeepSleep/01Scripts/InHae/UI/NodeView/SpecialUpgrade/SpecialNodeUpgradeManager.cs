using System;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using TMPro;
using UnityEngine;
using YH.EventSystem;

public class SpecialNodeUpgradeManager : MonoBehaviour
{
   [SerializeField] private GameEventChannelSO _uiEventChannel;
   public GameEventChannelSO specialNodeUpgradeEventChannel;
    
    private TextMeshProUGUI _description;
    private CanvasGroup _canvasGroup;
    
    private Dictionary<Type, ISpecialNodeUpgradeCompo> _components = new Dictionary<Type, ISpecialNodeUpgradeCompo>();
    
    private SpecialNodeUpgradeView _specialNodeUpgradeView;
    private SpecialNodeUpgradeProcessor _specialNodeUpgradeProcessor;
    
    private void Awake()
    {
        _description = transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;

        GetComponents<ISpecialNodeUpgradeCompo>().ToList().ForEach(x =>
        {
            _components.Add(x.GetType(), x);
            x.Initialize(this);
        });
        
        _specialNodeUpgradeView = GetCompo<SpecialNodeUpgradeView>();
        _specialNodeUpgradeProcessor = GetCompo<SpecialNodeUpgradeProcessor>();
    }
    
    private void Start()
    {
        _specialNodeUpgradeView.UpgradeEndAction += HandleUpgradeEndCheck;
    }
    
    private void OnDestroy()
    {
        _specialNodeUpgradeView.UpgradeEndAction -= HandleUpgradeEndCheck;
    }
    
    private void HandleUpgradeEndCheck()
    {
        Close();
    }
    
    public void Open()
    {
        var evt = UIPanelEvent.WindowPanelLockEvent;
        evt.isOpenLocked = true;
        _uiEventChannel.RaiseEvent(evt);

        _specialNodeUpgradeProcessor.SetCanUpgradeEnd(true);
        GetCompo<SpecialNodeUpgradeView>().CreateNodes();

        _description.text = "강화할 노드를 선택해주세요.\n(특별 노드 제외)";
        
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    private void Close()
    {
        var lockEvent = UIPanelEvent.WindowPanelLockEvent;
        lockEvent.isOpenLocked = false;
        _uiEventChannel.RaiseEvent(lockEvent);
        
        var reLoading = SpecialNodeUpgradeEvents.UpgradeSkillReLoadEvent;
        specialNodeUpgradeEventChannel.RaiseEvent(reLoading);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;    
    }
    
    public T GetCompo<T>() where T : ISpecialNodeUpgradeCompo
    {
        if (_components.TryGetValue(typeof(T), out ISpecialNodeUpgradeCompo component))
            return (T)component;
        
        return default;
    }
}