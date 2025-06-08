using UnityEngine;

[CreateAssetMenu(fileName = "GenericSkillDataSO", menuName = "SO/SkillData/GenericSkillDataSO")]
public class GenericSkillDataSO : SkillFieldDataSO
{
    public SkillAttackType attackType;

    public DefaultSkillStatElement attackDamageStat;
    public DefaultSkillStatElement attackCountStat;
    public DefaultSkillStatElement coolTimeStat;
    public DefaultSkillStatElement reShootTimeStat;
    public DefaultSkillStatElement criticalChanceStat;
    public DefaultSkillStatElement skillActiveDurationStat;
    
    public float skillDamageDelay;
    public float skillActiveDelay;     //this is for applying damage with an animation or particle delay on time.
}
