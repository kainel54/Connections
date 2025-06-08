using DG.Tweening;
using ObjectPooling;
using System;
using System.Collections;
using IH.EventSystem.StatusEvent;
using Unity.Behavior;
using UnityEngine;
using YH.Entities;
using YH.EventSystem;
using YH.Players;
using Random = UnityEngine.Random;

public class BTEnemy : Entity, IPoolable
{
    // debug
    [SerializeField] private GameEventChannelSO _statusEventChannel;
    [SerializeField] private EnemyPoolingType _enemyPoolingType;
    public Transform player;

    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private float turnSpeed;
    [SerializeField] protected LayerMask _whatIsTarget;
    [field: SerializeField] public LayerMask whatIsWall;
    public LayerMask whatIsGround;

    protected BehaviorGraphAgent _btAgent;
    protected EnemyMovement _mover;
    protected Collider _collider;
    private SkinnedMeshRenderer _renderer;
    private EnemyDamageCaster _enemyDamageCaster;
    public Outline _outline { get; private set; }

    public GameObject GameObject { get => gameObject; set { } }

    public Enum PoolEnum { get => _enemyPoolingType; set { } }

    protected override void Awake()
    {
        base.Awake();
        player = _playerManagerSO.PlayerTrm;
        _playerManagerSO.SetUpPlayerEvent += HandleSetPlayer;

        _btAgent = GetComponent<BehaviorGraphAgent>();
        _collider = GetComponentInChildren<Collider>();
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _enemyDamageCaster = GetComponentInChildren<EnemyDamageCaster>();
        _mover = GetCompo<EnemyMovement>();
        _outline = GetComponent<Outline>();
        _outline.OutlineColor = Color.red;
        _outline.OutlineWidth = 1f;
        _outline.enabled = false;
        _collider.enabled = false;
        _renderer.material.DOFade(0, 0.1f);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _playerManagerSO.SetUpPlayerEvent -= HandleSetPlayer;
    }


    private void HandleSetPlayer()
    {
        player = _playerManagerSO.PlayerTrm;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GetCompo<EntityHealth>().Die();
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            var evt = StatusEvents.AddStatusEvent;
            evt.entity = this;
            evt.status = StatusEnum.SpeedDownDebuff;

            _statusEventChannel.RaiseEvent(evt);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            var evt = StatusEvents.AddTimeStatusEvent;
            evt.entity = this;
            evt.time = 4f;
            evt.status = StatusEnum.SpeedDownDebuff;

            _statusEventChannel.RaiseEvent(evt);
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            var evt = StatusEvents.RemoveStatusEvent;
            evt.entity = this;
            evt.status = StatusEnum.SpeedDownDebuff;

            _statusEventChannel.RaiseEvent(evt);
        }
    }
#endif

    public BlackboardVariable<T> GetVariable<T>(string variableName)
    {
        if (_btAgent.GetVariable(variableName, out BlackboardVariable<T> variable))
        {
            return variable;
        }

        return null;
    }

    public void SetVariable<T>(string variableName, T value)
    {
        BlackboardVariable<T> variable = GetVariable<T>(variableName);
        Debug.Assert(variable != null, $"Variable {variableName} not found");
        variable.Value = value;
    }

    public Transform GetTargetInRadius(float radius)
    {
        Collider[] collider = Physics.OverlapSphere(transform.position, radius, _whatIsTarget);
        if (collider.Length < 1)
            return null;

        return collider[0] != null ? collider[0].transform : null;
    }

    public void FaceToTarget(Vector3 target)
    {
        Quaternion targetRot = Quaternion.LookRotation(target - transform.position);
        Vector3 currentEulerAngle = transform.rotation.eulerAngles;

        float yRotation = Mathf.LerpAngle(currentEulerAngle.y, targetRot.eulerAngles.y, turnSpeed * Time.deltaTime);

        transform.rotation = Quaternion.Euler(currentEulerAngle.x, yRotation, currentEulerAngle.z);
    }

    public void SetDead()
    {
        IsDead = true;

        _btAgent.End();
        _btAgent.enabled = false;

        _collider.enabled = false;


        _mover.SetStop(true);

        StartCoroutine(SelfDestroy());
    }


    public virtual void Setinvincible(bool enableValue)
    {
        _collider.enabled = !enableValue;
    }

    private IEnumerator SelfDestroy()
    {
        _renderer.material.DOFade(0, 1f);
        yield return new WaitForSeconds(1.5f);
        PoolManager.Instance.Push(this, true);

        float random = Random.Range(0f, 100f);
        if (random < 10)
        {
            HealingPotion potion = PoolManager.Instance.Pop(ObjectType.HealingPotion) as HealingPotion;
            potion.SetItemDropPosition(transform.position + new Vector3(0.5f, 1.5f, 0));
            potion.transform.position = transform.position + new Vector3(0, 1, 0);
        }
    }

    public void Setting(Vector3 spawnPos, Quaternion identity)
    {
        _mover.Setting(spawnPos, player.position);
        _mover.SetStop(true);
        DOVirtual.DelayedCall(1.5f, () =>
        {
            _mover.SetStop(false);
            _collider.enabled = true;
        });
    }

    public virtual void OnPop()
    {
        AfterInitComponents();
    }

    public virtual void OnPush()
    {
        DisposeComponents();
        if(_enemyDamageCaster!=null)
            _enemyDamageCaster.SetDamageCaster(false);
        GetCompo<EntityHealth>().Resurrection();
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include).transform;
        IsDead = false;
        _mover.SpeedMultiplier = 1;
        _btAgent.enabled = true;
        _btAgent.Start();
    }
}
