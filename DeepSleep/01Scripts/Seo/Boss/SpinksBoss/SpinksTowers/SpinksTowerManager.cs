using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;

public class SpinksTowerManager : MonoBehaviour
{
    [SerializeField] private List<SpinksBossTower> _towers = new();
    [SerializeField] private Transform[] _tornadoSpawnPos = new Transform[28];
    [SerializeField] private Transform _spawnPointTrm;

    private SpinksBossArrow _spinksBossArrow;  
    private SpinksBossTower _selectedTower;

    private int _randIdx;
    
    [SerializeField] private GameEventChannelSO _soundChannelSO;
    [SerializeField] private SoundSO _raiseSound;

    private void Awake()
    {
        _spinksBossArrow = GetComponentInChildren<SpinksBossArrow>();
    }

    public void SettingTowers(BTEnemy enemy)
    {
        _towers.ForEach(x => x.SetBoss(enemy));
    }

    public void TopUpEvent()
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.position = transform.position;
        soundEvt.clipData = _raiseSound;
        _soundChannelSO.RaiseEvent(soundEvt);
        
        _towers.ForEach(x => x.HandleGotoPhase2Event());
    }

    public bool CanGetAliveTower()
    {
        if (_towers.Any(x => x.IsDie == false))// �Ѱ��� �������
            return true;

        return false;
    }

    public SpinksBossTower GetRandomAliveTower()
    {
        List<SpinksBossTower> aliveTowers = new(4);
        if (CanGetAliveTower() == false)
        {
            Debug.LogError("There is None of alive towers");
            return null;
        }


        foreach (SpinksBossTower _tower in _towers)
        {
            if (_tower.IsDie == false)
            {
                aliveTowers.Add(_tower);
            }
        }

        _selectedTower = aliveTowers[Random.Range(0, aliveTowers.Count)];



        return _selectedTower;
    }

    public SpinksBossTower GetCurrentTower()
    {
        return _selectedTower;
    }

    public void UsingSkills()
    {
        StartCoroutine(UseSkillCoroutine());
    }

    private IEnumerator UseSkillCoroutine()
    {
        while (true)
        {
            SpinksBossTower tower = GetRandomAliveTower();
            if (tower == null)
            {
                break;
            }
            tower.UseSkill();
            yield return new WaitForSeconds(1f);
        }
    }

    public Vector3 GetTornadoRandomPos()
    {
        _randIdx = Random.Range(0, 28);

        return _tornadoSpawnPos[_randIdx].transform.position;
    }

    public Vector3 GetTornadoDirection()
    {
        return (_spawnPointTrm.position - _tornadoSpawnPos[_randIdx].transform.position).normalized;
    }

    public void CanAttackToTower()
    {
        Debug.Log("== CanAttackToTower Called ==");
        Debug.Log("This script's GameObject: " + gameObject.name);
        Debug.Log("activeSelf: " + gameObject.activeSelf);
        Debug.Log("activeInHierarchy: " + gameObject.activeInHierarchy);

        _selectedTower.SetCanAttack(true);
        _spinksBossArrow.gameObject.SetActive(true);
        _spinksBossArrow.SetMove(_selectedTower.transform.position);
    }

    public void CantAttackToTower()
    {
        _selectedTower.SetCanAttack(false);
        _spinksBossArrow.StopMove();
        _spinksBossArrow.gameObject.SetActive(false);
    }
}
