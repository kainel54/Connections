using ObjectPooling;
using UnityEngine;
using YH.StatSystem;

public class SpinksSandTornadoTower : SpinksBossTower
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _shootingRange;
    [SerializeField] private float _impactForce;
    [SerializeField] private int _bulletCount;

    [SerializeField] private StatElementSO _damageSO;

    private BulletPayload _bulletPayload;

    private void Awake()
    {
        _bulletPayload = new BulletPayload();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        _attackCompo.SpawnSandTornado();
    }

    public void HandleShootingEvent()
    {

        for (int i = 0; i < _bulletCount; i++)
        {
            float angle = (360f / _bulletCount) * i;
            SandTornado sandTornado = PoolManager.Instance.Pop(ProjectileType.SandTornado) as SandTornado;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 playerAngleSet = rotation * transform.forward;
            Quaternion targetDir = Quaternion.LookRotation(playerAngleSet);
            SetPayload(playerAngleSet, _bulletSpeed);
            sandTornado.Fire(transform.position + Vector3.up * 1.5f, targetDir, _bulletPayload, _enemy);
        }
    }

    private void SetPayload(Vector3 bulletDirection, float bulletSpeed)
    {
        _bulletPayload.mass = 20f / bulletSpeed;
        _bulletPayload.shootingRange = _shootingRange;
        _bulletPayload.impactForce = _impactForce;
        _bulletPayload.damage = _enemy.GetCompo<EntityStat>().GetElement(_damageSO).Value;
        _bulletPayload.velocity
            = bulletDirection * bulletSpeed;
    }
}
