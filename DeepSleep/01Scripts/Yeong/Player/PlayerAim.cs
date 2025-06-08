using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using YH.Entities;
using YH.Players;

public class PlayerAim : MonoBehaviour,IEntityComponent
{
    private Transform _aimTrm;
    [SerializeField] private Transform _rigAimTrm;
    [field: SerializeField] public bool IsAimPrecisely { get; private set; }
    [field: SerializeField] public bool IsAutoLock { get; private set; }
    [SerializeField] private float _playerHeight = 0.7f;
    [SerializeField] private Vector3 _aimOffset;
    public Transform AimingTarget { get; private set; }
    private Transform _previousTarget; // �߰�
    private Outline _currentOutline;   // ���� Ÿ���� Outline ĳ�̿�

    public event Action<Quaternion> OnLookDirectionEvent;

    private Player _player;
    private Vector3 _mousePos;
    private Vector3 _beforeLookDirection;

    public void Initialize(Entity player)
    {
        _player = player as Player;

        if (_aimTrm == null)
        {
            _aimTrm = GameObject.FindWithTag("AimTrm").transform;
        }
    }

    private void Update()
    {
        UpdateAimPosition();
        UpdateLookDirection();
        _rigAimTrm.position = _aimTrm.position;
    }
    

    private void UpdateAimPosition()
    {
        _mousePos = _player.PlayerInput.GetWorldMousePosition();
        _mousePos += _aimOffset;

        if (IsAutoLock)
        {
            Transform newTarget = GetTarget();
            if (newTarget != AimingTarget)
            {
                SetTargetOutline(AimingTarget, false); // ���� Ÿ���� Outline ���
                SetTargetOutline(newTarget, true);     // ���ο� Ÿ���� Outline �ѱ�
                AimingTarget = newTarget;
            }

            if (AimingTarget != null)
            {
                if (AimingTarget.TryGetComponent(out Renderer renderer))
                    _aimTrm.position = renderer.bounds.center;
                else
                    _aimTrm.position = AimingTarget.position + new Vector3(0, 1, 0);
                return;
            }
        }

        if (IsAimPrecisely)
        {
            _aimTrm.position = _mousePos;
        }
        else
        {
            _aimTrm.position = new Vector3(
                _mousePos.x, transform.position.y + _playerHeight, _mousePos.z);
        }
    }

    private void SetTargetOutline(Transform target, bool enabled)
    {
        if (target != null && target.TryGetComponent(out Outline outline))
        {
            outline.enabled = enabled;
        }
    }

    private Transform GetTarget()
    {
        Transform target = null;
        RaycastHit hit = _player.PlayerInput.GetMouseHitInfo();
        if (hit.collider != null)
        {
            target = hit.transform;
        }
        return target;
    }

    private void UpdateLookDirection()
    {
        if(_player.IsDead)
            return;
        
        Vector3 lookDirection = _mousePos - transform.position;
        lookDirection.y = 0;
        lookDirection.Normalize();
        if (_beforeLookDirection != lookDirection)
        {
            OnLookDirectionEvent?.Invoke(Quaternion.LookRotation(lookDirection));
            _beforeLookDirection = lookDirection;
        }
    }

    public Vector3 GetBulletDirection(Transform gunPointTrm)
    {
        Vector3 direction = (_aimTrm.position - gunPointTrm.position).normalized;
        //if (!IsAimPrecisely && AimingTarget == null)
        //{
        //    direction.y = 0;
        //}
        return direction;
    }
   
}
