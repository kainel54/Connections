using IH.EventSystem.StatusEvent;
using ObjectPooling;
using System.Collections;
using UnityEngine;
using YH.Core;
using YH.Enemy;
using YH.EventSystem;
using YH.Players;
using YH.StatSystem;

public class SpinksEnemyAttackCompo : EnemyAttackCompo
{

    [SerializeField] private GameEventChannelSO _spawnChannel;
    [SerializeField] private GameEventChannelSO _statusChannel;
    [SerializeField] private Transform _firePos;
    [SerializeField] private StatElementSO _damageSO;
    [SerializeField] private float _tornadoSpeed, _shootingRange, _impactForce, _fallMagicSpeed, _towerTornadoSpawnCount;
    [SerializeField] private int _healingValue = 70;
    private BulletPayload _bulletPayload;

    private SpinksTowerManager _spinksTowerManager;
    private SpinksEnemyAnimator _animator;
    private Spawner _enemySpawner;

    private EntityHealth _healthCompo;

    private int _firstmagicCount = 3;
    private int _secondmagicCount = 5;

    private int _firstmagicAngle = 50;
    private int _secondmagicAngle = 20;
    private bool _isChapter2 = false;

    private int _attackCount = 0;

    private Coroutine _coroutine;

    private void Awake()
    {
        _bulletPayload = new BulletPayload();
    }

    private void Start()
    {
        _healthCompo = _enemy.GetCompo<EntityHealth>();
        _animator = _enemy.GetCompo<SpinksEnemyAnimator>();
    }

    public void SetTowerManager(SpinksTowerManager spinksTowerManager)
    {
        _spinksTowerManager = spinksTowerManager;
        _enemySpawner = _spinksTowerManager.GetComponent<Spawner>();
    }


    #region SpinksSkills

    public void RushSliceAttack()
    {
        SpinksSlice spinksSlice = PoolManager.Instance.Pop(ProjectileType.SpinksSlice) as SpinksSlice;
        spinksSlice.Setting(_enemy);
        spinksSlice.PlayEffect(_firePos.position, transform.rotation, transform.localScale);
    }

    public void MagicAttack()
    {
        int objCount = _isChapter2 ? _secondmagicCount : _firstmagicCount;
        float angleChangeValue = _isChapter2 ? _secondmagicAngle : _firstmagicAngle;
        float startAngle = -angleChangeValue * (objCount - 1) / 2f;
        for (int i = 0; i < objCount; i++)
        {
            float angle = startAngle + i * angleChangeValue;

            SandTornado sandTornado = PoolManager.Instance.Pop(ProjectileType.SandTornado) as SandTornado;
            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 playerAngleSet = rotation * transform.forward;
            Quaternion targetDir = Quaternion.LookRotation(playerAngleSet);
            SetPayload(playerAngleSet, _tornadoSpeed);
            sandTornado.Fire(_firePos.position, targetDir, _bulletPayload, _enemy);
        }
    }

    private void CreateFallMagic(Vector3 spawnPos)
    {
        Vector3 createVector;

        for (int i = 0; i < 16; i++)
        {
            createVector = new Vector3(Random.Range(spawnPos.x - 15, spawnPos.x + 15), spawnPos.y + 30, Random.Range(spawnPos.z - 15, spawnPos.z + 15));
            SpinksFallMagic fallMagic = PoolManager.Instance.Pop(ProjectileType.FallMagic) as SpinksFallMagic;
            Quaternion targetDir = Quaternion.LookRotation(fallMagic.transform.forward);
            SetPayload(Vector3.down, _fallMagicSpeed);
            fallMagic.Fire(createVector, targetDir, _bulletPayload, _enemy);
            if (Physics.Raycast(fallMagic.transform.position, Vector3.down, out RaycastHit hit, 35f))
            {
                fallMagic.SetLifeTime(hit.point, Vector3.Distance(hit.point, fallMagic.transform.position) / _fallMagicSpeed);
            }
        }
    }

    public void FallMagicAttack()  // �ϴÿ��� �� �������� ���� << �̰Ŵ� Tower Skill �ε� ��� ����
    {
        CreateFallMagic(_magicFallShootTrm.position);
    }

    public void FallAttack()
    {
        var attackEffect = PoolManager.Instance.Pop(ProjectileType.FallAttack) as SpinksFallAttack;
        attackEffect.Setting(_enemy);
        attackEffect.PlayEffect(_firePos.position, transform.rotation, Vector3.one);
    }

    public void FallAttackShowingRANGE()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10f))
        {
            var display = PoolManager.Instance.Pop(UIPoolingType.BombCircleDisplay) as BombDisplay;
            display.SettingCircle(5, hit.point + Vector3.up * 0.3f, 1.4f); //����
        }
    }

    #endregion


    #region TowerSkill
    public void BuffGive()
    {
        int rand = Random.Range(0, 3);
        _coroutine = StartCoroutine(RandomBuffGiving(rand));
    }
    private IEnumerator RandomBuffGiving(int idx)
    {

        switch (idx)
        {
            case 0:
                {
                    var evt = StatusEvents.AddTimeStatusEvent;
                    evt.entity = _enemy;
                    evt.status = StatusEnum.AttackDamageBuff;
                    evt.time = 10f;
                    _statusChannel.RaiseEvent(evt);

                    var buffEffect = PoolManager.Instance.Pop(EffectPoolingType.DamageBuff) as PoolingDefaultEffectPlayer;
                    buffEffect.SetDuration(10f);
                    buffEffect.PlayEffect(_animator.transform.position, Quaternion.identity, Vector3.one, _animator.transform);

                    break;
                }
            case 1:
                {
                    for (int i = 0; i < 3; i++)
                    {

                        _healthCompo.ApplyRecovery(_healingValue);
                        var buffEffect = PoolManager.Instance.Pop(EffectPoolingType.HealingBuff) as PoolingDefaultEffectPlayer;
                        buffEffect.SetDuration(1f);
                        buffEffect.PlayEffect(_animator.transform.position, Quaternion.identity, Vector3.one, _animator.transform);

                        yield return new WaitForSeconds(1f);
                    }
                    break;
                }
            case 2:
                {
                    var evt = StatusEvents.AddTimeStatusEvent;
                    evt.entity = _enemy;
                    evt.status = StatusEnum.DefenseBuff;
                    evt.time = 10f;
                    _statusChannel.RaiseEvent(evt);

                    var buffEffect = PoolManager.Instance.Pop(EffectPoolingType.DefenceBuff) as PoolingDefaultEffectPlayer;
                    buffEffect.SetDuration(10f);

                    buffEffect.PlayEffect(_animator.transform.position, Quaternion.identity, Vector3.one, _animator.transform);
                    break;
                }
        }

        yield return null;
    }
    public void SpawnEnemy()
    {
        //Ǯ�޴�¡
        _enemySpawner.SetWave();
        _enemySpawner.Spawn();
    }

    public void SpawnSandTornado()
    {

        for (int i = 0; i < _towerTornadoSpawnCount; i++)
        {
            Vector3 randPos = _spinksTowerManager.GetTornadoRandomPos();

            Vector3 direction = _spinksTowerManager.GetTornadoDirection();

            SandTornado sandTornado = PoolManager.Instance.Pop(ProjectileType.SandTornado) as SandTornado;

            Quaternion targetDir = Quaternion.LookRotation(direction);

            SetPayload(direction, _tornadoSpeed);
            sandTornado.Fire(randPos, targetDir, _bulletPayload, _enemy);
        }
    }
    #endregion

    public void SetDizz()
    {

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
    public void GoToPhase2()
    {
        _isChapter2 = true;
        CameraManager.Instance.ShakeCamera(4f, 8f, 2f);
        _spinksTowerManager.TopUpEvent();
    }

    public void GoToPhase3()
    {
        StopAllCoroutines();
        StartCoroutine(CurrentTowerUsingSkill());
    }

    public void GoToDead()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        _enemy.SetDead();
    }


    public Vector3 GetSpawnPoint()
    {
        return _magicFallShootTrm.position;
    }


    private IEnumerator CurrentTowerUsingSkill()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            _spinksTowerManager.GetRandomAliveTower().UseSkill();
        }
    }
}
