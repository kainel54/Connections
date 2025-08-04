using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileSkillDataSO", menuName = "SO/SkillData/ProjectileSkillDataSO")]
public class ProjectileSkillDataSO : SkillFieldDataSO
{
    [Header("Show Icon")]
    public DefaultSkillStatElement projectileCountStat;
    public DefaultSkillStatElement projectileMoveSpeedStat;
    public DefaultSkillStatElement projectilePenetrationCountStat;
    public DefaultSkillStatElement projectileReflectionCountStat;
    
    [Header("No Icon")]
    public TrajectorySkillStatElement projectileTrajectoryStat;

    public bool canBeHit;

    public override void ValueInit()
    {
        base.ValueInit();
        canBeHit = false;
    }
}
