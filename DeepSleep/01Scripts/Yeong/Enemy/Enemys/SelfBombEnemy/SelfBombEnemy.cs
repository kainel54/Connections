using UnityEngine;
using YH.Enemy;

public class SelfBombEnemy : DefaultEnemy
{
    private SelfBombEnemyAnimator _animator;
    private SelfBombEnemyAttackCompo _attackCompo;
    [SerializeField] private GameObject _bombObject;
    protected override void Awake()
    {
        base.Awake();
        _animator = GetCompo<SelfBombEnemyAnimator>();
        _attackCompo = GetCompo<SelfBombEnemyAttackCompo>();
    }

    public override void OnPop()
    {
        base.OnPop();
        _animator.bombDisplayCnt = 0;
        _animator.BombEnabled(true);
        SetBombActive(true);
        _attackCompo.radius = 2.5f;
    }

    public override void OnPush()
    {
        base.OnPush();
        if(_attackCompo.GetDisplay() != null)
        {
            _attackCompo.SetDisplay(null);
        }
    }

    public override void OnDie()
    {
        base.OnDie();
    }

    public void SetBombActive(bool isActive)
    {
        _bombObject.SetActive(isActive);
    }
}
