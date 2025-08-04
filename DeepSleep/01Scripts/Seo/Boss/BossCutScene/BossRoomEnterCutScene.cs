using System;
using UnityEngine;
using YH.Players;

public class BossRoomEnterCutScene : MonoBehaviour
{
    public event Action cutSceneEnded;
    [SerializeField] private CameraSwitcher _cameraSwitcher;
    
    [SerializeField] private UIInputReader _uiInputReader;
    [SerializeField] private PlayerInputSO _playerInputReader;

    private GameObject UIObjects1;
    private GameObject UIObjects2;
    private GameObject PlayerObj;

    private void Start()
    {

        UIObjects1 = GameObject.Find("HudUI");
        UIObjects2 = GameObject.Find("InGameUI");
        PlayerObj = GameObject.Find("MOBAPlayer(Clone)");
        StartCutScene();
    }

    private void StartCutScene()
    {
        _uiInputReader.Controls.Disable();
        _playerInputReader.Controls.Disable();
        
        _cameraSwitcher.StartCinematic();
        _cameraSwitcher.SetCameraBrainSetting(true);
        Time.timeScale = 0f;
        UIObjects1.SetActive(false);
        UIObjects2.SetActive(false);
        PlayerObj.SetActive(false);
    }


    public void CutSceneEnded()
    {
        Time.timeScale = 1f;
        _cameraSwitcher.SetCameraBrainSetting(false);
        UIObjects1.SetActive(true);
        UIObjects2.SetActive(true);
        PlayerObj.SetActive(true);
        
        _uiInputReader.Controls.Enable();
        _playerInputReader.Controls.Enable();
        
        cutSceneEnded?.Invoke();
        Destroy(gameObject);
    }
}
