using System;
using UnityEngine;
using YH.Players;

public class AttackStep : TutorialStep
{
    private bool _isAttacked;

    public override void OnEnter()
    {
        base.OnEnter();
        _player.AttackEvent += HandleAttack;
    }

    private void HandleAttack()
    {
        _isAttacked = true;
        if (_isAttacked)
        {
            _player.AttackEvent -= HandleAttack;
        }
    }
}
