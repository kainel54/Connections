using UnityEngine;

public enum SkillType
{
    Normal,
    Position,
    Combo,
    Chain,
    Charge,
    Hold,
}

[CreateAssetMenu(fileName = "GenericSkillDataSO", menuName = "SO/SkillData/GenericSkillDataSO")]
public class GenericSkillDataSO : SkillFieldDataSO
{
    public SkillAttackType attackType;
    private SkillAttackType _defaultAttackType;
    
    public SkillType skillType;
    private SkillType _defaultSkillType;
    
    [Header("Show Icon")]
    public DefaultSkillStatElement attackDamageStat;
    public DefaultSkillStatElement coolTimeStat;
    public DefaultSkillStatElement skillActiveDurationStat;
    public DefaultSkillStatElement criticalChanceStat;
    public DefaultSkillStatElement criticalDamageStat;

    [Header("No Icon")]
    public DefaultSkillStatElement attackCountStat;
    public DefaultSkillStatElement reShootTimeStat;

    public float skillDamageDelay;
    public float skillActiveDelay;

    public bool canLightning = false; // 여기에 있으면 안되는 것들임. 나중에 어딘가로 옮겨질거
    public int lightningCount = 0;    // 여기에 있으면 안되는 것들임. 나중에 어딘가로 옮겨질거

    public override void Init()
    {
        base.Init();
        _defaultAttackType = attackType;
        _defaultSkillType = skillType;
    }

    public override void ValueInit()
    {
        base.ValueInit();
        skillType = _defaultSkillType;
        attackType = _defaultAttackType;
    }
}
