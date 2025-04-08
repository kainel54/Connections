using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using YH.Enemy;
using YH.Entities;

public class SelfBombEnemyAnimator : EnemyAnimator
{
    private SelfBombEnemyAttackCompo _attackCompo;
    public int bombDisplayCnt { get; set; }
    [SerializeField] private MeshRenderer _bombMesh;

    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);
        _attackCompo = entity.GetCompo<SelfBombEnemyAttackCompo>();
        _attackCompo.OnBombEvent += BombEnabled;
    }

    public void OnDestroy()
    {
        _attackCompo.OnBombEvent -= BombEnabled;
    }

    public void SelfBombDisplaySetting()
    {
        bombDisplayCnt++;
        _attackCompo.SelfBombDisplaySetting();
        BombBlink();
    }

    private void BombBlink()
    {
        _bombMesh.material.DOColor(Color.red, "_EmissionColor", 0.2f)
            .SetLoops(-1, LoopType.Yoyo);
        _bombMesh.material.DOFloat(2, "_EmmisionColor", 0.2f);
    }

    public void BombEnabled(bool isEnabled)
    {
        _bombMesh.enabled = isEnabled;
    }
}
