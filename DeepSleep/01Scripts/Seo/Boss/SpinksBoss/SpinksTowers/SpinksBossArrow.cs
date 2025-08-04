using DG.Tweening;
using System.Collections;
using UnityEngine;
using YH.Players;

public class SpinksBossArrow : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private Transform _playerTrm;
    private Vector3 _targetingPosition;
    private bool isStart = false;

    private void Awake()
    {
        _playerManagerSO.SetUpPlayerEvent += HandleSetPlayer;

    }

    private void OnDestroy()
    {
        _playerManagerSO.SetUpPlayerEvent -= HandleSetPlayer;
    }


    private void HandleSetPlayer()
    {
        _playerTrm = _playerManagerSO.PlayerTrm;
    }

    public void SetMove(Vector3 targetPos)
    {
        _targetingPosition = targetPos;
        isStart = true;
        StartCoroutine(StartTargeting());
    }

    public void StopMove()
    {
        isStart = false;
        _targetingPosition = Vector3.zero;
    }


    private IEnumerator StartTargeting()
    {
        while (isStart)
        {
            yield return null;
            transform.position = _playerTrm.position + Vector3.up * 1.5f;
            Vector3 dir = (_targetingPosition - transform.position).normalized;
            dir.y = 0;
            transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
        }
    }
}