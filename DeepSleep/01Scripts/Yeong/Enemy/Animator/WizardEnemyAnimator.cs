using DG.Tweening;
using ObjectPooling;
using UnityEngine;
using YH.Entities;
using YH.EventSystem;

namespace YH.Enemy
{
    public class WizardEnemyAnimator : EnemyAnimator
    {
        private WizardEnemyAttackCompo _attackCompo;
        [SerializeField] private PoolingItemSO _effectItem;
        [SerializeField] private GameEventChannelSO _spawnEvent;
        private SkinnedMeshRenderer _meshRender;

        private readonly int _blinkShaderParam = Shader.PropertyToID("_BlinkValue");
        private readonly int _blinkColorShaderParam = Shader.PropertyToID("_BlinkColor");
        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);
            _attackCompo = entity.GetCompo<WizardEnemyAttackCompo>();
            _movement.DodgeAction += HandleDodgeEvent;
            _meshRender = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        private void HandleDodgeEvent(bool isDodge)
        {
            if (isDodge)
            {
                _meshRender.enabled = false;
                var evt = SpawnEvents.EffectSpawn;
                evt.position = _entity.transform.position;
                evt.rotation = _entity.transform.rotation;
                evt.scale = _entity.transform.localScale;
                evt.effectItem = _effectItem;
                _spawnEvent.RaiseEvent(evt);

            }
            else
            {
                _meshRender.material.SetFloat(_blinkShaderParam, 1);
                _meshRender.enabled = true;
                _meshRender.material.DOFloat(0, _blinkShaderParam, 0.5f);
            }
        }

        public void FireEvent() => _attackCompo.FireBall();

        public void BeamEvent() => _attackCompo.Beam();
        public void BeamSetting()=>_attackCompo.BeamSetting();
    }
}

