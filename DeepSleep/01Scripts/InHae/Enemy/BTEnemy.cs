using DG.Tweening;
using ObjectPooling;
using System.Collections;
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
    
    public Transform player;

    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private float turnSpeed;
    [SerializeField] protected LayerMask _whatIsTarget;
    [field : SerializeField] public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    
    protected BehaviorGraphAgent _btAgent;
    private Collider _collider;
    private SkinnedMeshRenderer _renderer;
    private EnemyMovement _mover;

    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    protected override void Awake()
    {
        base.Awake();
        player = _playerManagerSO.PlayerTrm;
        
        _btAgent = GetComponent<BehaviorGraphAgent>();
        _collider = GetComponent<Collider>();
        _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _mover = GetCompo<EnemyMovement>();

    }


    private void HandleSetPlayer()
    {
        player = _playerManagerSO.PlayerTrm;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            GetCompo<HealthCompo>().Die();
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

    public BlackboardVariable<T> GetVariable<T>(string variableName)
    {
        if(_btAgent.GetVariable(variableName, out BlackboardVariable<T> variable))
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

    private IEnumerator SelfDestroy()
    {
        _renderer.material.DOFade(0, 5f);
        yield return new WaitForSeconds(3.5f);
        PoolManager.Instance.Push(this, true);
        Coin coin = PoolManager.Instance.Pop(PoolingType.Coin) as Coin;
        coin.SetItemDropPosition(transform.position + new Vector3(0, 1.5f, 0));
        coin.value = Random.Range(1, 3);
        coin.transform.position = transform.position + new Vector3(0, 1, 0);
    }

    public virtual void Init()
    {
        GetCompo<HealthCompo>().Resurrection();
        player = FindAnyObjectByType<Player>(FindObjectsInactive.Include).transform;
        _renderer.material.color = new Color(1, 1, 1, 1);
        _btAgent.enabled = true;
        _btAgent.Start();
        _collider.enabled = true;
        IsDead = false;
        _mover.SpeedMultiplier = 1;
    }

    public void Setting(Vector3 spawnPos, Quaternion identity)
    {
        _mover.Setting(spawnPos, player.position);
    }
}
