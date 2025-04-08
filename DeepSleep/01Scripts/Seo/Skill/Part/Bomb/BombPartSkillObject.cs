using DG.Tweening;
using System.Collections;
using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.StatSystem;

public class BombPartSkillObject : MonoBehaviour
{
    public GameObject bombObject;
    private Skill _usingSkill;

    private float _range;
    private float _skillActiveDelay;
    private float _bombDamage;
    private float _throwSpeed;

    public void InitializeSkill(Skill skill)
    {
        _usingSkill = skill;
        _range = (_usingSkill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO).rangeSize;
        _skillActiveDelay = (_usingSkill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).skillActiveDelay;
        _bombDamage = (_usingSkill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).damage * 1.5f;
        _throwSpeed = (_usingSkill.GetSkillData(SkillFieldDataType.Projectile) as ProjectileSkillDataSO).projMoveSpeed;

        transform.DOJump(transform.position + transform.forward * _throwSpeed, _throwSpeed / 2, 1, 1);
        StartCoroutine(DelayBomb());
    }

    private IEnumerator DelayBomb()
    {
        yield return new WaitForSeconds(_skillActiveDelay);

        Instantiate(bombObject, transform.position, Quaternion.identity);

        RaycastHit[] hitarr = Physics.SphereCastAll(transform.position, _range / 2, transform.forward, 0, _usingSkill.player.whatIsEnemy);

        Entity entity = _usingSkill.player as Entity;
        StatCompo statCompo = entity.GetCompo<StatCompo>();
        foreach (RaycastHit hit in hitarr)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageAbleComponent))
            {
                damageAbleComponent.ApplyDamage(statCompo, _bombDamage);
            }
        }

        _usingSkill.UseSkill(transform);
        gameObject.SetActive(false);
    }
    /*
      _duration = (_usingSkill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).skillActiveDuration;
        _reShootTime = (_usingSkill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).reShootTime;

    if (_duration <= 0)
            Destroy(gameObject);

        _duration -= Time.deltaTime;
        _reShootTime -= Time.deltaTime;

        if (_reShootTime <= 0)
        {
            _usingSkill.UseSkill(transform);
            _reShootTime = _resetShootTime;
        }
    */
}
