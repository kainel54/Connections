 using ObjectPooling;
using System;
using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.Entities;
using YH.EventSystem;

public class Grenade : MonoBehaviour, IPoolable
{
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private PoolingItemSO _explosionEffect;
    [SerializeField] private GameEventChannelSO _spawnChannel;
    [SerializeField] private SoundSO _bombSound;
    [field: SerializeField] public PoolingKey PoolKey { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    private Rigidbody _rbCompo;
    private float _gravity;

    private Entity _owner;
    private OverlapCircleDamageCaster _damageCaster;
    public float timeToTarget { get;private set; }
    public Enum PoolEnum { get => _type; set { } }

    [SerializeField] private ProjectileType _type;
    private void Awake()
    {
        _rbCompo = GetComponent<Rigidbody>();
        _gravity = Physics.gravity.magnitude;
        _damageCaster = GetComponentInChildren<OverlapCircleDamageCaster>();
    }

    public void FireGrenade(float fireAngle, Vector3 firePos, Vector3 targetPos, Entity owner)
    {
        _owner = owner;
        
        transform.position = firePos;
        float angle = fireAngle * Mathf.Deg2Rad; 
        Vector3 planeTarget = new Vector3(targetPos.x, 0, targetPos.z);
        Vector3 planePosition = new Vector3(firePos.x, 0, firePos.z);


        float distance = Vector3.Distance(planeTarget, planePosition);

        float yOffset = firePos.y - targetPos.y;

        float initVelocity = (1 / Mathf.Cos(angle))
            * Mathf.Sqrt((0.5f * _gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        float yVelocity = initVelocity * Mathf.Sin(angle);
        float zVelocity = initVelocity * Mathf.Cos(angle);
        Vector3 velocity = new Vector3(0, yVelocity, zVelocity);

        Vector3 planeDirection = planeTarget - planePosition;
        float angleBetween = Vector3.Angle(Vector3.forward, planeDirection);
        Vector3 crossValue = Vector3.Cross(Vector3.forward, planeDirection);

        if (crossValue.y < 0)
        {
            angleBetween *= -1;
        }

        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetween, Vector3.up) * velocity;
        _rbCompo.AddForce(finalVelocity * _rbCompo.mass, ForceMode.Impulse);
        _rbCompo.AddTorque(new Vector3(5f, 0, 0));
        timeToTarget = distance / zVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        var evt = SpawnEvents.EffectSpawn;
        evt.position = transform.position;
        evt.rotation = Quaternion.identity;
        evt.effectItem = _explosionEffect;
        evt.scale = Vector3.one;

        SoundPlayer sound = PoolManager.Instance.Pop(ObjectType.SoundPlayer) as SoundPlayer;
        sound.PlaySound(_bombSound);
        sound.transform.position = transform.position;

        CastDamage();
        CameraManager.Instance.ShakeCamera(4,4,0.15f);
        _spawnChannel.RaiseEvent(evt);

        PoolManager.Instance.Push(this);
    }
    
    private void CastDamage()
    {
        _damageCaster.InitCaster(_owner);
        _damageCaster.StartCasting();
        _damageCaster.CastDamage(10f, Vector3.zero, false, _targetLayer);
    }
    

    public void OnPop()
    {
        _rbCompo.linearVelocity = Vector3.zero;
        _rbCompo.angularVelocity = Vector3.zero;
    }

    public void OnPush()
    {

    }
}
