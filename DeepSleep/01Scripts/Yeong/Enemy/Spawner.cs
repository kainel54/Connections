using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using YH.Players;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;

    private int _dieEnemyCount;
    private int _spawnEnemyCount;
    private int _spawnPowerEnemyCount;

    [SerializeField] private Transform _pointParent;
    [SerializeField] private List<PoolingType> _enemyList;
    [SerializeField] private List<PoolingType> _powerEnemyList;

    [SerializeField] private int _maxEnemyCount;
    [SerializeField] private int _maxPowerEnemyCount;
    [SerializeField] private float _spawnDelay;
    private List<Transform> _pointsList;
    private List<BTEnemy> _spawnEnemy = new List<BTEnemy>();

    private Player _player;
    

    public event Action levelClearEvent;

    private void Start()
    {
        _playerManagerSo.SetUpPlayerEvent += HandleSettingPlayer;

        _pointsList = new List<Transform>();

        for (int i = 0; i < _pointParent.childCount; i++)
        {
            _pointsList.Add(_pointParent.GetChild(i));
        }
    }

    private void Update()
    {
        if (Keyboard.current.gKey.isPressed)
        {
            Spawn(); 
        }
    }

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
        while (_spawnEnemyCount < _maxEnemyCount)
        {
            Transform farthest = _pointsList.OrderBy(t =>
                Vector3.Distance(_player.transform.position, t.position)).First();
            List<Transform> filteredList = _pointsList.Where(t => t != farthest).ToList();

            Vector3 spawnPos = filteredList[Random.Range(0, filteredList.Count)].position;

            BTEnemy enemy = PoolManager.Instance.Pop(_enemyList[Random.Range(0, _enemyList.Count)]) as BTEnemy;
            enemy.transform.SetParent(transform);
            enemy.Setting(spawnPos, Quaternion.identity);
            
            enemy.GetCompo<HealthCompo>().OnDieEvent.RemoveListener(() =>
            {
                _spawnEnemy.Remove(enemy);
                HandleDeadCheck();
            }); 
            
            enemy.GetCompo<HealthCompo>().OnDieEvent.AddListener(() =>
            {
                _spawnEnemy.Remove(enemy);
                HandleDeadCheck();
            });
            

            _spawnEnemyCount++;
            _spawnEnemy.Add(enemy);
            yield return new WaitForSeconds(_spawnDelay);
        }
        while (_spawnPowerEnemyCount < _maxPowerEnemyCount)
        {
            Transform farthest = _pointsList.OrderBy(t =>
                Vector3.Distance(_player.transform.position, t.position)).First();
            List<Transform> filteredList = _pointsList.Where(t => t != farthest).ToList();

            Vector3 spawnPos = filteredList[Random.Range(0, filteredList.Count)].position;

            BTEnemy enemy = PoolManager.Instance.Pop(_powerEnemyList[Random.Range(0, _powerEnemyList.Count)]) as BTEnemy;
            enemy.transform.SetParent(transform);
            enemy.Setting(spawnPos, Quaternion.identity);
            
            enemy.GetCompo<HealthCompo>().OnDieEvent.RemoveListener(() =>
            {
                _spawnEnemy.Remove(enemy);
                HandleDeadCheck();
            }); 
            
            enemy.GetCompo<HealthCompo>().OnDieEvent.AddListener(() =>
            {
                _spawnEnemy.Remove(enemy);
                HandleDeadCheck();
            });
            

            _spawnPowerEnemyCount++;
            _spawnEnemy.Add(enemy);
            yield return new WaitForSeconds(_spawnDelay);
        }
    }

    private void HandleDeadCheck()
    {
        _dieEnemyCount++;
        if (_dieEnemyCount >= _maxEnemyCount+_maxPowerEnemyCount)
        {
            Debug.Log("Afaf");
            levelClearEvent?.Invoke();
        }
    }

}
