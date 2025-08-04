using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.Players;
using YH.StatSystem;

public class PlayerDamageCaster : MonoBehaviour, IEntityComponent, IAfterInitable
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private DamageCaster[] _casters;
    private bool _isActive;
    private EntityStat _statCompo;
    [SerializeField] private StatElementSO _attackPowerSO;
    private StatElement _attackPower;
    private Player _player;
    public bool IsSuccess { get; private set; }
    public void Initialize(Entity entity)
    {
        _player = entity as Player;

        foreach (var caster in _casters)
        {
            caster.InitCaster(entity);
        }
        _statCompo = entity.GetCompo<EntityStat>();
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
            caster.CastDamage(_attackPower.Value, Vector3.one * 2, true, _targetLayer);
        }
    }

    public void CastOn(int castIdx)
    {
        _casters[castIdx].StartCasting();
        _casters[castIdx].CastDamage(_attackPower.Value, Vector3.one * 2, true, _targetLayer);
    }

    public void Cast(DamageCaster caster)
    {
        caster.StartCasting();
        caster.CastDamage(_attackPower.Value, Vector3.one * 2, true, _targetLayer);
    }

    public DamageCaster GetCast(int castIdx)
    {
        return _casters[castIdx];
    }

    private void FixedUpdate()
    {
        if (_isActive)
        {
            foreach (var caster in _casters)
            {
                caster.CastDamage(_attackPower.Value, Vector3.one * 2.5f, true, _targetLayer);
            }
        }
    }

    

    public void SetCast(DamageCaster caster, int idx)
    {
        _casters[idx] = caster;
    }


    public void Dispose()
    {
    }
}
