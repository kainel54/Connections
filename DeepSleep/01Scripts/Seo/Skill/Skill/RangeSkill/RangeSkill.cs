using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class RangeSkill : Skill
{
    [SerializeField] private SkillRangeObj skillRangeObj;

    private RangeSkillDataSO _rangeDataSO;
    private GenericSkillDataSO _genericDataSO;
    public override void UseSkill(Transform shootTrm)
    {
        base.UseSkill(shootTrm);
        if (shootCount <= 0)
            SetCoolTime();
        _rangeDataSO = GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO;
        _genericDataSO = GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;

        StartCoroutine(RangeAttack(shootTrm));
    }

    private IEnumerator RangeAttack(Transform shootTrm)
    {


        for (int j = 1; j <= _genericDataSO!.attackCount; j++)
        {

            for (int i = 0; i < _rangeDataSO!.skillObjCreateCount; i++)
            {
                float startAngle = 360 / _rangeDataSO!.skillObjCreateCount * (i + 1);

                SkillRangeObj range = Instantiate(skillRangeObj, shootTrm.position + new Vector3(0, 1.1f, 0), Quaternion.identity);


                Quaternion rotation = Quaternion.Euler(0, startAngle, 0);

                Vector3 playerAngleSet = rotation * shootTrm.forward;
                range.transform.position = shootTrm.position + playerAngleSet;
                range.transform.forward = playerAngleSet.normalized;
                range.Initialize(this, j);
            }
            yield return new WaitForSeconds(_genericDataSO.reShootTime);
        }
    }
}
