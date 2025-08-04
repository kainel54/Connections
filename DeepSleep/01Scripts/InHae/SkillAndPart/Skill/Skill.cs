using System;
using System.Collections.Generic;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;
using YH.Players;

public enum PlayerSkillPointEnum
{
    Player, Sword, SwordEnd, Hip
}

public delegate void CooldownInfo(float current, float total);
public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected SkillObj _popSkillObj;
    
    [SerializeField] private PlayerSkillPointEnum _playerSkillPointEnum;
    [SerializeField] private PlayerManagerSO _playerManager;
    [HideInInspector] public Player player;
    
    [SerializeField] private GameEventChannelSO _soundEventChannel;
    [SerializeField] private SoundSO _soundSo;
    
    [SerializeField] private List<SkillFieldDataSO> _skillFieldDataSO = new();
    [SerializeField] private List<SkillStatInfoSO> _defaultUseSkillStats = new();
    
    public HashSet<SkillStatInfoSO> currentUseSkillStats = new();
    private Transform _partParent;
    
    private Dictionary<SkillFieldDataType, SkillFieldDataSO> _skillDataDictionary = new();
    private Dictionary<Type, SkillPart> _skillPartDictionary = new();

    public event Action SkillInputAction;
    public event Action PressAction;
    public event Action<Skill> UseSkillAction;
    
    public event CooldownInfo CooldownEvent;
    public event Action<int> skillCountAction;

    private int _shootCount = 0;
    private int _firedCount = 0;
    
    private float _cooldownTimer;
    public bool IsSkillCoolTime => _cooldownTimer > 0f;
    public bool CanShootSkill => _firedCount > 0;
    
    public LayerMask whatIsEnemy, whatIsGround;
    
    public SkillAnimation SkillAnimation {get; private set;}
    protected PlayerSkillPointManager _playerSkillPointManager;

    protected virtual void Awake()
    {
        _partParent = transform.Find("AbleParts");

        SkillAnimation = GetComponent<SkillAnimation>();

        PlayInitCheck();

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

        DataInit();
    }

    protected virtual void OnDestroy()
    {
        _playerManager.SetUpPlayerEvent -= HandleSetupPlayer;
    }

    private void HandleSetupPlayer()
    {
        player = _playerManager.Player;
        _playerSkillPointManager = player.GetCompo<PlayerSkillPointManager>();
        SkillAnimation.Init(this);
    }

    private void PlayInitCheck()
    {
        player = _playerManager.Player;

        if (player != null)
        {
            _playerSkillPointManager = player.GetCompo<PlayerSkillPointManager>();
            SkillAnimation.Init(this);
        }
        else
            _playerManager.SetUpPlayerEvent += HandleSetupPlayer;
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

    //스킬 키를 눌렀을 때 호출
    public void InputSkillProcess()
    {
        SkillInputAction?.Invoke();
    }
    
    //스킬 발동 전에 처리 (애니메이션 중 호출)
    public void UseBeforeProcess()
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

        PressAction?.Invoke();

        if (_shootCount > 0)
            return;
        
        //기본적인 스킬 사용
        UseSkill(_playerSkillPointManager.GetTransform(_playerSkillPointEnum));
    }

    //진짜 스킬 발동
    public virtual void UseSkill(Transform fireTrm)
    {
        UseSkillAction?.Invoke(this);
        Debug.Log("Use Skill");
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

    public void  DataInit()
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
