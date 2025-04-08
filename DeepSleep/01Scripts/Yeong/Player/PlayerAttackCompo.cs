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
    private BulletPayload _bulletPayload;
    [SerializeField] private StatElementSO _damageSO;

    public event Action<int,int> FireEvent;
    public event Action<int,bool> UIReloadEvent;

    private PlayerMovement _mover;
    private float _nextShootTime;
    private Player _player;
    private bool _isShooting, _weaponReady = true;
    private int _bulletInMagazine;
    [SerializeField] private float _bulletSpeed, _shootingRange, _impactForce;
    private float _currentSpread, _lastSpreadTime;
    [SerializeField] private float _spreadCooldown,_spreadAmount, _spreadIncRate,_maxSpreadAmount,_reloadSpeed; //탄퍼짐 제어 하는 용도
    [field:SerializeField] public int maxAmmo;
    private int _toReloadBullet;

    public event Action<float> ReloadEvent;
    public void Initialize(Entity entity)
    {
        _player = entity as Player;

        _mover = _player.GetCompo<PlayerMovement>();
        _player.PlayerInput.FireEvent += HandleFireEvent;
        _bulletPayload = new BulletPayload();
        _player.PlayerInput.ReloadEvent += HandleReloadEvent;
        _player.PlayerInput.FireEvent += HandleFireReloadEvent;

        _player.GetCompo<PlayerAnimator>().ReloadAnimationStatusChange += HandleReloadAnimationStatusChange;

        _bulletInMagazine = maxAmmo;
    }

    private void HandleFireReloadEvent(bool obj)
    {
        if (CheckBullet())
        {
            if (_weaponReady && CanReload())
            {
                ReloadEvent?.Invoke(_reloadSpeed);
                UIReloadEvent?.Invoke(maxAmmo, true);
                TryToReloadBullet(); //준비
            }
        }
    }

    private void HandleFireEvent(bool isFire)
    {
        _isShooting = isFire;
    }
    private void HandleReloadAnimationStatusChange(bool isReloadPlay)
    {
        _weaponReady = !isReloadPlay;

        if (!isReloadPlay)
        {
            FillBullets(); //재장전 완료니까 탄약 채워주라.
            UIReloadEvent?.Invoke(maxAmmo, false);
        }
    }

    private void HandleReloadEvent()
    {
        if (_weaponReady && CanReload())
        {
            UIReloadEvent?.Invoke(maxAmmo,true);
            ReloadEvent?.Invoke(_reloadSpeed);
            TryToReloadBullet(); //준비
        }
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
        
        _bulletInMagazine--;
        FireEvent?.Invoke(_bulletInMagazine,maxAmmo);
        float fireRate = _player.attackCooldownStat.Value;
        _nextShootTime = Time.time + 1/fireRate;

        FireSingleBullet();

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
        Vector3 bulletDirection = _player.GetCompo<PlayerAim>().GetBulletDirection(_player.fireTrm);
        _bulletPayload.mass = 20f / _bulletSpeed;
        _bulletPayload.shootingRange = _shootingRange;
        _bulletPayload.impactForce = _impactForce;
        _bulletPayload.damage = _player.GetCompo<StatCompo>().GetElement(_damageSO).Value;
        _bulletPayload.velocity
            = bulletDirection * _bulletSpeed;

        var evt = SpawnEvents.PlayerBulletCreate;
        evt.position = _player.fireTrm.position;
        evt.rotation = _player.fireTrm.rotation;
        evt.payload = _bulletPayload;
        evt.owner = _player;
        CameraManager.Instance.ShakeCamera(1.8f, 1.8f, 0.15f);

        _spawnChannel.RaiseEvent(evt);
    }


    private bool CanShooting()
    {
        bool isCoolTime = _nextShootTime > Time.time;
        bool isEmptyClip = _bulletInMagazine <= 0;
        if (isCoolTime || isEmptyClip) return false;

        return true;
    }
    public bool CanReload()
    {
        return _bulletInMagazine < maxAmmo;
    }

    public void TryToReloadBullet()
    {
        int requireCount = maxAmmo - _bulletInMagazine;
        _toReloadBullet = requireCount;
    }

    public void FillBullets()
    {
        if (_toReloadBullet <= 0) return;

        _bulletInMagazine += _toReloadBullet;
        _toReloadBullet = 0;
    }

    public bool CheckBullet()
    {
        return _bulletInMagazine == 0;
    }

}
