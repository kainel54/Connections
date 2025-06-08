using DG.Tweening;
using System;
using UnityEngine;
using YH.Animators;
using YH.Enemy;
using YH.Entities;
using YH.Feedbacks;
using YH.StatSystem;

public abstract class EnemyAnimator : MonoBehaviour, IEntityComponent, IAfterInitable
{
    protected Entity _entity;
    private Animator _animator;
    public Animator Animator => _animator;
    
    protected EnemyMovement _movement;

    public event Action<bool> SetDamageCasterEvent;
    [SerializeField] private StatElementSO _attackSpeedSO;
    [SerializeField] private AnimParamSO _attackSpeedParam;
    private Renderer _renderer;
    private BlinkFeedback _blinkFeedBack;
    private Material _blinkMat;
    private readonly int _blinkShaderParam = Shader.PropertyToID("_BlinkValue");
    private readonly int _alphaColorParam = Shader.PropertyToID("_Color");
    public virtual void Initialize(Entity entity)
    {
        _entity = entity;
        _animator = GetComponent<Animator>();
        _movement = _entity.GetCompo<EnemyMovement>();
        _renderer = GetComponentInChildren<Renderer>();
        _blinkFeedBack = _entity.GetComponentInChildren<BlinkFeedback>();
        _blinkMat = _renderer.material;

        _blinkMat.SetFloat(_blinkShaderParam, 0);
        Color color = new Color(1, 1, 1, 1);
        _blinkMat.SetColor(_alphaColorParam, color);
    }
  
    public void StartManualMove() => _movement.SetManualMove(true);
    public void StopManualMove() => _movement.SetManualMove(false);
    public void StartManualRotation() => _movement.SetManualRotation(true);
    public void StopManualRotation() => _movement.SetManualRotation(false);
   

    public void StartCast() => SetDamageCasterEvent?.Invoke(true);
    public void StopCast() => SetDamageCasterEvent?.Invoke(false);

    public void SetDead()
    {
        Animator.Rebind();
        Animator.SetBool("IDLE", false);
        Animator.SetBool("DEAD", true);
    }

    private void Setting()
    {
        _blinkFeedBack.StopDelayCorutine();
        _blinkMat.SetFloat(_blinkShaderParam, 0);
        Color color = new Color(1, 1, 1, 1);
        _blinkMat.SetColor(_alphaColorParam, color);

        float attackSpeed = _entity.GetCompo<EntityStat>().GetElement(_attackSpeedSO).Value;
        _animator.SetFloat(_attackSpeedParam.hashValue, attackSpeed);
    }

    public void AfterInit()
    {
        Setting();
    }

    public void Dispose()
    {
        
    }
}
