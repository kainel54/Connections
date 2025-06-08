using System.Collections.Generic;
using YH.StatSystem;

public class AttackSpeedUpNodeAbility : BaseNodeAbility, IAttackSpeedUpNodeAbility
{
    private HashSet<int> _hashSet = new HashSet<int>();
    
    public override void ApplyAbility(int selfIdx,int targetIdx, Skill skill)
    {
        AttackSpeedUp(selfIdx, targetIdx, 0.3f);
        _hashSet.Add(targetIdx);
    }

    public override void UnApplyAbility(int selfIdx, int targetIdx, Skill skill)
    {
        base.UnApplyAbility(selfIdx, targetIdx, skill);
        AttackSpeedDown(selfIdx, targetIdx, 0.3f);
    }
    
    public void AttackSpeedUp(int selfIdx, int targetIdx, float value)
    {
        _player.GetCompo<EntityStat>().GetElement("AttackSpeed")
            .AddModify($"{GetType().Name}_{selfIdx}_{targetIdx}", value, EModifyMode.Add);
    }

    public void AttackSpeedDown(int selfIdx, int targetIdx, float value)
    {
        _player.GetCompo<EntityStat>().GetElement("AttackSpeed")
            .RemoveModify($"{GetType().Name}_{selfIdx}_{targetIdx}", EModifyMode.Add);
    }

    public override void InitAbility(int selfIdx, Skill skill)
    {
        base.InitAbility(selfIdx, skill);
        
        foreach (var targetIdx in _hashSet)
        {
            _player.GetCompo<EntityStat>().GetElement("AttackSpeed")
                .RemoveModify($"{GetType().Name}_{selfIdx}_{targetIdx}", EModifyMode.Add);
        }
    }
}
