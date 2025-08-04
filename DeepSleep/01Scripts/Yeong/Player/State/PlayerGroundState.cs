using ObjectPooling;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.FSM;
using YH.Players;

public class PlayerGroundState : EntityState
{
    protected MOBAPlayer _player;
    protected EntityAIMover _mover;
    private bool _isEffectPlayed = false;
    public PlayerGroundState(Entity entity, AnimParamSO animParam) : base(entity, animParam)
    {
        _player = entity as MOBAPlayer;
        _mover = entity.GetCompo<EntityAIMover>();
    }

    public override void Enter()
    {
        base.Enter();
        
        _player.PlayerInput.StopEvent += HandleStopEvent;
        _player.PlayerInput.MoveEvent += HandleMoveEvent;
        _player.PlayerInput.DashEvent += HandleDashEvent;
    }

    private void HandleDashEvent()
    {
        if (_player.isDashing || !_player.IsDashEnable) return;
        if (_player.currentDashCount <= 0) return;
        _player.DashEvent?.Invoke();
        _player.currentDashCount--;
        _player.ChangeState(FSMState.Dash);
    }

    private void HandleMoveEvent(bool isClick)
    {
        _player.isMoveClick = isClick;
        _player.MoveEvent?.Invoke();
        if (!isClick)
        {
            _isEffectPlayed = false;
        }
    }

    public override void Update()
    {
        base.Update();
        if(_player.isMoveClick)
            Movement();

        if(_player.isAttackClick)
        {
            _player.ChangeState(FSMState.Attack);
            _player.AttackEvent?.Invoke();
        }
    }


    private void Movement()
    {
        if (!_mover.CanManualMove)
            return;

        RaycastHit hitData = _player.PlayerInput.GetMouseGroundHit();
        if (hitData.collider == null)
            return;

        if (!hitData.collider.CompareTag("InvisibleGround")&&!_isEffectPlayed)
        {
            var clickEffect = PoolManager.Instance.Pop(EffectPoolingType.ClickEffect) as PoolingEffectPlayer;
            clickEffect.PlayEffect(_player.PlayerInput.GetWorldMousePosition(), Quaternion.identity, Vector3.one * 0.2f, null);
            _isEffectPlayed = true;
        }

        _mover.SetMovement(_player.PlayerInput.GetWorldMousePosition());
    }

    private void HandleStopEvent()
    {
        _player.StopEvent?.Invoke();
        _player.ChangeState(FSMState.Idle);
    }

    public override void Exit()
    {
        base.Exit();
        _player.PlayerInput.StopEvent -= HandleStopEvent;
        _player.PlayerInput.MoveEvent -= HandleMoveEvent;
        _player.PlayerInput.DashEvent -= HandleDashEvent;
    }

    
}
