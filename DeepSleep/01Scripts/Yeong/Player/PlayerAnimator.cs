using DG.Tweening;
using ObjectPooling;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using YH.Animators;
using YH.Entities;
using YH.EventSystem;
using YH.Players;

public class PlayerAnimator : MonoBehaviour, IEntityComponent, IAfterInitable
{

    [SerializeField]
    private AnimParamSO _xVelocityParam, _zVelocityParam, _fireParam, _deathParam,_speedParam;

    private Animator _animator;
    private Player _player;
    private PlayerMovement _playerMovement;
    private PlayerAttackCompo _playerAttack;
    private SkinnedMeshRenderer _meshRender;

    public event Action OnDieEvent;

    private float _lastAttackTime;
    private bool _isAttackMode;
    private readonly int _blinkShaderParam = Shader.PropertyToID("_BlinkValue");
    private readonly int _blinkColorShaderParam = Shader.PropertyToID("_BlinkColor");

    [SerializeField] private GameEventChannelSO _spawnEvent;
    [SerializeField] private PoolingItemSO _effectItem;
    [SerializeField] private Color _dashEndColor;
    public void Initialize(Entity entity)
    {
        _animator = GetComponent<Animator>();
        _player = entity as Player;

        _playerMovement = _player.GetCompo<PlayerMovement>();
        _playerMovement.OnMovementEvent += HandleMovementEvent;
        _playerMovement.OnDashEvent += HandleDashEvent;
        _playerAttack = _player.GetCompo<PlayerAttackCompo>();
        _playerAttack.FireEvent += HandleFireEvent;
    }
    public void AfterInit()
    {
    }

    public void SwitchAnimationLayer(int layerIndex)
    {
        for (int i = 1; i < _animator.layerCount; i++)
        {
            float weight = i == layerIndex ? 1 : 0;
            _animator.SetLayerWeight(i, weight);
        }
    }
    private void HandleDashEvent(bool IsDash)
    {
        if (IsDash)
        {
            gameObject.SetActive(false);
            var evt = SpawnEvents.EffectSpawn;
            evt.position = _player.transform.position;
            evt.rotation = _player.transform.rotation;
            evt.scale = _player.transform.localScale;
            evt.effectItem = _effectItem;
            _spawnEvent.RaiseEvent(evt);
            _playerMovement.Dash();
        }
        else
        {
            _meshRender.material.SetFloat(_blinkShaderParam, 1);
            gameObject.SetActive(true);
            _meshRender.material.DOFloat(0, _blinkShaderParam, 0.5f);
        }
    }


    private void HandleFireEvent()
    {
        _animator.SetTrigger(_fireParam.hashValue);
    }

    private void HandleMovementEvent(Vector3 movement)
    {
        float x = Vector3.Dot(movement.normalized, transform.right);
        float z = Vector3.Dot(movement.normalized, transform.forward);

        float dampTime = 0.1f;
        _animator.SetFloat(_xVelocityParam.hashValue, x, dampTime, Time.fixedDeltaTime);
        _animator.SetFloat(_zVelocityParam.hashValue, z, dampTime, Time.fixedDeltaTime);

        float animationSpeed;
        if (x > 0.7f||z>0.7f)
        {
            animationSpeed = 1.5f;
        }
        else
        {
            animationSpeed = 1f;
        }
        _animator.SetFloat(_speedParam.hashValue, animationSpeed);
    }

    public void SetDie()
    {
        _animator.SetTrigger(_deathParam.hashValue);
    }

    public void DeathEvent()
    {
        OnDieEvent?.Invoke(); 
    }

    public void Dispose()
    {
        _playerMovement.OnMovementEvent -= HandleMovementEvent;
        _playerMovement.OnDashEvent -= HandleDashEvent;
        _playerAttack.FireEvent -= HandleFireEvent;
    }
}
