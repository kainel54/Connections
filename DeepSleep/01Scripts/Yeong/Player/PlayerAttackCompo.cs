using ObjectPooling;
using System;
using UnityEngine;
using YH.Core;
using YH.Entities;
using YH.EventSystem;
using YH.Players;
using YH.StatSystem;
using Random = UnityEngine.Random;

public class PlayerAttackCompo : MonoBehaviour, IEntityComponent
{
    [SerializeField] private GameEventChannelSO _spawnChannel;
    [SerializeField] private PoolingItemSO _muzzleFlashItem;
    private BulletPayload _bulletPayload;
    [SerializeField] private StatElementSO _damageSO;

    public event Action FireEvent;

    private PlayerMovement _mover;
    private float _nextShootTime;
    private Player _player;
    private bool _isShooting, _weaponReady = true;
    [SerializeField] private float _bulletSpeed, _shootingRange, _impactForce;
    private float _currentSpread, _lastSpreadTime;
    [SerializeField] private float _spreadCooldown,_spreadAmount, _spreadIncRate,_maxSpreadAmount,_reloadSpeed; //탄퍼짐 제어 하는 용도
    public bool isShooting => _isShooting;
    public void Initialize(Entity entity)
    {
        _player = entity as Player;

        _mover = _player.GetCompo<PlayerMovement>();
        _player.PlayerInput.AttackEvent += HandleFireEvent;
        _bulletPayload = new BulletPayload();
    }

    private void HandleFireEvent(bool isFire)
    {
        _isShooting = isFire;
    }

    private void Update()
    {
        if (_isShooting)
            Shooting();
    }

    private void Shooting()
    {
        if (_weaponReady == false)
            return;
        if (!CanShooting())
            return;
        if (_mover.IsDash)
            return;
        float fireRate = _player.attackCooldownStat.Value;
        _nextShootTime = Time.time + 1/fireRate;
        FireEvent?.Invoke();
        FireSingleBullet();

        TriggerEnemyAbility();
    }

    private void TriggerEnemyAbility()
    {
         Vector3 rayOrigin = _player.fireTrm.position;
         Vector3 rayDirection = _player.GetCompo<PlayerAim>().GetBulletDirection(_player.fireTrm);
         if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, Mathf.Infinity, _player.whatIsEnemy))
         {
             if (hit.collider.TryGetComponent(out IDodgeable dodge))
             {
                 dodge.DodgeTrigger();
             }
         }
    }

    public void UpdateSpread()
    {
        if (Time.time > _lastSpreadTime + _spreadCooldown)
        {
            _currentSpread = _spreadAmount;
        }
        else
        {
            IncreaseSpread();
        }
        _lastSpreadTime = Time.time;
    }

    public void IncreaseSpread()
    {
        _currentSpread = Mathf.Clamp(
            _currentSpread + _spreadIncRate,
            _spreadAmount,
            _maxSpreadAmount);
    }

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizeValue = Random.Range(-_currentSpread, _currentSpread);
        //쿼터니언 회전을 만들어서
        Quaternion spreadRot = Quaternion.Euler(randomizeValue, randomizeValue, randomizeValue);
        return spreadRot * originalDirection;
    }

    private void FireSingleBullet()
    {
        var muzzleEffect = PoolManager.Instance.Pop(EffectPoolingType.MuzzleFlash) as PoolingEffectPlayer;
        muzzleEffect.PlayEffect(_player.fireTrm.position,_player.fireTrm.rotation,Vector3.one,_player.transform);

        Vector3 bulletDirection = _player.GetCompo<PlayerAim>().GetBulletDirection(_player.fireTrm);
        _bulletPayload.mass = 20f / _bulletSpeed;
        _bulletPayload.shootingRange = _shootingRange;
        _bulletPayload.impactForce = _impactForce;
        _bulletPayload.damage = _player.GetCompo<EntityStat>().GetElement(_damageSO).Value;
        _bulletPayload.velocity
            = bulletDirection * _bulletSpeed;

        var evt = SpawnEvents.PlayerBulletCreate;
        evt.position = _player.fireTrm.position;
        evt.rotation = _player.fireTrm.rotation;
        evt.payload = _bulletPayload;
        evt.owner = _player;
        _spawnChannel.RaiseEvent(evt);

        
        CameraManager.Instance.ShakeCamera(1.8f, 1.8f, 0.15f);

    }
    private bool CanShooting()
    {
        bool isCoolTime = _nextShootTime > Time.time;
        if (isCoolTime) return false;

        return true;
    }



}
