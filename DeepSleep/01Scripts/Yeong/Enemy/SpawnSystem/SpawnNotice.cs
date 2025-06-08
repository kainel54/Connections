using DG.Tweening;
using ObjectPooling;
using System;
using UnityEngine;

public class SpawnNotice : MonoBehaviour, IPoolable
{
    [SerializeField] private EffectPoolingType _type;
    public GameObject GameObject { get => gameObject; set { } }

    public Enum PoolEnum { get => _type; set { } }
    public event Action OnEndEvent;

    public void OnPop()
    {
        transform.localScale = Vector3.zero;
        transform.DOScale(1, 0.8f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            OnEndEvent?.Invoke();
            DOVirtual.DelayedCall(1f, () =>
            {
                transform.DOScale(1, 0.2f).SetEase(Ease.OutCirc).OnComplete(() => PoolManager.Instance.Push(this));
            });
        });
        transform.DORotate(new Vector3(0,360,0), 2f, RotateMode.FastBeyond360);
    }

    public void OnPush()
    {
        OnEndEvent = null;
    }
}
