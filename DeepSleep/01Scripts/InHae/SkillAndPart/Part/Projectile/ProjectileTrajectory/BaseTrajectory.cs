using System;
using System.Collections.Generic;
using UnityEngine;
using YH.Players;

public enum TrajectoryType
{
    ZigZag,
    Circle,
}

public abstract class BaseTrajectory : MonoBehaviour
{
    public List<TrajectoryType> trajectoryCheckList = new List<TrajectoryType>();
    
    protected SkillProjectileObj skillProjectileObj;
    protected Player _player => skillProjectileObj.skill.player;
    protected Vector3 _shootDir;

    public virtual void Init(SkillProjectileObj skillProjectileObj)
    {
        this.skillProjectileObj = skillProjectileObj;
        _shootDir = skillProjectileObj.transform.forward;
        _shootDir.y = 0;
    }

    public void ShootDirInit(Vector3 direction)
    {
        _shootDir = direction.normalized;
        _shootDir.y = 0;
    }

    public abstract Vector3 UpdateTrajectory();
}
