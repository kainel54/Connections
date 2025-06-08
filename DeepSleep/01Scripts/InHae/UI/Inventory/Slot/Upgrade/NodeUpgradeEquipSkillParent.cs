using System.Collections.Generic;
using UnityEngine;
using YH.EventSystem;

public abstract class NodeUpgradeEquipSkillParent : MonoBehaviour
{
    [Header("Default Channel or Special Channel")]
    [SerializeField] protected GameEventChannelSO _upgradeEventChannel;

    protected Dictionary<int, BaseNodeUpgradeEquipSkillSlotUI> _equipSlotList = new();

    protected virtual void Awake()
    {
        foreach (var skillEquip in GetComponentsInChildren<BaseNodeUpgradeEquipSkillSlotUI>())
        {
            _equipSlotList.Add(skillEquip.index, skillEquip);
        }
    }
}
