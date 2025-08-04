using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StartItemListSO", menuName = "SO/StartItemListSO")]
public class StartItemListSO : ScriptableObject
{
    public List<SkillItemSO> skillItems = new List<SkillItemSO>();
    public SkillItemSO GetRandomSkillItem() => skillItems[Random.Range(0, skillItems.Count)];

    public List<SkillItemSO> GetRandomNoDuplicationSkillItems(int count)
    {
        if (count > skillItems.Count)
            count = skillItems.Count;

        List<SkillItemSO> tempItems = new List<SkillItemSO>(skillItems);
        List<SkillItemSO> resultItems = new List<SkillItemSO>();
        
        for (int i = 0; i < count; i++)
        {
            int randIdx = Random.Range(0, tempItems.Count - i);
            
            SkillItemSO selectSkillItem = tempItems[randIdx];
            SkillItemSO lastSkillItem = tempItems[skillItems.Count - 1 - i];

            tempItems[randIdx] = lastSkillItem;
            tempItems[tempItems.Count - 1 - i] = selectSkillItem;
            
            resultItems.Add(selectSkillItem);
        }
        
        return resultItems;
    }
}
