using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillStatTrajectorySlot : SkillStatBaseSlot
{
    public override void Init(BaseSkillStatElement baseSkillStatElement)
    {
        base.Init(baseSkillStatElement);
        
        TrajectorySkillStatElement trajectorySkillStatElement = baseSkillStatElement as TrajectorySkillStatElement;
        _valueText.text = TrajectoryTranslateManager.Instance.TrajectoryTranslate(trajectorySkillStatElement.currentTrajectory);
    }
}
