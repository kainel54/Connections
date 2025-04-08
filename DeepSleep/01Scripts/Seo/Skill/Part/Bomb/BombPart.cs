using UnityEngine;

public class BombPart : SkillPart, IBombPart
{
    [SerializeField] private BombPartSkillObject _bombPartSkillObject;

    public void BombEquip()
    {
        _skill.shootCount++;

        if (_skill.shootCount > 1)
        {
            return;
        }

        _skill.PressAction += HandleBomb;
    }

    public void BombUnEquip()
    {
        _skill.shootCount--;
        if (_skill.shootCount >= 1)
        {
            return;
        }
        _skill.PressAction -= HandleBomb;
    }

    private void HandleBomb()
    {
        _skill.SetCoolTime();
        BombPartSkillObject bombObj = Instantiate(_bombPartSkillObject, _skill.player.transform.position, _skill.player.transform.rotation);
        bombObj.InitializeSkill(_skill);
    }

    public override void InitSetting()
    {
        for (int i = 0; i < _skill.shootCount; i++)
            _skill.PressAction -= HandleBomb;
        
        _skill.shootCount = 0;
        _skill.firedCount = 0;
    }
}