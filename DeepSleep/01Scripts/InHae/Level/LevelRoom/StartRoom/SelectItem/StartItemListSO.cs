using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartItemListSO", menuName = "SO/StartItemListSO")]
public class StartItemListSO : ScriptableObject
{
    public List<SkillItemSO> skillItems = new List<SkillItemSO>();
    public SkillItemSO GetRandomSkillItem() => skillItems[Random.Range(0, skillItems.Count)];
}
