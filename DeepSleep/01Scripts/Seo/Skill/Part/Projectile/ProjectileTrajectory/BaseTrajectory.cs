using System;
using System.Collections.Generic;
using UnityEngine;

public enum TrajectoryType
{
    ZigZag,
    Circle,
}

public abstract class BaseTrajectory : MonoBehaviour
{
    protected SkillProjectileObj skillProjectileObj;
    public virtual void Init(SkillProjectileObj skillProjectileObj) => this.skillProjectileObj = skillProjectileObj;
    public List<TrajectoryType> trajectoryCheckList = new List<TrajectoryType>();
    public abstract Vector3 UpdateTrajectory();
}
