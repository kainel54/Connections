using YH.Animators;
using YH.Entities;

namespace YH.FSM
{
    public abstract class EntityState
    {
        protected Entity _entity;

        protected AnimParamSO _animParam;
        protected bool _isTriggerCall;

        protected EntityRenderer _renderer;
        protected EntityAnimationTrigger _animTrigger;
        public EntityState(Entity entity, AnimParamSO animParam)
        {
            _entity = entity;
            _animParam = animParam;
            _renderer = _entity.GetCompo<EntityRenderer>();
            _animTrigger = _entity.GetCompo<EntityAnimationTrigger>(true);
        }

        public virtual void Enter()
        {
            _renderer.SetParam(_animParam, true);
            _isTriggerCall = false;
            _animTrigger.OnAnimationEndTrigger += AnimationEndTrigger;
        }

        public virtual void Update() { }

        public virtual void Exit()
        {
            _renderer.SetParam(_animParam, false);
            _animTrigger.OnAnimationEndTrigger -= AnimationEndTrigger;
        }

        public virtual void AnimationEndTrigger()
        {
            _isTriggerCall = true;
        }
    }
}
