using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicSkillSO", menuName = "SO/Skill/BasicSkillSO")]
public class SkillItemSO : ItemDataSO
{
    //처음에 가지고 있는 노드 칸 수
    public List<int> defaultNodeCount;
    public string reflectionName;
    public GameObject visual;
}
