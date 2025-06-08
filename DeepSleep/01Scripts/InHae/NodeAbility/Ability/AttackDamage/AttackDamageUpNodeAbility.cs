using System.Collections.Generic;
using YH.StatSystem;

public class AttackDamageUpNodeAbility : BaseNodeAbility, IAttackDamageUpNodeAbility
{
    private HashSet<int> _hashSet = new HashSet<int>();
    
    public override void ApplyAbility(int selfIdx,int targetIdx, Skill skill)
    {
        AttackDamageUp(selfIdx, targetIdx, 3);
        _hashSet.Add(targetIdx);
    }

    public override void UnApplyAbility(int selfIdx, int targetIdx, Skill skill)
    {
        base.UnApplyAbility(selfIdx, targetIdx, skill);
        AttackDamageDown(selfIdx, targetIdx, 3);
    }
    
    public void AttackDamageUp(int selfIdx, int targetIdx, float value)
    {
        _player.GetCompo<EntityStat>().GetElement("AttackPower")
            .AddModify($"{GetType().Name}_{selfIdx}_{targetIdx}", value, EModifyMode.Add);
    }

    public void AttackDamageDown(int selfIdx, int targetIdx, float value)
    {
        _player.GetCompo<EntityStat>().GetElement("AttackPower")
            .RemoveModify($"{GetType().Name}_{selfIdx}_{targetIdx}", EModifyMode.Add);
    }

    public override void InitAbility(int selfIdx, Skill skill)
    {
        base.InitAbility(selfIdx, skill);
        
        foreach (var targetIdx in _hashSet)
        {
            _player.GetCompo<EntityStat>().GetElement("AttackPower")
                .RemoveModify($"{GetType().Name}_{selfIdx}_{targetIdx}", EModifyMode.Add);
        }
    }
}
