using System;
using UnityEngine;

public class SkillRangeObj : SkillObj
{
    protected int shootCount = 0;
    
    public void RangeInit(int shootcnt)
    {
        shootCount = shootcnt;
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
