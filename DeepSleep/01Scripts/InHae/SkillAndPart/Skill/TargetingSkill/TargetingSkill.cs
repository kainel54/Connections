using System.Collections;
using UnityEngine;

public class TargetingSkill : Skill
{
    private RangeSkillDataSO _rangeDataSO;
    private GenericSkillDataSO _genericDataSO;
    private TargetingSkillDataSO _targetingDataSO;

    private Vector3 _direction;
    private Vector3 _playerInputPos;

    protected override void Awake()
    {
        base.Awake();
        SkillInputAction += HandleInputPosition;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        SkillInputAction -= HandleInputPosition;
    }

    private void HandleInputPosition()
    {
        _targetingDataSO = GetSkillData(SkillFieldDataType.Targeting) as TargetingSkillDataSO;
        _rangeDataSO = GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO;
        _genericDataSO = GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO;
        
        float skillRange = _targetingDataSO.canUseSkillRangeStat.currentValue;
        _playerInputPos = GetSkillPlacementPosition(player.transform, skillRange);
    }

    public override void UseSkill(Transform shootTrm)
    {
        base.UseSkill(shootTrm);
        StartCoroutine(ShootingSkill(shootTrm));
    }

    private IEnumerator ShootingSkill(Transform shootTrm)
    {
        PlaySound();

        float attackCount = _genericDataSO.attackCountStat.currentValue;
        float objCount = _rangeDataSO.rangeObjCountStat.currentValue;

        for (int i = 0; i < attackCount; i++)
        {
            if (player.transform != shootTrm)
                _playerInputPos = shootTrm.position;

            for (int j = 0; j < objCount; j++)
            {
                float angle = 360f / objCount * j;
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 forward = rotation * player.transform.forward;

                SkillTargetingObj skillObj = PoolManager.Instance.Pop(_popSkillObj.PoolEnum) as SkillTargetingObj;

                if (skillObj != null)
                {
                    skillObj.transform.position = _playerInputPos;
                    skillObj.transform.forward = forward.normalized;
                    skillObj.Initialize(this, shootTrm);
                }
            }

            yield return new WaitForSeconds(_genericDataSO.reShootTimeStat.currentValue);
        }
    }

    private Vector3 GetSkillPlacementPosition(Transform playerTransform, float maxDistance)
    {
        // xz평면에서의 마우스 위치 확인 (y = 0)
        Plane groundPlane = new Plane(Vector3.up, playerTransform.position);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!groundPlane.Raycast(mouseRay, out float enter))
            return playerTransform.position;

        // xz평면과 마우스 포인트 위치로 쏜 레이의 교차지점 (목표 위치)
        Vector3 mouseWorldPoint = mouseRay.GetPoint(enter);
        
        Vector3 dir = (mouseWorldPoint - playerTransform.position).normalized;
        float distance = Mathf.Min(maxDistance, Vector3.Distance(playerTransform.position, mouseWorldPoint));
        Vector3 desiredPos = playerTransform.position + dir * distance;

        Vector3 rayStart = desiredPos + Vector3.up * 3f;
        if (Physics.Raycast(rayStart, Vector3.down, out RaycastHit hit, 10f, whatIsGround))
            return hit.point;

        Vector3 offsetDir = (playerTransform.position - desiredPos).normalized;
        desiredPos.y -= 0.2f;

        if (Physics.Raycast(desiredPos, offsetDir, out RaycastHit groundHit, 
                Mathf.Infinity, whatIsGround))
        {
            Vector3 fallbackPos = groundHit.point;
            fallbackPos.y += 0.2f;
            return fallbackPos;
        }

        return desiredPos;
    }
}