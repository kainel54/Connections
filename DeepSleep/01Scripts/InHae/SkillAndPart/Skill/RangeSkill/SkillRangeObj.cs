using System;
using UnityEngine;

public class SkillRangeObj : SkillObj
{
    protected int _shootCount = 0;
    
    public void RangeInit(int shootCnt)
    {
        _shootCount = shootCnt;
    }
}
