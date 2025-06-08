using IH.EventSystem.UIEvent;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerSkillHud : MonoBehaviour
{
    [SerializeField] private Image _skillIcon;
    [SerializeField] private Image _skillCoolFadeImage;
    [SerializeField] private TextMeshProUGUI _skillCoolTimeText;
    [SerializeField] private TextMeshProUGUI _skillCountText;
    
    private Skill _currentSkill;

    private void OnDestroy()
    {
        if (_currentSkill != null)
        {
            _currentSkill.CooldownEvent -= HandleCoolDownCheck;
            _currentSkill.skillCountAction -= HandleCountCheck;
            
            _skillIcon.color = Color.black;
        }
    }

    public void HandleChangeSkillHud(SkillHudEvent evt)
    {
        Init();
        if (_currentSkill != null)
        {
            _currentSkill.CooldownEvent -= HandleCoolDownCheck;
            _currentSkill.skillCountAction -= HandleCountCheck;

            _skillIcon.color = Color.black;
        }

        if (evt.SkillItemData == null)
            return;
        
        _skillIcon.sprite = evt.SkillItemData.icon;
        _skillIcon.color = Color.white;
        
        _currentSkill = evt.skill;

        _currentSkill.CooldownEvent += HandleCoolDownCheck;
        _currentSkill.skillCountAction += HandleCountCheck;
    }
    
    private void Init()
    {
        _skillCoolFadeImage.fillAmount = 0;
        _skillCoolTimeText.SetText("");
        _skillCountText.SetText("");
    }
    
    private void HandleCountCheck(int count)
    {
        if (count == 0)
        {
            _skillCountText.SetText("");
            return;
        }

        _skillCountText.SetText(count.ToString());
    }

    private void HandleCoolDownCheck(float current, float total)
    {
        _skillCoolFadeImage.fillAmount = current / total;
        _skillCoolTimeText.SetText(current <= 0 ? "" : current.ToString("F1"));
    }
}
