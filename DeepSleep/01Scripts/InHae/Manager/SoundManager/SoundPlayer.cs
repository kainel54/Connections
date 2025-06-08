using System;
using System.Collections;
using DG.Tweening;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour, IPoolable
{
    public GameObject GameObject { get => gameObject; set { } }

    public Enum PoolEnum { get => _type; set { } }

    [SerializeField] private ObjectType _type;

    public void Init()
    {
        _audioSource.volume = 1f;
    }

    [SerializeField] private AudioMixerGroup _sfxGroup, _bgmGroup;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(SoundSO clipData)
    {
        if (clipData.audioType == SoundSO.AudioType.SFX)
        {
            _audioSource.outputAudioMixerGroup = _sfxGroup;
            _audioSource.volume = clipData.volume;
        }
        else if (clipData.audioType == SoundSO.AudioType.Music)
        {
            _audioSource.outputAudioMixerGroup = _bgmGroup;
            _audioSource.volume = 0;
            _audioSource.DOFade(clipData.volume, 1f); //BGM은 천천히 올라가게 한다.
        }

        _audioSource.spatialBlend = clipData.is3DSound ? 1f : 0; //3D 사운드면 셋팅한다.

        _audioSource.pitch = clipData.pitch;
        if (clipData.isRandomizePitch)
        {
            float modifier = clipData.randomPitchModifier;
            _audioSource.pitch += Random.Range(-modifier, modifier);
        }

        _audioSource.clip = clipData.clip;
        _audioSource.loop = clipData.isLoop;
        
        _audioSource.time = clipData.startTime;

        if (!clipData.isLoop)
        {
            float time = _audioSource.clip.length + 0.2f;
            StartCoroutine(DisableSoundTimer(time));
        }
        _audioSource.Play();
    }

    private IEnumerator DisableSoundTimer(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    public void StopAndGotoPool(bool isFadeOut)
    {
        if (isFadeOut)
        {
            _audioSource.DOFade(0, 1f).OnComplete(()=>gameObject.SetActive(false));
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPop()
    {

    }

    public void OnPush()
    {

    }
}
