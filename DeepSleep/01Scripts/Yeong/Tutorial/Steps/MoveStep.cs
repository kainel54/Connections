using System;
using UnityEngine;
using YH.Players;

public class MoveStep : TutorialStep
{
    private bool _isMoved;
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

        if (_isMoved)
        {
            _tutorialManager.NextStep();
        }
        if (!_isMoved)
        {
            _isMoved = Mathf.Abs(_playerMover.Movement.x) > 0.5f || Mathf.Abs(_playerMover.Movement.z) > 0.5f;
        }
    }
}
