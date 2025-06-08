using System.Collections.Generic;
using UnityEngine;
using YH.Combat;

public class Arrow : SkillProjectileObj
{
    [SerializeField] private float _damageInterval = 1;

    private Dictionary<Collider, float> _lastDamageTime = new Dictionary<Collider, float>();

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Tower"))
        {
            if (_lastDamageTime.TryGetValue(other, out float lastTime))
            {
                if (Time.time - lastTime >= _damageInterval)
                {
                    ApplyDamage(other);
                }
            }
            else
            {
                ApplyDamage(other);
            }
        }
    }

    private void ApplyDamage(Collider collider)
    {
        // todo: apply damage
        ImpactEffectPlay();
        if (collider.gameObject.TryGetComponent(out IDamageable damageable))
        {
            ImpactEffectPlay();
            damageable.ApplyDamage(GetHitData());
        }

        if (_penetrationCount <= _currentPenetrationCount)
            CallDestroyEvent();
        else
            _currentPenetrationCount++;

        // todo: apply other effects like slow, stun, etc.
        _lastDamageTime[collider] = Time.time; // ��ųʸ��� �ݶ��̴��� ������ �ð��� ����
                                               // todo: push this object (object pooling)
    }
}
