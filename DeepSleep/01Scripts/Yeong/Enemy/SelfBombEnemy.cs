using UnityEngine;

public class SelfBombEnemy : DefaultEnemy
{
    private SelfBombEnemyAnimator _animator;

    protected override void Awake()
    {
        base.Awake();
        _animator = GetCompo<SelfBombEnemyAnimator>();
    }

    public override void Init()
    {
        base.Init();
        _animator.bombDisplayCnt = 0;
        _animator.BombEnabled(true);
    }
}
