using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using YH.Entities;
using YH.Players;
using YH.StatSystem;

public class PlayerMovement : MonoBehaviour, IEntityComponent, IAfterInitable
{
    [SerializeField] private float _gravity = -9.8f, _rotationSpeed;
    [SerializeField] private StatElementSO _speedSO;
    [SerializeField] private LayerMask _whatIsWall;
    [SerializeField] private AnimationCurve _dashSpeedCurve;

    public CharacterController CharacterControllerCompo { get; private set; }
    public bool IsGround => CharacterControllerCompo.isGrounded;
    public bool IsDash => _isDash;

    public event Action<Vector3> OnMovementEvent;
    public event Action<bool> OnDashEvent;
    public event Action<float, float, int> DashCoolEvent;

    private Player _player;
    private Vector3 _movement;
    public Vector3 Movement => _movement;

    private float _verticalVelocity;
    private bool _isDash;
    private Quaternion _targetRotation;

    private StatElement _speedStat;
    private EntityStat _statCompo;
    private PlayerAim _aimCompo;

    private float _dashCooldown = 2f;
    private float _dashCooldownTimer = 0f;
    private int _maxDashCount = 2;
    private int _currentDashCount = 2;

    private Collider _collider;
    private Vector3 _dashDestination;
    public bool CanManualMove { get; set; } = true;
    private readonly float _dashTime = 0.2f;

    public void Initialize(Entity player)
    {
        _player = player as Player;
        CharacterControllerCompo = _player.GetComponent<CharacterController>();
        _aimCompo = _player.GetCompo<PlayerAim>();
        _statCompo = _player.GetCompo<EntityStat>();
        _collider = _player.GetComponent<Collider>();

        _aimCompo.OnLookDirectionEvent += HandleLookDirectionEvent;
        _player.PlayerInput.DashEvent += HandleDashEvent;
    }

    public void AfterInit()
    {
        _speedStat = _statCompo.GetElement("Speed");
    }

    private void HandleDashEvent()
    {
        if (_isDash) return;
        if (_currentDashCount <= 0) return;
        if (_player.PlayerInput.Movement.magnitude < 0.1f) return;

        OnDashEvent?.Invoke(true);
    }

    public void Dash()
    {
        StopImmediately();
        _isDash = true;
        _currentDashCount--;

        _player.GetCompo<EntityHealth>().SetInvincible(true);
        CanManualMove = false;

        Vector3 rollingDirection = GetRollingDirection();
        _player.transform.rotation = Quaternion.LookRotation(rollingDirection);
        _dashDestination = rollingDirection;

        DOVirtual.DelayedCall(_dashTime, EndDash);
    }

    private void EndDash()
    {
        OnDashEvent?.Invoke(false);
        _isDash = false;
        CanManualMove = true;
        StartCoroutine(InvicibleDelay());
    }

    private IEnumerator InvicibleDelay()
    {
        yield return new WaitForSeconds(0.2f);
        _player.GetCompo<EntityHealth>().SetInvincible(false);
    }

    private Vector3 GetRollingDirection()
    {
        Vector3 direction = Vector3.zero;
        Vector2 moveInput = _player.PlayerInput.Movement;

        if (moveInput.magnitude < 0.1f)
        {
            moveInput = new Vector2(_player.transform.forward.x, _player.transform.forward.z);
        }

        direction = Quaternion.Euler(0, -45f, 0) * new Vector3(moveInput.x, 0, moveInput.y);
        _targetRotation = Quaternion.LookRotation(direction);
        return direction;
    }

    private void HandleLookDirectionEvent(Quaternion rotation)
    {
        if (_isDash) return;
        _targetRotation = rotation;
    }

    private void Update()
    {
        if (_currentDashCount < _maxDashCount)
        {
            _dashCooldownTimer += Time.deltaTime;

            if (_dashCooldownTimer >= _dashCooldown)
            {
                _dashCooldownTimer -= _dashCooldown;
                _currentDashCount++;

                if (_currentDashCount >= _maxDashCount)
                {
                    _currentDashCount = _maxDashCount;
                    _dashCooldownTimer = 0f;
                }
            }

            DashCoolEvent?.Invoke(_dashCooldownTimer, _dashCooldown, _currentDashCount);
        }
    }

    private void FixedUpdate()
    {
        CalculateMovement();
        ApplyGravity();
        ApplyRotation();
        Move();
        DashMove();
    }

    private void DashMove()
    {
        if (_isDash)
        {
            CharacterControllerCompo.Move(_dashDestination * Time.fixedDeltaTime * (_dashSpeedCurve.Evaluate(_dashTime) * 30));
        }
    }

    private void CalculateMovement()
    {
        Vector2 moveInput = _player.PlayerInput.Movement;
        _movement = Quaternion.Euler(0, -45f, 0) * new Vector3(moveInput.x, 0, moveInput.y);

        OnMovementEvent?.Invoke(_movement);
        _movement *= _speedStat.Value * Time.fixedDeltaTime;
    }

    private void ApplyGravity()
    {
        if (IsGround && _verticalVelocity < 0)
        {
            _verticalVelocity = -1.5f;
        }
        else
        {
            _verticalVelocity += _gravity * Time.fixedDeltaTime;
        }

        _movement.y = _verticalVelocity;
    }

    private void ApplyRotation()
    {
        _player.transform.rotation = _targetRotation;
    }

    private void Move()
    {
        if (!CanManualMove) return;
        if (_player.IsDead) return;
        CharacterControllerCompo.Move(_movement);
    }

    public void StopImmediately()
    {
        _movement = Vector3.zero;
    }

    public bool CheckColliderInFront(Vector3 direction, ref float distance)
    {
        Vector3 center = _collider.bounds.center;
        Vector3 size = _collider.bounds.size;
        size.y -= 0.3f;

        var hit = Physics.BoxCast(center, size / 2, direction, out RaycastHit hitInfo, Quaternion.identity, distance, _whatIsWall);
        if (hit)
            distance = hitInfo.distance;
        return hit;
    }

    public void Dispose()
    {
        _player.GetCompo<PlayerAim>().OnLookDirectionEvent -= HandleLookDirectionEvent;
        _player.PlayerInput.DashEvent -= HandleDashEvent;
    }
}
