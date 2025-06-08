using DG.Tweening;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Events;
using YH.Combat;
using YH.Enemy;
using YH.StatSystem;

public class SpinksBossTower : MonoBehaviour, IDamageable
{
    [SerializeField] private AnimatorOverrideController _overrideAnimation;

    public UnityEvent OnDieEvent;

    public bool CanAttack = false;
    public bool IsDie { get; private set; } = false;

    private float _health = 150f;

    protected BTEnemy _enemy;

    protected SpinksEnemyAttackCompo _attackCompo;

    private Transform _objTopTrm;
    private void Start()
    {
        _objTopTrm = transform.Find("TopTrm").transform;
    }

    public void SetBoss(BTEnemy enemy)
    {
        _enemy = enemy;
        _attackCompo = _enemy.GetCompo<SpinksEnemyAttackCompo>();
    }

    public void HandleGotoPhase2Event()
    {
        transform.DOLocalMoveY(6, 2);
    }

    public Vector3 GetTowerPos()
    {
        return _objTopTrm.position;
    }


    public virtual void UseSkill()
    {

    }

    public void ApplyDamage(HitData hitData, bool isChangeVisible = true, bool isTextVisible = true, float damageDecrease = 1)
    {
        if (IsDie) return;
        if (!CanAttack) return;

        bool isCritical = false;

        float damage = hitData.damage;
        float random = Random.Range(0f, 100f);

        //damage = 100 / (100 + statCompo.GetElement("Defense").Value) * damage;
        //damage = damage * Mathf.Log(damage / statCompo.GetElement("Defense").Value * 10);

        if (random < hitData.ciriticalChance)
        {
            isCritical = true;
            damage *= (hitData.ciriticalDamage / 100);
        }

        float prev = _health;
        _health -= damage;
        if (_health < 0)
            _health = 0;

        if (isTextVisible)
        {
            DamageText damageText = PoolManager.Instance.Pop(UIPoolingType.DamageText) as DamageText;
            damageText.Setting((int)damage, isCritical, transform.position);
        }

        if (_health == 0)
            Die();
    }

    private void Die()
    {
        if (IsDie) return;
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DOShakePosition(1.5f, .6f));
        seq.Append(transform.DOLocalMoveY(-8, 2));

        IsDie = true;

        OnDieEvent?.Invoke();
    }

    public AnimatorOverrideController GetOverrideAnimatior()
    {
        return _overrideAnimation;
    }

    public void SetCanAttack(bool canAttack)
    {
        CanAttack = canAttack;
    }
}
