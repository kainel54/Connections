using System.Collections.Generic;
using YH.StatSystem;

public class MaxHealthUpNodeAbility : BaseNodeAbility, IMaxHealthUpNodeAbility
{
    private int _upValue = 5;
    
    private HashSet<int> _hashSet = new HashSet<int>();
    private EntityHealth _entityHealth;
    
    private float _defaultHealth;

    protected override void Init(Skill currentSkill)
    {
        base.Init(currentSkill);
        _hashSet.Clear();
        _entityHealth = _player.GetCompo<EntityHealth>();
    }

    public override void ApplyAbility(int selfIdx,int targetIdx, Skill skill)
    {
        MaxHealthUp(selfIdx, targetIdx, _upValue);
        _hashSet.Add(targetIdx);
    }

    public override void UnApplyAbility(int selfIdx, int targetIdx, Skill skill)
    {
        base.UnApplyAbility(selfIdx, targetIdx, skill);
        MaxHealthDown(selfIdx, targetIdx, _upValue);
    }

    public void MaxHealthUp(int selfIdx, int targetIdx, float value)
    {
        _entityStat.GetElement("Health")
            .AddModify($"{GetType().Name}_{selfIdx}_{targetIdx}", value, EModifyMode.Add);
        //_entityHealth.ApplyRecovery((int)value);
    }

    public void MaxHealthDown(int selfIdx, int targetIdx, float value)
    {
        // if(_hashSet.Contains(targetIdx))
        //     _entityHealth.ApplyRecovery((int)-value); 
        
        _entityStat.GetElement("Health")
            .RemoveModify($"{GetType().Name}_{selfIdx}_{targetIdx}", EModifyMode.Add);
    }

    public override void InitAbility(int selfIdx, Skill skill)
    {
        base.InitAbility(selfIdx, skill);

        foreach (var targetIdx in _hashSet)
        {
            _entityStat.GetElement("Health")
                .RemoveModify($"{GetType().Name}_{selfIdx}_{targetIdx}", EModifyMode.Add);
        }
    }
}
