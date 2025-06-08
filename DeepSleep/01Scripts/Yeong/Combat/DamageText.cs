using DG.Tweening;
using ObjectPooling;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour, IPoolable
{
    public GameObject GameObject { get => gameObject; set { } }

    public Enum PoolEnum { get => _type; set { } }

    [SerializeField] private UIPoolingType _type;



    [SerializeField] private TextMeshPro _text;

    private void Update()
    {
    }

    public void Setting(int damage, bool isCritiacl, Vector3 pos)
    {
        transform.position = pos + Vector3.up + UnityEngine.Random.insideUnitSphere * 0.5f;
        _text.color = isCritiacl ? Color.red : Color.white;
        _text.fontSize = isCritiacl ? 9 : 7;
        _text.text = damage.ToString();
        float scaleTime = 0.2f;
        float fadeTime = 1.5f;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(2.5f, scaleTime));
        seq.Append(transform.DOScale(1.2f, scaleTime));
        seq.Append(transform.DOScale(0.3f, fadeTime));
        seq.Join(_text.DOFade(0, fadeTime));
        seq.Join(transform.DOLocalMoveY(pos.y + 4, fadeTime));
        seq.AppendCallback(() => PoolManager.Instance.Push(this));
    }

    public void Setting(string text, bool isCritiacl, Vector3 pos)
    {
        transform.position = pos + Vector3.up + UnityEngine.Random.insideUnitSphere * 0.5f;
        _text.color = isCritiacl ? Color.red : Color.white;
        _text.fontSize = isCritiacl ? 9 : 7;
        _text.text = text;
        float scaleTime = 0.2f;
        float fadeTime = 1.5f;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(2.5f, scaleTime));
        seq.Append(transform.DOScale(1.2f, scaleTime));
        seq.Append(transform.DOScale(0.3f, fadeTime));
        seq.Join(_text.DOFade(0, fadeTime));
        seq.Join(transform.DOLocalMoveY(pos.y + 4, fadeTime));
        seq.AppendCallback(() => PoolManager.Instance.Push(this));
    }

    public void Init()
    {

    }

    public void OnPop()
    {

    }

    public void OnPush()
    {

    }
}
