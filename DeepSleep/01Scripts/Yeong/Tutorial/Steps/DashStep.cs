using System;
using UnityEngine;
using YH.Players;

public class DashStep : TutorialStep
{
    private bool _isDashed;
    private Player _player;
    private PlayerMovement _playerMover;
    [SerializeField] private PlayerManagerSO _playermanager;

    public override void OnEnter()
    {
        base.OnEnter();
        _player = _playermanager.Player;
        _playerMover = _player.GetCompo<PlayerMovement>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (_isDashed)
        {
            _tutorialManager.NextStep();
        }
        if (!_isDashed)
        {
            _isDashed = _playerMover.IsDash;
        }
    }
}
