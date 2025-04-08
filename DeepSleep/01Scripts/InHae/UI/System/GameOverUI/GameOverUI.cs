using DG.Tweening;
using System;
using IH.EventSystem;
using UnityEngine;
using UnityEngine.SceneManagement;
using YH.EventSystem;
using YH.Players;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemChannelSo;
    
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private UIInputReader _uiInputReader;
    [SerializeField] private CanvasGroup _gameOverPanel;
    private Player _player;
    
    private void Awake()
    {
        _playerManagerSO.SetUpPlayerEvent += HandlePlayerSetUp;
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _playerManagerSO.SetUpPlayerEvent -= HandlePlayerSetUp;
        
        if(_player != null)
            _player.GetCompo<PlayerAnimator>().OnDieEvent -= HandleDeadEvent;
    }

    private void HandlePlayerSetUp()
    {
        _player = _playerManagerSO.Player;
        _player.GetCompo<PlayerAnimator>().OnDieEvent += HandleDeadEvent;
    }

    private void HandleDeadEvent()
    {
        _uiInputReader.Controls.UI.Disable();
        _player.PlayerInput.Controls.Player.Disable();
        
        gameObject.SetActive(true);
        _gameOverPanel.blocksRaycasts = true;
        
        _gameOverPanel.DOFade(1, 0.5f).OnComplete(() =>
        {
            Time.timeScale = 0f;
        });
        
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        _systemChannelSo.AddListener<FadeComplete>(RestartInput);

        var evt = SystemEvents.FadeScreenEvent;
        evt.fadeDuration = 0.5f;
        evt.isFadeIn = false;
        evt.isCircle = true;

        _systemChannelSo.RaiseEvent(evt);

    }

    private void RestartInput(FadeComplete evt)
    {
        _systemChannelSo.RemoveListener<FadeComplete>(RestartInput);
        _uiInputReader.Controls.UI.Enable();
        _player.PlayerInput.Controls.Player.Enable();
        SceneManager.LoadScene("MergeScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
