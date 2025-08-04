using UnityEngine;

public class SkillDamageCaster : MonoBehaviour
{
    protected SkillDamageCasterParent _skillDamageCasterParent;

    private void Awake()
    {
        _skillDamageCasterParent = GetComponentInParent<SkillDamageCasterParent>();
    }

    public virtual void Init(SkillObj skillObj, SkillDamageCasterParent skillDamageCasterParent)
    {
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (_skillDamageCasterParent.IsOnceCheck)
        {
            _skillDamageCasterParent.ApplyDamage(other);
            if(_skillDamageCasterParent.IsOnceCheck)
                _skillDamageCasterParent.CasterEnable(false);
        }
        else
        {
            _skillDamageCasterParent.StayCastDamageCheck(other);
        }
    }
}
