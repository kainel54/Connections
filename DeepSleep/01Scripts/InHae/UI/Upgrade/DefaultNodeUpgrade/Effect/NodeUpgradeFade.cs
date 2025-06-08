using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace IH.UI
{
    public class NodeUpgradeFade : MonoBehaviour
    {
        [SerializeField] private GameObject _checkButton;
        
        private Image _image;
        private Material _material;
        
        private readonly int _circleValueHash = Shader.PropertyToID("_CircleValue");
        private readonly int _fadeValueHash = Shader.PropertyToID("_FadeValue");
        private readonly int _fadeModeHash = Shader.PropertyToID("_FadeMode");

        private void Awake()
        {
            _image = GetComponent<Image>();
            _material = _image.material;
        }

        public void StartFade()
        {
            _material.SetInt(_fadeModeHash, 0);
            _material.SetFloat(_fadeValueHash, 1f);
            _material.SetFloat(_circleValueHash, 0f);

            _material.DOFloat(3f, _circleValueHash, 0.4f).OnComplete(() 
                => StartCoroutine(FadeOut()));
        }

        private IEnumerator FadeOut()
        {
            _checkButton.SetActive(true);
            
            yield return new WaitForSeconds(0.5f);
            _material.SetInt(_fadeModeHash, 1);
            _material.DOFloat(0f, _fadeValueHash, 0.5f);
        }
    }
}
