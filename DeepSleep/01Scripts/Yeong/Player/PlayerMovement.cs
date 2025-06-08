using DG.Tweening;
using System;
using System.Collections;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    public bool CanDash { get; private set; } 

    public event Action<Vector3> OnMovementEvent;
    public event Action<bool> OnDashEvent;
    public event Action<float, float> DashCoolEvent;

    private Player _player;
    private Vector3 _movement;
    public Vector3 Movement => _movement;

    private float _verticalVelocity;
    private bool _isDash;
    private Quaternion _targetRotation;
    
    private StatElement _speedStat;
    private EntityStat _statCompo;
    private PlayerAim _aimCompo;
    
    private float _currentDashCool = 0f;
    private float _dashCoolTime = 2f;
    private Collider _collider;
    private Vector3 _dashDestination;
    public bool CanManualMove { get; set; } = true;
    private readonly float _dashTime = 0.2f;

    public void Initialize(Entity player)
    {
        _player = player as Player;
        CharacterControllerCompo = _player.GetComponent<CharacterController>();
        _aimCompo = _player.GetCompo<PlayerAim>();
        _aimCompo.OnLookDirectionEvent += HandleLookDirectionEvent;
        _statCompo = _player.GetCompo<EntityStat>();
        _player.PlayerInput.DashEvent += HandleDashEvent;
        _collider = _player.GetComponent<Collider>();
    }

    private void HandleDashEvent()
    {
        if(_currentDashCool >= 0)
            return;
        if (_player.PlayerInput.Movement.magnitude < 0.1f)
            return;
        OnDashEvent?.Invoke(true);
    }

    public void Dash()
    {
        StopImmediately();
        _player.GetCompo<EntityHealth>().SetInvincible(true);
        _currentDashCool = _dashCoolTime;
        CanManualMove = false;
        _isDash = true;
        CanDash = false;

        Vector3 rollingDirection = GetRollingDirection();
        _player.transform.rotation = Quaternion.LookRotation(rollingDirection);
        _dashDestination = rollingDirection;

        DOVirtual.DelayedCall(_dashTime, EndDash);
    }

    private void EndDash()
    {
        OnDashEvent?.Invoke(false);
        StartCoroutine(InvicibleDelay());
        CharacterControllerCompo.enabled = true;
        CanManualMove = true;
        _isDash = false;
    }

    private IEnumerator InvicibleDelay()
    {
        yield return new WaitForSeconds(0.2f);
        _player.GetCompo<EntityHealth>().SetInvincible(false);
    }

    private Vector3 GetRollingDirection()
    {
        Vector3 direction = Vector3.zero;
        Vector3 moveInput = _player.PlayerInput.Movement;
        if (_player.PlayerInput.Movement.magnitude < 0.1f)
        {
            moveInput = _player.transform.forward.normalized;
            moveInput = new Vector2(moveInput.x, moveInput.z);
        }

        direction = Quaternion.Euler(0, -45f, 0) * new Vector3(moveInput.x, 0, moveInput.y);
        _targetRotation = Quaternion.LookRotation(direction);
        return direction;
    }

    public void AfterInit()
    {
        _speedStat = _statCompo.GetElement("Speed");
    }
    
    private void HandleLookDirectionEvent(Quaternion rotation)
    {
        if (_isDash) return;
        _targetRotation = rotation;
    }

    private void Update()
    {
        if (_currentDashCool >= 0)
        {
            _currentDashCool -= Time.deltaTime;
            
            if (_currentDashCool <= 0)
            {
                CanDash = true;
            }
            
            DashCoolEvent?.Invoke(_currentDashCool, _dashCoolTime);
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
        if(_isDash)
            CharacterControllerCompo.Move(_dashDestination*Time.fixedDeltaTime* (_dashSpeedCurve.Evaluate(_dashTime)*30));
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
        _player.transform.rotation =  _targetRotation;
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
        size.y -= 0.3f; // 바닥과의 불필요한 충돌 방지

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
