using System.Collections;
using System.Linq;
using UnityEngine;
using YH.Combat;
using YH.Core;
using YH.Entities;
using YH.StatSystem;

public class SwordDown : SkillRangeObj
{
    private float _width;
    private float _height;

    private RangeSkillDataSO _rangeData;
    private GenericSkillDataSO _genericData;

    private LayerMask _enemyMask;
    private RaycastHit[] enemies = new RaycastHit[50];

    public override void Initialize(Skill _skill, Transform shootTrm)
    {
        base.Initialize(_skill, shootTrm);

        _rangeData = (skill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO);
        _genericData = (skill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO);
        _enemyMask = skill.whatIsEnemy;

        _width = _rangeData.rangeAttackSizeStat.currentWidthValue;
        _height = _rangeData.rangeAttackSizeStat.currentHeightValue;

        //transform.localScale = new Vector3(shootCount, shootCount, shootCount);  << It Can be another Part

        OnSkillDestroyEvent += DestroyAction;

        StartCoroutine(AttackCoroutine());
    }

    private void DestroyAction()
    {
        DestroyObject(this.gameObject, 1.5f);
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(_genericData.skillDamageDelay);
        ApplyDamage();
    }

    private void ApplyDamage()
    {
        int count = Physics.BoxCastNonAlloc(transform.position, new Vector3(_width, 1, 0) / 2, transform.forward, enemies, transform.rotation,/* shootCount * */_height, _enemyMask);

        for (int i = 0; i < count; i++)
        {
            if (enemies[i].collider.gameObject.TryGetComponent(out IDamageable damageable))
            {

                damageable.ApplyDamage(GetHitData());
                CameraManager.Instance.ShakeCamera(2, 2, .3f);
            }
        }

        CallDestroyEvent();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(_width, 1,_height));
    }

    private void OnDestroy()
    {
        OnSkillDestroyEvent -= DestroyAction;
    }
}
