using System;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.UIEvent;
using UnityEngine;
using YH.EventSystem;

public class SkillHudUIManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _uiEventChannelSO;
    
    private List<PlayerSkillHud> _skillHuds = new List<PlayerSkillHud>();

    private void Awake()
    {
        _skillHuds = GetComponentsInChildren<PlayerSkillHud>().ToList();
        
        _uiEventChannelSO.AddListener<SkillHudEvent>(HandleChangeSkillHud);
    }
    
    private void OnDestroy()
    {
        _uiEventChannelSO.RemoveListener<SkillHudEvent>(HandleChangeSkillHud);
    }

    private void HandleChangeSkillHud(SkillHudEvent evt)
    {
        _skillHuds[evt.skillIdx].HandleChangeSkillHud(evt);
    }
}
