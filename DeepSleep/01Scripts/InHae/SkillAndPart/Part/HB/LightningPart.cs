using System;
using System.Collections.Generic;
using UnityEngine;
using YH.Combat;
using YH.Players;

public class LightningPart : SkillPart, ILightningPart
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private GameObject _lightningPrefab;

    public void LightningEquip()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            if (data.canLightning == false)
            {
                data.canLightning = true;
                _skill.PressAction += HandleLightning;
            }

            data.lightningCount += 1;

            Debug.Log($"----- Lightning Equip -----");
            Debug.Log($"can lightning: {data.canLightning}");
            Debug.Log($"lightning count: {data.lightningCount}");
            Debug.Log($"---------------------------");
        }
    }

    public void LightningUnEquip()
    {
        if (_skill.GetSkillData(SkillFieldDataType.Generic) is GenericSkillDataSO data)
        {
            if (data.lightningCount > 0)
                data.lightningCount -= 1;
            if (data.lightningCount == 0)
            {
                data.canLightning = false;
                _skill.PressAction -= HandleLightning;
            }

            Debug.Log($"----- Lightning UnEquip -----");
            Debug.Log($"can lightning: {data.canLightning}");
            Debug.Log($"lightning count: {data.lightningCount}");
            Debug.Log($"-----------------------------");
        }
    }

    private void HandleLightning()
    {
        print("lightning part on");

        _skill.SetCoolTime();

        List<BTEnemy> spawnedEnemy = _skill.player.GetEnemies();
        List<Tuple<BTEnemy, float>> enemyDistances = new();

        foreach (BTEnemy enemy in spawnedEnemy)
        {
            if (enemy.TryGetComponent<IBossComponent>(out IBossComponent bossComp))
                continue;

            float distance = Vector3.Distance(enemy.transform.position, _playerManagerSO.PlayerTrm.position);
            enemyDistances.Add(new Tuple<BTEnemy, float>(enemy, distance));
        }

        enemyDistances.Sort((a, b) => a.Item2.CompareTo(b.Item2));

        int lightningCount = (_skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).lightningCount;

        for (int i = 0; i < Mathf.Min(lightningCount, enemyDistances.Count); i++)
        {
            BTEnemy targetEnemy = enemyDistances[i].Item1;

            GameObject lightningObj = Instantiate(_lightningPrefab, targetEnemy.transform.position, Quaternion.identity);
            // 사운드 재생

            EntityHealth enemyHealthCompo = targetEnemy.GetCompo<EntityHealth>();

            float damage = Mathf.Max(enemyHealthCompo.Health * 0.2f, 1);
            HitData hitData = new HitData(_playerManagerSO.Player, damage, 0, 0);

            enemyHealthCompo.ApplyDamage(hitData, true, true, 1);
        }
    }
}