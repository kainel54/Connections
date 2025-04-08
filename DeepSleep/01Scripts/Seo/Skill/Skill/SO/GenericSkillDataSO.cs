using UnityEngine;

[CreateAssetMenu(fileName = "GenericSkillDataSO", menuName = "SO/SkillData/GenericSkillDataSO")]
public class GenericSkillDataSO : SkillFieldDataSO
{
    public float damage;
    private float Damage;
    
    public int attackCount;
    private int AttackCount;
    
    public float coolTime;
    private float CoolTime;
    
    public float skillDamageDelay;
    private float SkillDamageDelay;
    
    public float skillActiveDelay;     //this is for applying damage with an animation or particle delay on time.
    private float SkillActiveDelay;
    
    public float skillActiveDuration;   // this is for duratino buff or somthing like that
    private float SkillActiveDuration;
    
    public float reShootTime;       // this is for re shoot coroutine delay
    private float ReShootTime;

    public override void SetDefaultValues()
    {
        Damage = damage;
        AttackCount = attackCount;
        CoolTime = coolTime;
        SkillDamageDelay = skillDamageDelay;
        SkillActiveDelay = skillActiveDelay;
        SkillActiveDuration = skillActiveDuration;
        ReShootTime = reShootTime;
    }

    public override void Init()
    {
        damage = Damage;
        attackCount = AttackCount;
        coolTime = CoolTime;
        skillDamageDelay = SkillDamageDelay;
        skillActiveDelay = SkillActiveDelay;
        skillActiveDuration = SkillActiveDuration;
        reShootTime = ReShootTime;
    }
}
