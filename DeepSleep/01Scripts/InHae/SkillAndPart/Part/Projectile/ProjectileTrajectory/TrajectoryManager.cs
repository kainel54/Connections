using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TrajectoryManager : MonoBehaviour
{
    private List<BaseTrajectory> _trajectories = new List<BaseTrajectory>();
    private Dictionary<Type, BaseTrajectory> _trajectoryDictionary = new();
    
    private void Awake()
    {
        _trajectories = GetComponentsInChildren<BaseTrajectory>().ToList();
    }

    public BaseTrajectory GetTrajectory(List<TrajectoryType> trajectoryTypes)
    {
        foreach (BaseTrajectory trajectory in _trajectories)
        {
            bool equal = trajectoryTypes.OrderBy(x => x).
                SequenceEqual(trajectory.trajectoryCheckList.OrderBy(x => x));

            if (equal)
            {
                return trajectory;
            }
        }

        return new StraightTrajectory();
    }
}
