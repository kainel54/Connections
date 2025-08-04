using DG.Tweening;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;
using Random = UnityEngine.Random;

public abstract class NodeUpgradeEffectBase : MonoBehaviour
{
    [SerializeField] protected GameEventChannelSO _soundChannelSO;
    [SerializeField] protected SoundSO _hammerImpactSound;
    
    [SerializeField] protected Image _hammer;
    [SerializeField] protected Transform _hammerReadyTrm;

    [SerializeField] protected float _upSpeed;
    [SerializeField] protected float _downSpeed;
    [SerializeField] protected float _shakeValue;

    [SerializeField] protected ParticleSystem _hitParticle;
    [SerializeField] protected ParticleSystem _lastHitParticle;
    
    protected GameObject _checkButton;

    protected Vector3 _hammerInitPos;
    protected Vector3 _hammerInitRot;
    
    private UpgradeCheckPanelBase _upgradeCheckPanel;

    protected virtual void Awake()
    {
        _upgradeCheckPanel = GetComponent<UpgradeCheckPanelBase>();
        _upgradeCheckPanel.UpgradeEvent += UpgradeCheckTween;
        
        _hammerInitPos = _hammer.transform.position;
        _hammerInitRot = _hammer.transform.localEulerAngles;
        
        _checkButton = transform.Find("Buttons/CheckButton").gameObject;
    }

    protected virtual void OnDestroy()
    {
        _upgradeCheckPanel.UpgradeEvent -= UpgradeCheckTween;
    }

    protected void UpgradeCheckTween()
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
            
            if(i==2)
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
