using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BossCoolTimeManager : MonoBehaviour
{
    private Dictionary<string, float> _skillCoolTimeItems = new Dictionary<string, float>();

    private void Update()
    {
        foreach (var key in _skillCoolTimeItems.Keys.ToList())
        {
            _skillCoolTimeItems[key] -= Time.deltaTime;
            _skillCoolTimeItems[key] = Mathf.Max(0, _skillCoolTimeItems[key]);
        }
    }

    public bool CanUseSkill(string skillName) => _skillCoolTimeItems[skillName] <= 0;
    public void SetCoolTime(string skillName, float time) => _skillCoolTimeItems[skillName] = time;

}
