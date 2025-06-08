using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpinksTowerManager : MonoBehaviour
{
    [SerializeField] private SpinksBossArrow _spinksBossArrow;  //TODO_SE

    [SerializeField] private List<SpinksBossTower> _towers = new();
    [SerializeField] private Transform[] _tornadoSpawnPos = new Transform[28];
    [SerializeField] private Transform _spawnPointTrm;

    private SpinksBossTower _selectedTower;

    private int _randIdx;

    public void SettingTowers(BTEnemy enemy)
    {
        _towers.ForEach(x => x.SetBoss(enemy));
    }

    public void TopUpEvent()
    {
        _towers.ForEach(x => x.HandleGotoPhase2Event());
    }


    public bool CanGetAliveTower()
    {
        if (_towers.Any(x => x.IsDie == false))// 한개라도 살아있음
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


        foreach (SpinksBossTower _tower in _towers)  //TOFIX_SE
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
        _selectedTower.SetCanAttack(true);
    }
    public void CantAttackToTower()
    {
        _selectedTower.SetCanAttack(false);
    }
}
