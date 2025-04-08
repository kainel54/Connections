using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemPartsSO", menuName = "SO/Skill/PartsSO")]
public class PartItemSO : ItemDataSO, IComparable<PartItemSO>
{
    public PartType type;
    // 노드 스크립트 이름을 복붙해줘야 합니다
    [FormerlySerializedAs("reflectionName")] public string nodeScriptName;
    public GameObject visual;

    public int CompareTo(PartItemSO other)
    {
        return string.Compare(nodeScriptName, other.nodeScriptName, StringComparison.Ordinal);
    }
}
