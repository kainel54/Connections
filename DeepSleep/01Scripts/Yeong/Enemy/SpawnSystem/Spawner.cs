using DG.Tweening;
using ObjectPooling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using YH.Players;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;

    private int _dieEnemyCount;

    [SerializeField] private Transform spawnPos;
    [SerializeField] private WaveListSO _waveData;
    [SerializeField] private TextMeshProUGUI _testText;
    [SerializeField] private SoundSO _spawnSound;

    [SerializeField] private float spawnRangeRadius = 10f;
    private List<BTEnemy> _spawnEnemy = new List<BTEnemy>();

    private Player _player;
    private bool _debugTextOn;
    private WaveSO _currentWaveSO;
    private WaveData _currentWaveData;
    private byte _currentWaveCnt;

    private LevelRoom _currentLevelRoom;


    private void Start()
    {
        _currentLevelRoom = GetComponent<LevelRoom>();
        
        _playerManagerSo.SetUpPlayerEvent += HandleSettingPlayer;

        for (int i = 0; i < _waveData.waveList.Count; i++)
        {
            WaveSO wave = _waveData.waveList[i];
            if(wave.waveList.Count <= 0)
            {
                _waveData.waveList.Remove(wave);
            } 
        }
        _testText.enabled = false;
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Keyboard.current.gKey.isPressed)
        {
            Spawn(); 
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"{_currentWaveSO.name}/ 난이도{_currentWaveSO.waveDifficult.ToString()}, 웨이브개수{_currentWaveSO.waveList.Count}개");
            _testText.text = $"{_currentWaveSO.name}/ 난이도{_currentWaveSO.waveDifficult.ToString()}, 웨이브개수{_currentWaveSO.waveList.Count}개";
            _testText.enabled = true;
            _debugTextOn = true;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            _testText.enabled = false;
        }
    }
#endif

    private void OnDestroy()
    {
        _playerManagerSo.SetUpPlayerEvent -= HandleSettingPlayer;
    }

    private void HandleSettingPlayer()
    {
        _player = _playerManagerSo.Player;
    }

    public void Spawn()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        List<Vector3> spawnPositions = GetSpawnPositionsByShape(_currentWaveData.enemyType.Count);

        for (int i = 0; i < _currentWaveData.enemyType.Count; i++)
        {
            int copyIndex = i;
            Vector3 spawnPos = spawnPositions[i];

            // 기존 코드 유지
            SpawnNotice spawnNotice = PoolManager.Instance.Pop(EffectPoolingType.SpawnNotice) as SpawnNotice;
            spawnNotice.GameObject.transform.position = spawnPos;

            SoundPlayer sound = PoolManager.Instance.Pop(ObjectType.SoundPlayer) as SoundPlayer;
            sound.PlaySound(_spawnSound);
            sound.transform.position = spawnPos;

            spawnNotice.OnEndEvent += () =>
            {
                BTEnemy enemy = SpawnEnemy(_currentWaveData.enemyType[copyIndex], spawnPos);
                RegisterOnDieEvent(enemy);
                _spawnEnemy.Add(enemy);
            };

            yield return new WaitForSeconds(_currentWaveData.spawnDelay);
        }

    }

    private List<Vector3> GetSpawnPositionsByShape(int count)
    {
        List<Vector3> positions = new List<Vector3>();
        Vector3 center = spawnPos.position;

        switch (_currentWaveData.spawnType)
        {
            case SpawnType.Circle:
                for (int i = 0; i < count; i++)
                {
                    float angle = i * Mathf.PI * 2f / count;
                    float radius = spawnRangeRadius; // 최대 범위 내 원
                    Vector3 pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius + center;
                    positions.Add(pos);
                }
                break;

            case SpawnType.XShape:
                int maxLevel = Mathf.CeilToInt(count / 4f);
                float baseStep = spawnRangeRadius / maxLevel;
                bool reverse = _currentWaveData.isReverse;

                // 1) !reverse일 때만 중앙을 먼저 추가
                if (!reverse && positions.Count < count)
                    positions.Add(center);

                // 2) 레벨 순서 결정
                int lvlStart = reverse ? maxLevel-1 : 1;
                int lvlEnd = reverse ? 1 : maxLevel - 1;
                int lvlStep = reverse ? -1 : 1;

                for (int lvl = lvlStart; reverse ? lvl >= lvlEnd : lvl <= lvlEnd; lvl += lvlStep)
                {
                    int reverseCount = reverse ? 1 : 0;
                    if (positions.Count >= count - reverseCount) break;
                    float step = lvl * baseStep * 2f;

                    // 네 대각선 방향 (X자)
                    positions.Add(center + new Vector3(step, 0, step)); // NE
                    if (positions.Count >= count - reverseCount) break;
                    positions.Add(center + new Vector3(-step, 0, step)); // NW
                    if (positions.Count >= count - reverseCount) break;
                    positions.Add(center + new Vector3(step, 0, -step)); // SE
                    if (positions.Count >= count - reverseCount) break;
                    positions.Add(center + new Vector3(-step, 0, -step)); // SW
                }

                // 3) reverse일 때만 중앙을 마지막에 추가
                if (reverse && positions.Count < count)
                    positions.Add(center);
                break;

            case SpawnType.Cross:

                maxLevel = Mathf.CeilToInt(count / 4f);
                baseStep = spawnRangeRadius / maxLevel;
                reverse = _currentWaveData.isReverse;

                // 1) !isReverse일 때 중앙을 먼저, 아닐 때는 나중에 추가
                if (!reverse && positions.Count < count)
                    positions.Add(center);

                // 2) 레벨 순서를 reverse에 따라 바꿔서 loop
                lvlStart = reverse ? maxLevel-1 : 1;
                lvlEnd = reverse ? 1 : maxLevel - 1;
                lvlStep = reverse ? -1 : 1;

                for (int lvl = lvlStart; reverse ? lvl >= lvlEnd : lvl <= lvlEnd; lvl += lvlStep)
                {
                    int reverseCount = reverse ? 1 : 0;
                    if (positions.Count >= count - reverseCount) break;

                    float step = lvl * baseStep * 2f;
                    positions.Add(center + Vector3.forward * step);
                    if (positions.Count >= count - reverseCount) break;
                    positions.Add(center + Vector3.right * step);
                    if (positions.Count >= count - reverseCount) break;
                    positions.Add(center + Vector3.back * step);
                    if (positions.Count >= count- reverseCount) break;
                    positions.Add(center + Vector3.left * step);
                }

                // 3) isreverse일 때는 중앙을 마지막에 추가
                if (reverse && positions.Count < count)
                    positions.Add(center);
                break;

            default:
                for (int i = 0; i < count; i++)
                {
                    Vector2 randCircle = Random.insideUnitCircle * spawnRangeRadius;
                    Vector3 pos = center + new Vector3(randCircle.x, 0, randCircle.y);
                    positions.Add(pos);
                }
                break;
        }

        if (positions.Count > count)
            return positions.Take(count).ToList();
        else if (positions.Count < count)
        {
            // 모자란 만큼 랜덤으로 채워줍니다.
            for (int i = positions.Count; i < count; i++)
            {
                Vector2 randCircle = Random.insideUnitCircle * spawnRangeRadius;
                positions.Add(center + new Vector3(randCircle.x, 0, randCircle.y));
            }
        }

        return positions;
    }


    private BTEnemy SpawnEnemy(EnemyPoolingType enemyType, Vector3 spawnPos)
    {
        BTEnemy enemy = PoolManager.Instance.Pop(enemyType) as BTEnemy;

        if(enemy== null)
        {
            Debug.LogWarning($"{enemyType} pooling �Ұ�");
        }

        enemy.transform.SetParent(transform);
        enemy.Setting(spawnPos, Quaternion.identity);

        return enemy;
    }

    private void RegisterOnDieEvent(BTEnemy enemy)
    {
        EntityHealth health = enemy.GetCompo<EntityHealth>();

        UnityAction onDie = null;
        onDie = () =>
        {
            _spawnEnemy.Remove(enemy);
            HandleDeadCheck(enemy);
            health.OnDieEvent.RemoveListener(onDie);
        };

        health.OnDieEvent.AddListener(onDie);
    }

    private void HandleDeadCheck(BTEnemy enemy)
    {
        _dieEnemyCount++;

        if (_dieEnemyCount < _currentWaveData.enemyType.Count)
            return;

        NextWave();
    }

    private void NextWave()
    {
        _currentWaveCnt++;


        while (_currentWaveCnt < _currentWaveSO.waveList.Count &&
               _currentWaveSO.waveList[_currentWaveCnt].enemyType.Count == 0)
        {
            _currentWaveCnt++;
        }

        if (_currentWaveCnt >= _currentWaveSO.waveList.Count)
        {
            _currentLevelRoom.LevelClear();
            return;
        }

        _dieEnemyCount = 0;
        Spawn();
    }


    public void SetWave()
    {
        while (true)
        {
            int randomIdx = Random.Range(0, _waveData.waveList.Count);

            _currentWaveSO = _waveData.waveList[randomIdx];
            if (_currentWaveSO.waveDifficult != WaveDifficult.Hard)
                break;
        }
        
        _currentWaveData = _currentWaveSO.waveList[0];

        _dieEnemyCount = 0;
        _currentWaveCnt = 0;
    }
}
