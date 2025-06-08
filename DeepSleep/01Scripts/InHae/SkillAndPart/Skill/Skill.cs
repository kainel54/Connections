using System;
using System.Collections.Generic;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public delegate void CooldownInfo(float current, float total);
public abstract class Skill : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManager;
    [HideInInspector] public Player player;
    
    [SerializeField] private GameEventChannelSO _soundEventChannel;
    [SerializeField] private SoundSO _soundSo;
    
    [SerializeField] private Transform _partParent;
    [SerializeField] private List<SkillFieldDataSO> _skillFieldDataSO = new();

    [SerializeField] private List<SkillStatInfoSO> _defaultUseSkillStats = new();
    public HashSet<SkillStatInfoSO> currentUseSkillStats = new();

    private Dictionary<SkillFieldDataType, SkillFieldDataSO> _skillDataDictionary = new();
    private Dictionary<Type, SkillPart> _skillPartDictionary = new();

    public event Action PressAction;
    public event CooldownInfo CooldownEvent;
    public event Action<Skill> UseSkillAction;
    public event Action<int> skillCountAction;

    private int _shootCount = 0;
    private int _firedCount = 0;
    
    private float _cooldownTimer;
    public bool isCoolTime => _cooldownTimer > 0;
    
    public LayerMask whatIsEnemy, whatIsGround;

    private void Awake()
    {
        _playerManager.SetUpPlayerEvent += HandleSetupPlayer;

        for (int i = 0; i < _skillFieldDataSO.Count; i++)
        {
            _skillFieldDataSO[i] = Instantiate(_skillFieldDataSO[i]);
            _skillFieldDataSO[i].Init();
        }
        
        _skillFieldDataSO.ForEach(x => _skillDataDictionary.Add(x.fieldType, x));
        
        foreach (var skillPart in _partParent.GetComponentsInChildren<SkillPart>())
            _skillPartDictionary.Add(skillPart.GetType(), skillPart);
        
        foreach (var useSkillStatInfoSo in _defaultUseSkillStats)
            currentUseSkillStats.Add(useSkillStatInfoSo);
    }

    private void OnDestroy()
    {
        _playerManager.SetUpPlayerEvent -= HandleSetupPlayer;
    }

    private void HandleSetupPlayer()
    {
        player = _playerManager.Player;
    }

    protected virtual void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;

                if (_shootCount > 1 && _shootCount > _firedCount)
                {
                    _firedCount++;
                    if (_shootCount != _firedCount)
                        SetCoolTime();
                }
                
                skillCountAction?.Invoke(_firedCount);
            }

            CooldownEvent?.Invoke(_cooldownTimer,
                ((GenericSkillDataSO)_skillDataDictionary[SkillFieldDataType.Generic]).coolTimeStat.currentValue); // 토탈에 비례해서 쿨타임 돌려줄 ui 일것이다.
        }
    }

    public void PressSkill()
    {
        if(_shootCount == 1)
            skillCountAction?.Invoke(0);
        
        if (_shootCount > 1 && _firedCount > 0)
        {
            _firedCount--;
            skillCountAction?.Invoke(_firedCount);
            PressAction?.Invoke();
            return;
        }

        if (_cooldownTimer > 0)
            return;
        PressAction?.Invoke();

        if (_shootCount > 0)
            return;
        UseSkill(player.transform);
    }

    public virtual void UseSkill(Transform fireTrm)
    {
        UseSkillAction?.Invoke(this);
    }

    public void SetCoolTime()
    {
        if (_cooldownTimer <= 0)
            _cooldownTimer = ((GenericSkillDataSO)_skillDataDictionary[SkillFieldDataType.Generic])
                .coolTimeStat.currentValue;
    }

    public void AddShootCount(int count)
    {
        _shootCount += count;
        _firedCount += count;
        skillCountAction?.Invoke(_shootCount);
    }

    public int GetShootCount() => _shootCount;

    public void CountInit()
    {
        _shootCount = 0;
        _firedCount = 0;   
        skillCountAction?.Invoke(_shootCount);
    }

    public SkillFieldDataSO GetSkillData(SkillFieldDataType fieldType) => _skillDataDictionary[fieldType];
    public SkillPart GetSkillPart(Type type) => _skillPartDictionary[type];

    public void DataInit()
    {
        foreach (SkillFieldDataSO dataSo in _skillFieldDataSO)
        {
            dataSo.ValueInit();
        }
    }

    public void UseActionClear() => UseSkillAction = null;

    public void PlaySound()
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.position = transform.position;
        soundEvt.clipData = _soundSo;
        _soundEventChannel.RaiseEvent(soundEvt);
    }
}
