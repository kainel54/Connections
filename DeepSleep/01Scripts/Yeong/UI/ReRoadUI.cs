using DG.Tweening;
using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YH.Players;

public class ReRoadUI : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private Slider _reloadSlide;
    [SerializeField] private Image _reloadImage;
    private Player _player;
    private float _currentTime = 0;
    private bool _reloading = false;

    private void Awake()
    {
        _playerManagerSO.SetUpPlayerEvent += HandlePlayerSetUp;
    }

    private void OnDestroy()
    {
        _playerManagerSO.SetUpPlayerEvent -= HandlePlayerSetUp;
        _player.GetCompo<PlayerAttackCompo>().ReloadEvent -= ReloadEvent;
    }

    private void Update()
    {
        transform.parent.rotation = Camera.main.transform.rotation;

        if (_reloading == false)
            return;

        _currentTime += Time.deltaTime;

        _reloadSlide.value = _currentTime;

        if (_currentTime >= _reloadSlide.maxValue && _reloading)
        {
            StopReload();
            _reloading = false;
        }
    }

    private void StopReload()
    {
        StartCoroutine(DelayDestroy());
    }
    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        _currentTime = 0;
        _reloadImage.gameObject.SetActive(false);
    }

    private void ReloadEvent(float time)
    {
        _reloadSlide.value = 0;
        _reloadSlide.maxValue = time + .6f;
        _reloading = true;
        _reloadImage.gameObject.SetActive(true);
    }

    private void HandlePlayerSetUp()
    {
        _player = _playerManagerSO.Player;
        transform.parent.SetParent(_player.transform);
        _player.GetCompo<PlayerAttackCompo>().ReloadEvent += ReloadEvent;
    }
}
