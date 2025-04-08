using System.Collections;
using System.Collections.Specialized;
using UnityEngine;
using UnityEngine.Serialization;
using YH.Players;

public class TargetingSkill : Skill
{
    [FormerlySerializedAs("targeting")][SerializeField] private SkillTargetingObj skilltargetingObj;

    private RangeSkillDataSO _rangeDataSO;
    private GenericSkillDataSO _genericDataSO;
    private TargetingSkillDataSO _targetingDataSO;
    private Vector3 _direction;
    private Vector3 playerInputPos;
    public override void UseSkill(Transform shootTrm)
    {

        _targetingDataSO = GetSkillData(SkillFieldDataType.Targeting) as TargetingSkillDataSO;
        _rangeDataSO = GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO;
        _genericDataSO = GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;

        playerInputPos = player.PlayerInput.GetWorldMousePosition();

        if (Vector3.Distance(playerInputPos, shootTrm.position) > _targetingDataSO.canUseSkillRange)
        {
            return;
        }
        base.UseSkill(shootTrm);
        if (shootCount <= 0)
            SetCoolTime();
        StartCoroutine(ShootingSkill(shootTrm));
        // todo use skill start at mouse cursor point and same as other skill
    }

    private IEnumerator ShootingSkill(Transform shootTrm)
    {

        for (int j = 1; j <= _genericDataSO!.attackCount; j++)
        {

            for (int i = 0; i < _rangeDataSO!.skillObjCreateCount; i++)
            {
                float startAngle = 360 / _rangeDataSO!.skillObjCreateCount * (i + 1);
                SkillTargetingObj targetobj = Instantiate(skilltargetingObj, shootTrm.position + new Vector3(0, 1.1f, 0), Quaternion.identity);


                if (Physics.Raycast(playerInputPos, Vector3.down, out RaycastHit hit, Mathf.Infinity, whatIsGround))
                {
                    Debug.Log(hit.collider.gameObject.name);
                    playerInputPos = hit.point;
                }



                Quaternion rotation = Quaternion.Euler(0, startAngle, 0);

                Vector3 playerAngleSet = Vector3.zero; // rotation * shootTrm.forward;
                targetobj.transform.position = playerInputPos + playerAngleSet;
                targetobj.transform.forward = playerAngleSet.normalized;
                targetobj.Initialize(this);
            }
            yield return new WaitForSeconds(_genericDataSO.reShootTime);
        }
    }
}
