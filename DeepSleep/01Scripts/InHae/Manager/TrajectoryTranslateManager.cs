using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct TrajectoryTranslate
{
    public List<TrajectoryType> trajectoryCheckList;
    public string text;
}

public class TrajectoryTranslateManager : MonoSingleton<TrajectoryTranslateManager>
{
    [SerializeField] private List<TrajectoryTranslate> _trajectoryCheckList;

    public string TrajectoryTranslate(List<TrajectoryType> trajectoryCheckList)
    {
        foreach (var checkList in _trajectoryCheckList)
        {
            bool equal = checkList.trajectoryCheckList.OrderBy(x => x).
                SequenceEqual(trajectoryCheckList.OrderBy(x => x));

            if (equal)
            {
                return checkList.text;
            }
        }

        return "직선";
    }
        
}
