using IH.EventSystem.LevelEvent;
using System.Collections;
using UnityEngine;
using YH.EventSystem;

public class Barrier : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _startStageEventChannel;
    [SerializeField] private GameEventChannelSO _endStageEventChannel;

    private readonly int _offset = Shader.PropertyToID("_Offset");
    private MeshRenderer _barrierMaterial;

    private float _startValue = 0;
    private float _endValue = 0.8f;

    private void OnEnable()
    {
        _barrierMaterial = GetComponent<MeshRenderer>();
        _barrierMaterial.material.SetFloat(_offset, 0);

        if (!gameObject.activeInHierarchy)
        {
            _startStageEventChannel.AddListener<StageStartEvent>((evt) => StartCoroutine(RaiseBarrier()));
            _endStageEventChannel.AddListener<StageEndEvent>((evt) => StartCoroutine(LowerBarrier()));
        }
    }

    public IEnumerator RaiseBarrier()
    {
        float startValue = _startValue;
        float endValue = _endValue;

        float currentTime = 0.0f;
        float endTime = 1.0f;
        float ratio = 0.0f;

        while (ratio <= 1)
        {
            currentTime += Time.deltaTime;
            ratio = currentTime / endTime;
            float currentValue = Mathf.Lerp(startValue, endValue, ratio);
            _barrierMaterial.material.SetFloat(_offset, currentValue);

            yield return null;
        }
    }

    public IEnumerator LowerBarrier()
    {
        float startValue = _endValue;
        float endValue = _startValue;

        float currentTime = 0.0f;
        float endTime = 1.0f;
        float ratio = 0.0f;

        while (ratio <= 1)
        {
            currentTime += Time.deltaTime;
            ratio = currentTime / endTime;
            float currentValue = Mathf.Lerp(startValue, endValue, ratio);
            _barrierMaterial.material.SetFloat(_offset, currentValue);

            yield return null;
        }
    }

    private void OnDisable()
    {
        _startStageEventChannel.RemoveListener<StageStartEvent>((evt) => StartCoroutine(RaiseBarrier()));
        _endStageEventChannel.RemoveListener<StageEndEvent>((evt) => StartCoroutine(LowerBarrier()));
    }
}
