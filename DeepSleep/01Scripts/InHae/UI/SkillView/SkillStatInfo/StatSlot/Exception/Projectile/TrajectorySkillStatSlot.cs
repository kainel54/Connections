using UnityEngine;

public class TrajectorySkillStatSlot : SkillStatBaseSlot
{
    public override void Init(BaseSkillStatElement baseSkillStatElement)
    {
        base.Init(baseSkillStatElement);
        TrajectorySkillStatElement trajectorySkillStatElement = baseSkillStatElement as TrajectorySkillStatElement;
        
        string currentTrajectory =
            EnumStringManager.Instance.GetString(trajectorySkillStatElement.currentTrajectory);
        string defaultTrajectory =
            EnumStringManager.Instance.GetString(trajectorySkillStatElement.DefaultTrajectory);

        if (defaultTrajectory == currentTrajectory)
        {
            _valueText.text = currentTrajectory;
            return;
        }
        
        _valueText.SetText($"{currentTrajectory}  ({defaultTrajectory} -> {currentTrajectory})");
    }
}
