using DG.Tweening;
using System.Collections;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.Combat;
using YH.Entities;
using YH.EventSystem;
using YH.StatSystem;

public class BombPartSkillObject : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _soundEventChannelSo;
    [SerializeField] private SoundSO _readySound;
    [SerializeField] private SoundSO _bombSound;
    
    public GameObject bombObject;
    private Skill _usingSkill;

    private float _range;
    private float _skillActiveDelay;
    private float _bombDamage;
    private float _throwSpeed;

    public void InitializeSkill(Skill skill)
    {
        PlaySound(_readySound);
        
        _usingSkill = skill;
        _range = (_usingSkill.GetSkillData(SkillFieldDataType.Range) as RangeSkillDataSO)
            .rangeAttackSizeStat.currentSphereValue;
        
        _skillActiveDelay = (_usingSkill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO).skillActiveDelay;
        
        _bombDamage = (_usingSkill.GetSkillData(SkillFieldDataType.Generic) as GenericSkillDataSO)
            .attackDamageStat.currentValue * 1.5f;
        
        _throwSpeed = 4f;

        transform.DOJump(transform.position + transform.forward * _throwSpeed, _throwSpeed / 2, 1, 1);
        StartCoroutine(DelayBomb());
    }

    private IEnumerator DelayBomb()
    {
        yield return new WaitForSeconds(_skillActiveDelay);

        PlaySound(_bombSound);
        
        Instantiate(bombObject, transform.position, Quaternion.identity);

        RaycastHit[] hitarr = Physics.SphereCastAll(transform.position, _range / 2, transform.forward, 0, _usingSkill.player.whatIsEnemy);

        Entity entity = _usingSkill.player as Entity;
        EntityStat statCompo = entity.GetCompo<EntityStat>();
        foreach (RaycastHit hit in hitarr)
        {
            if (hit.collider.TryGetComponent<IDamageable>(out IDamageable damageAbleComponent))
            {
                HitData hitData = new HitData(entity, _bombDamage, 0, 0);
                damageAbleComponent.ApplyDamage(hitData);
            }
        }

        _usingSkill.UseSkill(transform);
        gameObject.SetActive(false);
    }

    private void PlaySound(SoundSO clip)
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.position = transform.position;
        soundEvt.clipData = clip;
        _soundEventChannelSo.RaiseEvent(soundEvt);
    }
}
