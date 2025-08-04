using System;
using UnityEngine;
using YH.Players;

public class DashStep : TutorialStep
{
    private bool _isDashed;

    public override void OnEnter()
    {
        base.OnEnter();
        _player.DashEvent += HandleDash;
    }

    private void HandleDash()
    {
        _isDashed = true;
        if (_isDashed)
        {
            _player.DashEvent -= HandleDash;
        }
    }
}
