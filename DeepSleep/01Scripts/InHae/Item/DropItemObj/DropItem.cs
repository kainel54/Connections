using System;
using System.Collections;
using System.Diagnostics;
using DG.Tweening;
using IH.EventSystem.SoundEvent;
using ObjectPooling;
using UnityEngine;
using YH.EventSystem;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public abstract class DropItem : MonoBehaviour, IPoolable
{
    [SerializeField] private GameEventChannelSO _soundEventChannelSO;
    [SerializeField] private SoundSO _soundSo;

    public ItemDataSO itemData;

    [SerializeField] private float _rotateSpeed = 10f;

    protected bool _alreadyCollected;
    public event Action CollectEvent;
    private bool _dropEnd;

    private Coroutine _dropRoutine;
    private Tween _tween;

    public virtual bool IsCollectAble { get; protected set; } = true;

    public SphereCollider SphereCollider { get; private set; }

    public bool HasTagged { get; set; } = true;
    
    private PoolingDefaultEffectPlayer _explodeEffect;
    private PoolingNoLifeTimeEffectPlayer _glowEffect;

    protected virtual void Awake()
    {
        SphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (!HasTagged)
            return;

        if (_alreadyCollected)
            return;

        PlaySound();
        _alreadyCollected = true;
        PickUp(other);
        CollectEvent?.Invoke();

    }

    private void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }

    public void SetItemDropPosition(Vector3 destination, LayerMask layerMask)
    {
        if (Physics.Raycast(destination, Vector3.down, out RaycastHit hit, 10f, layerMask))
        {
            destination = hit.point + new Vector3(0, 0.5f);
        }

        float jumpPower = Random.Range(1.5f, 2f);
        float duration = Random.Range(0.7f, 1.2f);
        int jumpCount = 1;

        if (transform == null)
            return;

        PlayGlowEffect();
        if (_tween != null && _tween.IsActive())
            _tween.Kill();

        _tween = transform.DOJump(destination, jumpPower, jumpCount, duration).OnComplete(() =>
        {
            _dropEnd = true;
            PlayExplodeEffect();
        });
    }

    public bool PickUpItem(Transform pickerTrm)
    {
        if (_alreadyCollected || !_dropEnd || !HasTagged)
            return false;

        if (_dropRoutine != null)
            StopCoroutine(_dropRoutine);

        _dropRoutine = StartCoroutine(PickUpCoroutine(pickerTrm));
        return true;
    }

    private IEnumerator PickUpCoroutine(Transform pickerTrm)
    {
        if (!SphereCollider.isTrigger)
            yield break;

        Vector3 startPos = transform.position;
        float distance = (pickerTrm.position - startPos).magnitude;

        float totalTime = distance * 0.1f;
        float current = 0;

        while (current / totalTime <= 1)
        {
            current += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, pickerTrm.position, current / totalTime);
            yield return null;
        }
    }

    public virtual void PickUp(Collider other)
    {
        if (_explodeEffect != null && _explodeEffect.gameObject.activeInHierarchy)
            PoolManager.Instance.Push(_explodeEffect, true);
        if (_glowEffect != null && _glowEffect.gameObject.activeInHierarchy)
            PoolManager.Instance.Push(_glowEffect, true);
        
        PoolManager.Instance.Push(this, true);
    }

    public void PlaySound()
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.position = transform.position;
        soundEvt.clipData = _soundSo;
        _soundEventChannelSO.RaiseEvent(soundEvt);
    }

    public GameObject GameObject => gameObject;
    public virtual Enum PoolEnum { get; }
    public virtual void OnPop()
    {
        SphereCollider.enabled = true;
        _dropEnd = false;
        _alreadyCollected = false;
        CollectEvent = null;
    }

    public virtual void OnPush()
    {
        _explodeEffect = null;
        _glowEffect = null;
    }

    private void PlayExplodeEffect()
    {
        if(itemData ==null || itemData.itemTier == ItemTier.Normal)
            return;

        switch (itemData.itemTier)
        {
            case ItemTier.Rare:
                _explodeEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemRareExplode) as PoolingDefaultEffectPlayer;
                break;
            case ItemTier.Epic:
                _explodeEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemEpicExplode) as PoolingDefaultEffectPlayer;
                break;
            case ItemTier.Legendary:
                _explodeEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemLegendaryExplode) as PoolingDefaultEffectPlayer;
                break;
        }

        _explodeEffect.PlayEffect(transform.position, Quaternion.Euler(-90f,0f,0f),
            Vector3.one * 0.3f, transform);
    }

    private void PlayGlowEffect()
    {
        if(itemData ==null || itemData.itemTier == ItemTier.Normal)
            return;

        switch (itemData.itemTier)
        {
            case ItemTier.Rare:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemRareGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
            case ItemTier.Epic:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemEpicGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
            case ItemTier.Legendary:
                _glowEffect = PoolManager.Instance.Pop(EffectPoolingType.ItemLegendaryGlow) as PoolingNoLifeTimeEffectPlayer;
                break;
        }

        _glowEffect.PlayEffect(transform.position, Quaternion.identity, Vector3.one, transform);
    }
}
