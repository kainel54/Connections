using System;
using UnityEngine;
using YH.Combat;

public class SkillObj : MonoBehaviour
{
    [HideInInspector] public Skill skill;
    public event Action OnSkillDestroyEvent;

    protected float _damage;
    protected float _criticalChance;
    protected float _criticalDamage = 100f;

    public void CallDestroyEvent()
    {
        OnSkillDestroyEvent?.Invoke();
    }
    
    public virtual void Initialize(Skill _skill, Transform shootTrm)
    {
        skill = _skill;
        
        GenericSkillDataSO genericSkillDataSo = skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;
        
        _damage = genericSkillDataSo.attackDamageStat.currentValue;
        _criticalChance = genericSkillDataSo.criticalChanceStat.currentValue;
        _criticalDamage = 125f;
    }
    
    protected HitData GetHitData() => new HitData(skill.player, _damage, _criticalChance, _criticalDamage);
}
