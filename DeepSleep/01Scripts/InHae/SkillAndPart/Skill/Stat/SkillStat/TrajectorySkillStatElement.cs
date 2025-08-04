using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TrajectorySkillStatElement : BaseSkillStatElement
{
    [SerializeField] private List<TrajectoryType> _defaultTrajectory;
    public List<TrajectoryType> currentTrajectory = new List<TrajectoryType>();

    public TrajectorySkillStatElement(DefaultSkillStatInfoSO defaultSkillStatInfo) : base(defaultSkillStatInfo)
    {
    }

    public List<TrajectoryType> DefaultTrajectory => _defaultTrajectory;
    
    public override void ValueInit()
    {
        currentTrajectory.Clear();
        foreach (var trajectory in _defaultTrajectory)
            currentTrajectory.Add(trajectory);
    }
}
