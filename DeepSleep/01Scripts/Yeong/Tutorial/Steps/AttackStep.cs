using System;
using UnityEngine;
using YH.Players;

public class AttackStep : TutorialStep
{
    private bool _isAttacked;
    private Player _player;
    private PlayerAttackCompo _playerAttacker;
    [SerializeField] private PlayerManagerSO _playerManager;

    public override void OnEnter()
    {
        base.OnEnter();
        _player = _playerManager.Player;
        _playerAttacker = _player.GetCompo<PlayerAttackCompo>();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if(_isAttacked)
        {
            _tutorialManager.NextStep();
        }
        if (!_isAttacked)
        {
            _isAttacked = _playerAttacker.isShooting;
        }
    }
}
