using System.Collections.Generic;
using YH.StatSystem;

public class MoveSpeedUpNodeAbility : BaseNodeAbility, IMoveSpeedUpNodeAbility
{
    private HashSet<int> _hashSet = new HashSet<int>();
    
    public override void ApplyAbility(int selfIdx,int targetIdx, Skill skill)
    {
        MoveSpeedUp(selfIdx, targetIdx, 1f);
        _hashSet.Add(targetIdx);
    }

    public override void UnApplyAbility(int selfIdx, int targetIdx, Skill skill)
    {
        base.UnApplyAbility(selfIdx, targetIdx, skill);
        MoveSpeedDown(selfIdx, targetIdx, 1f);
    }

    public void MoveSpeedUp(int selfIdx, int targetIdx, float value)
    {
        _player.GetCompo<EntityStat>().GetElement("Speed")
            .AddModify($"{GetType().Name}_{selfIdx}_{targetIdx}", value, EModifyMode.Add);
    }

    public void MoveSpeedDown(int selfIdx, int targetIdx, float value)
    {
        _player.GetCompo<EntityStat>().GetElement("Speed")
            .RemoveModify($"{GetType().Name}_{selfIdx}_{targetIdx}", EModifyMode.Add);
    }
    public override void InitAbility(int selfIdx, Skill skill)
    {
        base.InitAbility(selfIdx, skill);
        
        foreach (var targetIdx in _hashSet)
        {
            _player.GetCompo<EntityStat>().GetElement("Speed")
                .RemoveModify($"{GetType().Name}_{selfIdx}_{targetIdx}", EModifyMode.Add);
        }
    }
}
