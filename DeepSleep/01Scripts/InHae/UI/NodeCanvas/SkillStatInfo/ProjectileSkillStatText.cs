using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[Serializable]
public struct TrajectoryTranslate
{
    public List<TrajectoryType> trajectoryCheckList;
    public string text;
}

public class ProjectileSkillStatText : SkillStatTextUI
{
    [SerializeField] private List<TrajectoryTranslate> _trajectoryCheckList;
    
    [SerializeField] private TextMeshProUGUI _countText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private TextMeshProUGUI _trajectoryText;
    
    public override void Init(SkillFieldDataSO skillFieldData)
    {
        ProjectileSkillDataSO projectileSkillData = (ProjectileSkillDataSO)skillFieldData;
        
        _countText.SetText($"투사체 개수 : {projectileSkillData.skillObjCreateCount}개");
        _speedText.SetText($"투사체 속도 : {projectileSkillData.projMoveSpeed}");
        _trajectoryText.SetText($"궤적 : {TrajectoryTranslate(projectileSkillData.currentTrajectoryList)}");
    }

    private string TrajectoryTranslate(List<TrajectoryType> trajectoryCheckList)
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
