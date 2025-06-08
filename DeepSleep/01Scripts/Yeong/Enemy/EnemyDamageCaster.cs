using System;
using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.StatSystem;

public class EnemyDamageCaster : MonoBehaviour, IEntityComponent, IAfterInitable
{
    [SerializeField] private LayerMask _targetLayer;

    private BTEnemy _btEnemy;
    [SerializeField] private DamageCaster[] _casters;
    private bool _isActive;
    private EntityStat _statCompo;
    [SerializeField] private StatElementSO _attackPowerSO;
    private StatElement _attackPower;
    public bool IsSuccess { get; private set; }

    public void Initialize(Entity entity)
    {
        _btEnemy = entity as BTEnemy;

        foreach (var caster in _casters)
        {
            caster.InitCaster(_btEnemy);
        }
        _statCompo = _btEnemy.GetCompo<EntityStat>();
    }

    public void AfterInit()
    {
        _attackPower = _statCompo.GetElement(_attackPowerSO);
    }

    public void SetDamageCaster(bool isActive)
    {
        _isActive = isActive;
        if (isActive)
        {
            IsSuccess = false; //공격을 시작할때 성공을 꺼두고.

            foreach (var caster in _casters)
            {
                caster.StartCasting();
            }
        }
    }

    public void OnceCast()
    {
        foreach (var caster in _casters)
        {
            caster.StartCasting();
            caster.CastDamage(_attackPower.Value, Vector3.zero, false, _targetLayer);
        }
    }

    public void CastOn(int castIdx)
    {
        _casters[castIdx].StartCasting();
        _casters[castIdx].CastDamage(_attackPower.Value, Vector3.zero, false, _targetLayer);
    }

    public void Cast(DamageCaster caster)
    {
        caster.StartCasting();
        caster.CastDamage(_attackPower.Value, Vector3.zero, false, _targetLayer);
    }

    public DamageCaster GetCast(int castIdx)
    {
        return _casters[castIdx];
    }

    private void FixedUpdate() 
    {
        if(_isActive)
        {
            foreach(var caster in _casters)
            {
                bool result = caster.CastDamage(_attackPower.Value, Vector3.zero, false, _targetLayer);
                if(result)
                {
                    IsSuccess = true; //캐스팅중 한번이라도 성공하면 
                }
            }
        }
    }

    public void SetCast(DamageCaster caster,int idx)
    {
        _casters[idx] = caster;
    }

    public void Dispose()
    {

    }
}
