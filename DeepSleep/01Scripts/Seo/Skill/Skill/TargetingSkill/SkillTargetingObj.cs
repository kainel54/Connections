using System;
using UnityEngine;

public class SkillTargetingObj : SkillObj
{
    [HideInInspector] public Skill skill;
    public void Initialize(TargetingSkill targetngSkill)
    {
        skill = targetngSkill;
    }

    public void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public void DestroyObject(GameObject gameObject, float delay)
    {
        Destroy(gameObject, delay);
    }
}