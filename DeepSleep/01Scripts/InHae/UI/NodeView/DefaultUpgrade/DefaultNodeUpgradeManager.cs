using System;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.NodeEvent.DefaultNodeUpgradeEvent;
using IH.EventSystem.UIEvent.PanelEvent;
using TMPro;
using UnityEngine;
using YH.EventSystem;

public class DefaultNodeUpgradeManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannel;
    public GameEventChannelSO defaultNodeEventChannel;
    
    private TextMeshProUGUI _description;
    private CanvasGroup _canvasGroup;
    
    private int _upgradeCount;
    private Dictionary<Type, IDefaultNodeUpgradeCompo> _components = new Dictionary<Type, IDefaultNodeUpgradeCompo>();
    
    private DefaultNodeUpgradeView _defaultNodeUpgradeView;
    private DefaultNodeUpgradeProcessor _defaultNodeUpgradeProcessor;
    
    private void Awake()
    {
        _description = transform.Find("DescriptionText").GetComponent<TextMeshProUGUI>();

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;

        GetComponents<IDefaultNodeUpgradeCompo>().ToList().ForEach(x =>
        {
            _components.Add(x.GetType(), x);
            x.Initialize(this);
        });

        _defaultNodeUpgradeView = GetCompo<DefaultNodeUpgradeView>();
        _defaultNodeUpgradeProcessor = GetCompo<DefaultNodeUpgradeProcessor>();
    }

    private void Start()
    {
        defaultNodeEventChannel.AddListener<UpgradeCountInitEvent>(HandleCountInit);
        
        _defaultNodeUpgradeProcessor.UpgradeAction += HandleUpgradeCountDown;
        _defaultNodeUpgradeView.UpgradeEndAction += HandleUpgradeEndCheck;
    }

    private void OnDestroy()
    {
        defaultNodeEventChannel.RemoveListener<UpgradeCountInitEvent>(HandleCountInit);
        
        _defaultNodeUpgradeProcessor.UpgradeAction -= HandleUpgradeCountDown;
        _defaultNodeUpgradeView.UpgradeEndAction -= HandleUpgradeEndCheck;
    }

    private void HandleUpgradeCountDown()
    {
        _upgradeCount--;
        _description.text = $"강화할 노드를 선택해주세요.\n(남은 횟수 : {_upgradeCount})";
        
        if (_upgradeCount > 0) 
            return;
        _defaultNodeUpgradeProcessor.SetCanUpgradeEnd(false);
    }
    
    private void HandleUpgradeEndCheck()
    {
        if (_upgradeCount > 0)
            return;
        Close();
    }
    
    private void HandleCountInit(UpgradeCountInitEvent evt) => _upgradeCount = evt.count;

    public void Open()
    {
        var evt = UIPanelEvent.WindowPanelLockEvent;
        evt.isOpenLocked = true;
        _uiEventChannel.RaiseEvent(evt);
        
        _defaultNodeUpgradeProcessor.SetCanUpgradeEnd(true);
        GetCompo<DefaultNodeUpgradeView>().CreateNodes();

        _description.text = $"강화할 노드를 선택해주세요.\n(남은 횟수 : {_upgradeCount})";
        
        _canvasGroup.alpha = 1;
        _canvasGroup.blocksRaycasts = true;
    }

    private void Close()
    {
        var lockEvent = UIPanelEvent.WindowPanelLockEvent;
        lockEvent.isOpenLocked = false;
        _uiEventChannel.RaiseEvent(lockEvent);
        
        var evt = DefaultNodeUpgradeEvents.UpgradeSkillReLoadEvent;
        defaultNodeEventChannel.RaiseEvent(evt);
        
        _canvasGroup.alpha = 0;
        _canvasGroup.blocksRaycasts = false;    
    }
    
    public T GetCompo<T>() where T : IDefaultNodeUpgradeCompo
    {
        if (_components.TryGetValue(typeof(T), out IDefaultNodeUpgradeCompo component))
            return (T)component;
        
        return default;
    }
}
