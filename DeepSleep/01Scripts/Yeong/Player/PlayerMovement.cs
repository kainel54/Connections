using DG.Tweening;
using System;
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
    public CharacterController CharacterControllerCompo { get; private set; }
    public bool IsGround => CharacterControllerCompo.isGrounded;
    public bool IsDash => _isDash;
    public bool CanDash { get; private set; } 

    public event Action<Vector3> OnMovementEvent;
    public event Action OnDashEvent;
    public event Action<float, float> DashCoolEvent;

    private Player _player;
    private Vector3 _movement;
    
    private float _verticalVelocity;
    private bool _isDash;
    private Quaternion _targetRotation;
    
    private StatElement _speedStat;
    private StatCompo _statCompo;
    
    private float _currentDashCool = 0f;
    private float _dashCoolTime = 2f;
    private Collider _collider;
    public bool CanManualMove { get; set; } = true;
    private readonly float _dashDistance = 5f, _dashTime = 0.5f;

    public void Initialize(Entity player)
    {
        _player = player as Player;
        CharacterControllerCompo = _player.GetComponent<CharacterController>();
        _player.GetCompo<PlayerAim>().OnLookDirectionEvent += HandleLookDirectionEvent;
        _statCompo = _player.GetCompo<StatCompo>();
        _player.PlayerInput.DashEvent += HandleDashEvent;
        _collider = _player.GetComponent<Collider>();
    }

    private void HandleDashEvent()
    {
        if(_currentDashCool >= 0)
            return;
        if (_player.PlayerInput.Movement.magnitude < 0.1f)
            return;
        OnDashEvent?.Invoke();
    }

    public void Dash()
    {
        StopImmediately();
        _currentDashCool = _dashCoolTime;
        CanManualMove = false;
        _isDash = true;
        CanDash = false;

        Vector3 rollingDirection = GetRollingDirection();
        _player.transform.rotation = Quaternion.LookRotation(rollingDirection);
        Vector3 destination = _player.transform.position + rollingDirection * (_dashDistance - 0.5f);
        float distance = _dashDistance;
        float dashTime = _dashTime;


        if (CheckColliderInFront(rollingDirection, ref distance))
        {
            destination = _player.transform.position + _player.transform.forward.normalized * (distance - 0.5f);
            dashTime = distance * _dashTime / _dashDistance;
            CharacterControllerCompo.enabled = false;
        }
        _player.transform.DOMove(destination, _dashTime).SetEase(Ease.OutQuad).OnComplete(EndDash);
    }

    private void EndDash()
    {
        CharacterControllerCompo.enabled = true;
        CanManualMove = true;
        _isDash = false;
    }

    private Vector3 GetRollingDirection()
    {
        Vector3 direction = Vector3.zero;
        direction = new Vector3(_player.transform.forward.x, 0, _player.transform.forward.z);
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
        _player.transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, Time.fixedDeltaTime * _rotationSpeed);
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
}
