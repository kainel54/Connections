using UnityEngine;
using YH.Players;
using YH.StatSystem;

public abstract class BaseNodeAbility : MonoBehaviour
{
    protected Skill _skill;
    protected Player _player;
    protected EntityStat _entityStat;

    public virtual void ApplyAbility(int selfIdx,int nodeIdx, Skill skill)
    {
    }

    public virtual void UnApplyAbility(int selfIdx, int targetIdx, Skill skill)
    {
        Init(skill);
    }

    protected virtual void Init(Skill currentSkill)
    {
        _skill = currentSkill;
        _player = _skill.player;
        Debug.Log(_player == null);
        _entityStat = _player.GetCompo<EntityStat>();
    }
    
    public virtual void InitAbility(int selfIdx, Skill skill)
    {
        
    }
}
