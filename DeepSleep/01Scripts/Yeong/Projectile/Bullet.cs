using ObjectPooling;
using UnityEngine;
using YH.Entities;
using YH.Projectile;

public class BulletPayload
{
    public float mass;
    public Vector3 velocity;
    public float shootingRange;
    public float impactForce;
    public float damage;
}

public class Bullet : ProjectileObj
{
    protected TrailRenderer _trailRenderer;
    [SerializeField] private SoundSO _impactSound;

    protected override void Awake()
    {
        base.Awake();
        _trailRenderer = GetComponent<TrailRenderer>();
    }

    protected override void Update()
    {
        base.Update();
        if (_distance >= _maxDistance * 0.6f)
        {
            _trailRenderer.time -= 2 * Time.deltaTime;
        }
    }

    protected override void OnCollisionEnter(Collision other)
    {
        base.OnCollisionEnter(other);
        SoundPlayer sound = PoolManager.Instance.Pop(ObjectType.SoundPlayer) as SoundPlayer;
        sound.PlaySound(_impactSound);
    }

    public override void Fire(Vector3 position, Quaternion rotation, BulletPayload payload,Entity owner)
    {
        base.Fire(position, rotation, payload, owner);
        _trailRenderer.Clear();
    }

    public override void Init()
    {
        _trailRenderer.time = 0.25f;
    }


}
