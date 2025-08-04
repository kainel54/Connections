using System;
using IH.EventSystem.LevelEvent;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using YH.EventSystem;

public class Barrier : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _startStageEventChannel;
    [SerializeField] private GameEventChannelSO _endStageEventChannel;

    private readonly int _offset = Shader.PropertyToID("_Offset");
    private MeshRenderer _barrierMaterial;
    private BoxCollider _boxCollider;
    private NavMeshObstacle _navMeshObstacle;

    private float _startValue = 0;
    private float _endValue = 0.8f;

    private void OnEnable()
    {
        _navMeshObstacle = GetComponent<NavMeshObstacle>();
        _barrierMaterial = GetComponent<MeshRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
        
        _boxCollider.enabled = false;
        _navMeshObstacle.enabled = false;
        _barrierMaterial.material.SetFloat(_offset, 0);

        if (gameObject.activeInHierarchy)
        {
            _startStageEventChannel.AddListener<StageStartEvent>(Raise);
            _endStageEventChannel.AddListener<StageEndEvent>(Lower);
        }
    }
    
    private void OnDisable()
    {
        _startStageEventChannel.RemoveListener<StageStartEvent>(Raise);
        _endStageEventChannel.RemoveListener<StageEndEvent>(Lower);
    }

    private void OnDestroy()
    {
        _startStageEventChannel.RemoveListener<StageStartEvent>(Raise);
        _endStageEventChannel.RemoveListener<StageEndEvent>(Lower);
    }

    private void Raise(StageStartEvent evt)
    {
        StartCoroutine(RaiseBarrier());
    }

    private void Lower(StageEndEvent evt)
    {
        StartCoroutine(LowerBarrier());
    }

    public IEnumerator RaiseBarrier()
    {
        _boxCollider.enabled = true;
        _navMeshObstacle.enabled = true;

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
        _boxCollider.enabled = false;
        _navMeshObstacle.enabled = false;

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
}
