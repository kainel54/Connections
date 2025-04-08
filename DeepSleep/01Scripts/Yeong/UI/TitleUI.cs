using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    [SerializeField] private Button _startBtn, _quitBtn;

    private void Awake()
    {
        _startBtn.onClick.AddListener(GameStart);
        _quitBtn.onClick.AddListener(Quit);
    }

    private void GameStart()
    {
        SceneManager.LoadScene("MergeScene");
    }
    private void Quit()
    {
        Application.Quit();
    }

}
