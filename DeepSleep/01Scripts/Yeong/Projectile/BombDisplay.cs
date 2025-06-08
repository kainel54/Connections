using DG.Tweening;
using ObjectPooling;
using System;
using UnityEngine;

public class BombDisplay : MonoBehaviour, IPoolable
{
    [SerializeField] private Transform _fill;
    private float _lifetime;

    public GameObject GameObject { get => gameObject; set { } }

    public Enum PoolEnum { get => _poolType; set { } }
    [SerializeField] private UIPoolingType _poolType;
    public event Action<BombDisplay> DisplayEndEvent;

    public void Init()
    {
    
    }

    public void SettingCircle(float radius, Vector3 position, float lifeTime)
    {
        transform.localScale = new Vector3(radius * 2, radius * 2, 0);
        transform.localPosition = position;
        _lifetime = lifeTime;
        Sequence seq = DOTween.Sequence().SetAutoKill(true);
        seq.Append(_fill.DOScaleX(0, 0));
        seq.Join(_fill.DOScaleY(0, 0));
        seq.Append(_fill.DOScaleX(1, _lifetime));
        seq.Join(_fill.DOScaleY(1, _lifetime));
        seq.AppendCallback(() =>
        {
            DisplayEndEvent?.Invoke(this);
            PoolManager.Instance.Push(this,true);
        });
    }


    public void SettingBox(float width, float height, Vector3 position,Quaternion dir, float lifeTime)
    {
        transform.localScale = new Vector3(width, height, 0);
        transform.SetLocalPositionAndRotation(position, dir);
        _lifetime = lifeTime;
        Sequence seq = DOTween.Sequence().SetAutoKill(true);
        seq.Join(_fill.DOScaleY(0, 0));
        seq.Join(_fill.DOScaleY(1, _lifetime));
        seq.AppendCallback(() =>
        {
            DisplayEndEvent?.Invoke(this);
            PoolManager.Instance.Push(this,true);
        });
    }

    public void OnPop()
    {
        
    }

    public void OnPush()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.zero;
    }
}
