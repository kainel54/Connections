using System.Collections;
using System.Collections.Generic;
using IH.EventSystem;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    
    [SerializeField] private GameEventChannelSO _levelChannel;
    [SerializeField] private GameEventChannelSO _systemChannel;
    
    private Dictionary<Vector2Int, LevelRoom> _levelGridDictionary;
    private Vector2Int _currentGrid = Vector2Int.zero;

    private LevelRoom _currentRoom;
    private DoorDir _beforeDir;

    private void Awake()
    {
        _levelChannel.AddListener<LevelDataPassEvent>(HandleDataPassEvent);
        _levelChannel.AddListener<LevelMoveEvent>(HandleLevelMoveEvent);
        
    }

    private void OnDestroy()
    {
        _levelChannel.Clear();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            _levelGridDictionary[_currentGrid].LevelClear();
        }
    }

    private void HandleLevelMoveEvent(LevelMoveEvent evt)
    {
        _currentGrid += LevelModuler.GetNextGrid(evt.enterDoorDir);

        if (!_levelGridDictionary.ContainsKey(_currentGrid))
        {
            _currentGrid -= LevelModuler.GetNextGrid(evt.enterDoorDir);
            return;
        }
        
        _beforeDir = evt.enterDoorDir;
        _systemChannel.AddListener<FadeComplete>(HandleFadeCompleteEvent);
        FadeOut();
    }

    private void HandleFadeCompleteEvent(FadeComplete evt)
    {
        _systemChannel.RemoveListener<FadeComplete>(HandleFadeCompleteEvent);
                
        LevelMoveCompleteEvent();
        CurrentRoomChange();

        PlayerMove();
        
        DoorProcess();
        _currentRoom.EnterEvent();
        
        StartCoroutine(FadeIn());
    }

    private void FadeOut()
    {
        var fadeOutEvent = SystemEvents.FadeScreenEvent;
        fadeOutEvent.isFadeIn = false;
        fadeOutEvent.isCircle = true;
        fadeOutEvent.fadeDuration = 0.2f;
        
        _systemChannel.RaiseEvent(fadeOutEvent);
    }
    
    private IEnumerator FadeIn()
    {
        var fadeOutEvent = SystemEvents.FadeScreenEvent;
        fadeOutEvent.isFadeIn = true;
        fadeOutEvent.isCircle = true;
        fadeOutEvent.fadeDuration = 0.4f;

        yield return new WaitForSeconds(0.4f);
        _systemChannel.RaiseEvent(fadeOutEvent);
    }

    private void PlayerMove()
    {
        LevelDoor levelDoor = _currentRoom.IsExistDirDoor(LevelModuler.GetConverseDir(_beforeDir));
        _player.GetComponent<CharacterController>().enabled = false;
        _player.transform.position = levelDoor.transform.Find("SpawnPoint").position;
        _player.GetComponent<CharacterController>().enabled = true;
    }

    private void CurrentRoomChange()
    {
        _currentRoom.gameObject.SetActive(false);
        _currentRoom = _levelGridDictionary[_currentGrid];
        _currentRoom.gameObject.SetActive(true);
    }

    private void DoorProcess()
    {
        if(_levelGridDictionary[_currentGrid].isClear)
            return;

        _currentRoom.OpenDoors.ForEach(x => x.visual.localPosition =
            new Vector3(x.defaultValue.x + 3f, x.defaultValue.y, x.defaultValue.z));
        _currentRoom.ConnectDoorClose();
    }

    private void HandleDataPassEvent(LevelDataPassEvent evt)
    {
        _levelGridDictionary = evt.levelGridDictionary;

        foreach (var room in _levelGridDictionary.Values)
        {
            room.OpenDoors.ForEach(x=> x.SetOpen(true));
            room.gameObject.SetActive(false);
        }

        _currentRoom = _levelGridDictionary[_currentGrid];
        _currentRoom.gameObject.SetActive(true);
        
        _player = Instantiate(_player);
    }

    private void LevelMoveCompleteEvent()
    {
        var evt = LevelEvents.LevelMoveCompleteEvent;
        evt.currentPoint = _currentGrid;
        _levelChannel.RaiseEvent(evt);
    }
}
