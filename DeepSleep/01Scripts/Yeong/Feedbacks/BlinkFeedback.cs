using System;
using System.Collections;
using UnityEngine;

namespace YH.Feedbacks
{
    public class BlinkFeedback : Feedback
    {
        [SerializeField] private SkinnedMeshRenderer _targetRenderer;
        [SerializeField] private float _delaySecond;
        [SerializeField] private float _blinkValue;
        
        private readonly int _blinkShaderParam = Shader.PropertyToID("_BlinkValue");
        private readonly int _alphaColorParam = Shader.PropertyToID("_Color");

        private Material _blinkMaterial;
        private Coroutine _delayCoroutine;

        private void Awake()
        {
            _blinkMaterial = _targetRenderer.material;
        }

        public override void CreateFeedback()
        {
            FinishFeedback();
            _blinkMaterial.SetFloat(_blinkShaderParam, _blinkValue);
            _delayCoroutine = StartCoroutine(SetToNormalAfterDelay());
        }

        private IEnumerator SetToNormalAfterDelay()
        {
            yield return new WaitForSeconds(_delaySecond);
            FinishFeedback();
        }

        public override void FinishFeedback()
        {
            if(_delayCoroutine != null)
                StopCoroutine(_delayCoroutine);
            
            _blinkMaterial.SetFloat(_blinkShaderParam, 0);
        }

        public void StopDelayCorutine()
        {
            if(_delayCoroutine != null)
            {
                StopCoroutine(_delayCoroutine);
            }
        }
    }
}
