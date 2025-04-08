using System;
using System.Collections.Generic;
using UnityEngine;
using YH.Players;

public delegate void CooldownInfo(float current, float total);
public abstract class Skill : MonoBehaviour
{
    [SerializeField] List<PartItemSO> debugList = new();
    [SerializeField] private PlayerManagerSO _playerManager;
    [SerializeField] private Transform _partParent;
    [SerializeField] private List<SkillFieldDataSO> _skillFieldDataSO = new();

    private Dictionary<SkillFieldDataType, SkillFieldDataSO> _skillDataDictionary = new();
    private Dictionary<Type, SkillPart> _skillPartDictionary = new();

    public event Action PressAction;
    public event CooldownInfo CooldownEvent;
    public event Action<Skill> UseSkillAction;

    [HideInInspector] public Player player;

    [HideInInspector] public int shootCount = 0;
    [HideInInspector] public int firedCount = 0;
    public bool isCoolTime => _cooldownTimer > 0;


    private float _cooldownTimer;
    public LayerMask whatIsEnemy, whatIsGround;

    private void Awake()
    {
        for (int i = 0; i < _skillFieldDataSO.Count; i++)
        {
            _skillFieldDataSO[i] = Instantiate(_skillFieldDataSO[i]);
            _skillFieldDataSO[i].SetDefaultValues();
        }

        _playerManager.SetUpPlayerEvent += HandleSetupPlayer;

        _skillFieldDataSO.ForEach(x => _skillDataDictionary.Add(x.fieldType, x));

        foreach (var skillPart in _partParent.GetComponentsInChildren<SkillPart>())
            _skillPartDictionary.Add(skillPart.GetType(), skillPart);
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
        Debug.Log(shootCount);

        if (Input.GetKeyDown(KeyCode.N))
        {
            foreach (var skill in debugList)
            {
                PartNode partNode1;
                Type t = Type.GetType(skill.nodeScriptName);
                partNode1 = Activator.CreateInstance(t) as PartNode;
                partNode1.partType = ((PartItemSO)skill).type;

                partNode1.EquipPart(this);
            }
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            foreach (var skill in debugList)
            {
                PartNode partNode1;
                Type t = Type.GetType(skill.nodeScriptName);
                partNode1 = Activator.CreateInstance(t) as PartNode;
                partNode1.partType = ((PartItemSO)skill).type;

                partNode1.UnEquipPart(this);
            }
        }

        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer <= 0)
            {
                _cooldownTimer = 0;

                if (shootCount > 1 && shootCount > firedCount)
                {
                    firedCount++;
                    if (shootCount != firedCount)
                        SetCoolTime();
                }
            }

            CooldownEvent?.Invoke(_cooldownTimer,
                ((GenericSkillDataSO)_skillDataDictionary[SkillFieldDataType.Generic]).coolTime); // 토탈에 비례해서 쿨타임 돌려줄 ui 일것이다.
        }
    }

    public void PressSkill()
    {
        if (shootCount > 1 && firedCount > 0)
        {
            firedCount--;
            PressAction?.Invoke();
            return;
        }

        if (_cooldownTimer > 0)
            return;

        PressAction?.Invoke();

        if (shootCount > 0)
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
            _cooldownTimer = ((GenericSkillDataSO)_skillDataDictionary[SkillFieldDataType.Generic]).coolTime;
    }

    public SkillFieldDataSO GetSkillData(SkillFieldDataType fieldType)
    {
        return _skillDataDictionary.GetValueOrDefault(fieldType);
    }

    public SkillPart GetSkillPart(Type type)
    {
        return _skillPartDictionary.GetValueOrDefault(type);
    }

    public void DataInit()
    {
        foreach (SkillFieldDataSO dataSo in _skillFieldDataSO)
        {
            dataSo.Init();
        }
    }
}
