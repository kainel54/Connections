using System;
using UnityEngine;

public class SkillRangeObj : SkillObj
{
    [HideInInspector] public Skill skill;
    protected int shootCount = 0;
    public void Initialize(Skill rangeSkill, int shootcnt)
    {
        skill = rangeSkill;
        shootCount = shootcnt;
    }

    public void DestroyObject(GameObject gameObject)
    {
        Debug.Log("DestroyObject");
        Destroy(gameObject);
    }

    public void DestroyObject(GameObject gameObject, float delay)
    {
        Debug.Log("DestroyObject");
        Destroy(gameObject, delay);
    }
}
