using System;
using UnityEngine;
using YH.Players;

public class MoveStep : TutorialStep
{
    private bool _isMoved;

    public override void OnEnter()
    {
        base.OnEnter();
        _player.MoveEvent += HandleMove;
    }

    private void HandleMove()
    {
        _isMoved = true;
        if (_isMoved)
        {
            _player.MoveEvent -= HandleMove;
        }
    }
}
