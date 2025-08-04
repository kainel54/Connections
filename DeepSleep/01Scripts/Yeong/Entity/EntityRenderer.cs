using System;
using YH.Animators;
using UnityEngine;

namespace YH.Entities
{
    public class EntityRenderer : MonoBehaviour, IEntityComponent
    {
        private Entity _entity;
        private Animator _animator;
        
        public void Initialize(Entity entity)
        {
            _entity = entity;
            _animator = GetComponent<Animator>();
        }

        public void SetParam(AnimParamSO param, bool value) => _animator.SetBool(param.hashValue, value);
        public void SetParam(AnimParamSO param, float value) => _animator.SetFloat(param.hashValue, value);
        public void SetParam(AnimParamSO param, int value) => _animator.SetInteger(param.hashValue, value);
        public void SetParam(AnimParamSO param) => _animator.SetTrigger(param.hashValue);

        public void Play(string clipName)
        {
            _animator.Play(clipName);
        }
        
        public void Play(AnimationClip clip)
        {
            _animator.Play(clip.name);
        }

        public void SetSpeed(float speed)
        {
            _animator.speed = speed;
        }

        public void FadeSprite(float time, Action CompleteCallback = null)
        {
        }
    }
}
