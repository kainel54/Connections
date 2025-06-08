using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PartItemSO", menuName = "SO/Item/PartItemSO")]
public class PartItemSO : ItemDataSO, IComparable<PartItemSO>
{
    private void Awake()
    {
        inventoryType = InventoryType.Part;
    }

    public PartType type;
    // 노드 스크립트 이름을 복붙해줘야 합니다
    [FormerlySerializedAs("reflectionName")] public string nodeScriptName;
    public GameObject visual;

    // Chain Data Check 를 위한 정의
    public int CompareTo(PartItemSO other)
    {
        return string.Compare(nodeScriptName, other.nodeScriptName, StringComparison.Ordinal);
    }
}
