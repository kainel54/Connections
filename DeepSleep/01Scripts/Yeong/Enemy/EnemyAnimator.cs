using System;
using UnityEngine;
using YH.Animators;
using YH.Enemy;
using YH.Entities;
using YH.StatSystem;

public abstract class EnemyAnimator : MonoBehaviour, IEntityComponent, IAfterInitable
{
    private Entity _entity;
    private Animator _animator;
    public Animator Animator => _animator;
    
    protected EnemyMovement _movement;

    public event Action<bool> SetDamageCasterEvent;
    [SerializeField] private StatElementSO _attackSpeedSO;
    [SerializeField] private AnimParamSO _attackSpeedParam;

    public virtual void Initialize(Entity entity)
    {
        _entity = entity;
        _animator = GetComponent<Animator>();
        _movement = _entity.GetCompo<EnemyMovement>();
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
        float attackSpeed = _entity.GetCompo<StatCompo>().GetElement(_attackSpeedSO).Value;
        _animator.SetFloat(_attackSpeedParam.hashValue, attackSpeed);
    }

    public void AfterInit()
    {
        Setting();
    }
}
