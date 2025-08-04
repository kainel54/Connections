using UnityEngine;
using YH.FSM;

public class SpinningSlashSkillAnimation : SkillAnimation
{
    [SerializeField] private float _yMoveDuration;
    [SerializeField] private float _xMoveDuration;

    private bool _verticalMoved;
    private bool _forwardMoved;
    
    private SpinningSlashSkill _spinningSlashSkill;

    public override void Init(Skill skill)
    {
        base.Init(skill);
        _spinningSlashSkill = skill as SpinningSlashSkill;
    }

    protected override void OnDestroy()
    {
        _playerAnimatorTrigger.OnMoveForwardTrigger -= HandleMoveTrigger;
        _playerAnimatorTrigger.OnMoveYTrigger -= HandleMoveYTrigger;

        _forwardMoved = false;
        _verticalMoved = false;
        
        base.OnDestroy();
    }

    protected override void AnimationPlay()
    {
        _skillActions[1] = _spinningSlashSkill.SlashAttack;
        
        _playerAnimatorTrigger.OnMoveForwardTrigger += HandleMoveTrigger;
        _playerAnimatorTrigger.OnMoveYTrigger += HandleMoveYTrigger;
        
        _forwardMoved = false;
        _verticalMoved = false;
        
        base.AnimationPlay();
    }

    protected override void SkillCompleteProcess()
    {
        _playerAnimatorTrigger.OnMoveForwardTrigger -= HandleMoveTrigger;
        _playerAnimatorTrigger.OnMoveYTrigger -= HandleMoveYTrigger;
        base.SkillCompleteProcess();
    }

    private void HandleMoveYTrigger(float yDirection)
    {
        if(_verticalMoved)
            return;

        _verticalMoved = true;
        _playerMover.ApplyVerticalOffset(yDirection, 0.5f);
    }

    private void HandleMoveTrigger(float fowardDirection)
    {
        if(_forwardMoved)
            return;
        
        _forwardMoved = true;
        _playerMover.MoveForward(fowardDirection, 0.5f);
    }
}
