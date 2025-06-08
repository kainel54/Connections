using UnityEngine;

public class BombPart : SkillPart, IBombPart
{
    [SerializeField] private BombPartSkillObject _bombPartSkillObject;

    public void BombEquip()
    {
        _skill.AddShootCount(1);

        if (_skill.GetShootCount() > 1)
        {
            return;
        }

        AddUseSkillStat();
        _skill.PressAction += HandleBomb;
    }

    public void BombUnEquip()
    {
        _skill.AddShootCount(-1);
        if (_skill.GetShootCount() >= 1)
        {
            return;
        }
        
        RemoveUseSkillStat();
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
        for (int i = 0; i < _skill.GetShootCount(); i++)
            _skill.PressAction -= HandleBomb;
        
        _skill.CountInit();
    }
}