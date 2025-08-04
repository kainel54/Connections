using YH.Enemy;
using YH.Entities;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.SoundEvent;
using YH.Core;
using YH.EventSystem;
using YH.Players;
public class SpinksEnemyAnimator : EnemyAnimator
{
    [SerializeField] private GameEventChannelSO _soundEventSo;
    [SerializeField] private SoundSO _slashSound;
    [SerializeField] private SoundSO _stingSound;
    [SerializeField] private SoundSO _dashSound;
    [SerializeField] private SoundSO _jumpSound;
    [SerializeField] private SoundSO _fallSound;
    [SerializeField] private SoundSO _projectileSound;

    private SpinksEnemyAttackCompo _attackCompo;

    private SpinksTowerManager _spinksTowerManager;
    private GameObject _rootObj, _characterObj;
    public override void Initialize(Entity entity)
    {
        base.Initialize(entity);

        _attackCompo = entity.GetCompo<SpinksEnemyAttackCompo>();
        _attackCompo.SetBossLevel();
        _spinksTowerManager = _attackCompo.GetEnemyBossLevel().GetComponentInChildren<SpinksTowerManager>();
        _spinksTowerManager.SettingTowers(_entity as BTEnemy);

        _attackCompo.SetTowerManager(_spinksTowerManager);

        _rootObj = transform.Find("Root").gameObject;
        _characterObj = transform.Find("Character").gameObject;
    }


    #region BasicSkill
    public void Attack3WaysMagic() => _attackCompo.MagicAttack();

    public void RushSlice()
    {
        _attackCompo.RushSliceAttack();
    }
    public void FallAttack() => _attackCompo.FallAttack();
    public void AttackRangeShower() => _attackCompo.FallAttackShowingRANGE();
    #endregion

    #region ForTowerSkill
    public void FallMagicSkill() => _attackCompo.FallMagicAttack();
    public void GiveBuff() => _attackCompo.BuffGive();
    public void SpawnSandTornado() => _attackCompo.SpawnSandTornado();
    public void SpawnEnemy() => _attackCompo.SpawnEnemy();

    #endregion

    #region BossPhase
    public void GotoPhase2() => _attackCompo.GoToPhase2();

    public void GoToPhase3() => _attackCompo.GoToPhase3();

    public void GoToDaed() => _attackCompo.GoToDead();

    #endregion

    #region Moving
    public void ForceMoveToTower()
    {
        Vector3 moveToPos = _spinksTowerManager.GetRandomAliveTower().GetTowerPos();
        _movement.ForceMove(moveToPos);
        transform.localPosition = Vector3.zero;
    }

    public void ForceMoveToFloor()
    {
        Vector3 moveToPos = _attackCompo.GetSpawnPoint();
        _movement.ForceMove(moveToPos);
        transform.localPosition = Vector3.zero;
    }

    public void ForceMoveToTarget(Vector3 targetPos)
    {
        Vector3 moveToPos = targetPos;
        _movement.ForceMove(moveToPos);
    }

    public void GimicOfPattern1()
    {
        _rootObj.SetActive(true);
        _characterObj.SetActive(true);
    }

    public void JumpUp()
    {
        _movement.NavMeshEnable(false);
        _rootObj.SetActive(false);
        _characterObj.SetActive(false);
    }

    public void StingSound()
    {
        PlaySound(_stingSound);
    }


    public void SlashSound()
    {
        PlaySound(_slashSound);
    }


    public void JumpSound()
    {
        PlaySound(_jumpSound);
    }


    public void LandingSound()
    {
        PlaySound(_fallSound);
    }


    public void ProjectileSound()
    {
        PlaySound(_projectileSound);
    }

    public void SetDizzing() => _attackCompo.SetDizz();



    #endregion

    private void PlaySound(SoundSO sound)
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.clipData = sound;
        soundEvt.position = transform.position;
        _soundEventSo.RaiseEvent(soundEvt);
    }
}
