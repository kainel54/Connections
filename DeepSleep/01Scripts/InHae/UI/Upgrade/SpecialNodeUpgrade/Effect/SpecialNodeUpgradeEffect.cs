using DG.Tweening;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;
using Random = UnityEngine.Random;

public class SpecialNodeUpgradeEffect : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _soundChannelSO;
    [SerializeField] private SoundSO _hammerImpactSound;
    
    [SerializeField] private Image _hammer;
    [SerializeField] private Transform _hammerReadyTrm;

    [SerializeField] private float _upSpeed;
    [SerializeField] private float _downSpeed;
    [SerializeField] private float _shakeValue;
    
    [SerializeField] private ParticleSystem _hitParticle;
    [SerializeField] private ParticleSystem _lastHitParticle;
    
    private SpecialUpgradeCheckPanel _specialUpgradeCheckPanel;
    private GameObject _checkButton;

    private Vector3 _hammerInitPos;
    private Vector3 _hammerInitRot;

    private void Awake()
    {
        _specialUpgradeCheckPanel = GetComponent<SpecialUpgradeCheckPanel>();
        _checkButton = transform.Find("Buttons/CheckButton").gameObject;

        _specialUpgradeCheckPanel.UpgradeEvent += UpgradeCheckTween;
        
        _hammerInitPos = _hammer.transform.position;
        _hammerInitRot = _hammer.transform.localEulerAngles;
    }

    private void OnDestroy()
    {
        _specialUpgradeCheckPanel.UpgradeEvent -= UpgradeCheckTween;
    }

    private void UpgradeCheckTween()
    {
        Color color = _hammer.color;
        color.a = 1;
        _hammer.color = color;
        
        Sequence sequence = DOTween.Sequence();
        
        for (int i = 0; i < 3; i++)
        {
            Vector3 rot = _hammerInitRot;
            rot.z += 50f;
            
            sequence.Append(_hammer.transform.DOMove(_hammerReadyTrm.position, _upSpeed));
            sequence.Join(_hammer.transform.DORotate(rot, _upSpeed));
            
            sequence.AppendInterval(0.05f);

            rot = _hammerInitRot;

            var captureI = i;
            sequence.Append(_hammer.transform.DOMove(_hammerInitPos, _downSpeed).SetEase(Ease.InBack).OnComplete(() =>
            {
                if(captureI == 2)
                    _lastHitParticle.Play();
                else
                    _hitParticle.Play();

                HammerImpactSound();
            }));
            sequence.Join(_hammer.transform.DORotate(rot, _downSpeed));
            
            Vector2 randomStrength = new Vector2(Random.Range(-_shakeValue, _shakeValue), 
                Random.Range(-_shakeValue, _shakeValue));

            if (i == 2)
                randomStrength *= 4f;
            
            sequence.Append(transform.DOShakePosition(0.3f, randomStrength));
            sequence.AppendInterval(0.2f);
        }

        sequence.AppendInterval(0.3f);
        sequence.OnComplete(() =>
        {
            _hammer.DOFade(0, 0.2f);
            _checkButton.gameObject.SetActive(true);
        });
    }
    private void HammerImpactSound()
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.clipData = _hammerImpactSound;
        soundEvt.position = transform.position;
        _soundChannelSO.RaiseEvent(soundEvt);
    }
}
