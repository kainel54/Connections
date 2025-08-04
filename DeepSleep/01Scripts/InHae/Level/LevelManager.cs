using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem.LevelEvent;
using IH.EventSystem.SystemEvent;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private LevelTypeEnum _moveType;
    [SerializeField] private LevelTypeEnum _moveType2;
    [SerializeField] private LevelTypeEnum _moveType3;

    [SerializeField] private GameEventChannelSO _levelChannel;
    [SerializeField] private GameEventChannelSO _systemChannel;
    [SerializeField] private GameEventChannelSO _startStageEventChannel;

    private Dictionary<Vector2Int, LevelRoom> _levelGridDictionary;
    private Vector2Int _currentGrid = Vector2Int.zero;

    private LevelRoom _currentRoom;
    private DoorDir _beforeDir;
    private LevelTypeEnum _moveLevelType;

    private bool _isLevelMoving;

    private void Awake()
    {
        _levelChannel.AddListener<LevelDataPassEvent>(HandleDataPassEvent);

        _levelChannel.AddListener<BasicLevelMoveEvent>(HandleBasicLevelMoveEvent);
        _levelChannel.AddListener<PosLevelMoveEvent>(HandlePosLevelMoveEvent);
        _levelChannel.AddListener<TypeLevelMoveEvent>(HandleTypeLevelMoveEvent);
        _startStageEventChannel.AddListener<StageStartEvent>(HandleStartEnterEvent);
    }

    private void OnDestroy()
    {
        _levelChannel.RemoveListener<LevelDataPassEvent>(HandleDataPassEvent);

        _levelChannel.RemoveListener<BasicLevelMoveEvent>(HandleBasicLevelMoveEvent);
        _levelChannel.RemoveListener<PosLevelMoveEvent>(HandlePosLevelMoveEvent);
        _levelChannel.RemoveListener<TypeLevelMoveEvent>(HandleTypeLevelMoveEvent);
        _startStageEventChannel.RemoveListener<StageStartEvent>(HandleStartEnterEvent);
    }

    private void Update()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.T) && Input.GetKey(KeyCode.LeftControl))
        {
            var evt = LevelEvents.TypeLevelMoveEvent;
            evt.levelType = _moveType;
            _levelChannel.RaiseEvent(evt);
        }
        if (Input.GetKeyDown(KeyCode.V) && Input.GetKey(KeyCode.LeftControl))
        {
            _levelGridDictionary[_currentGrid].LevelClear();
        }
        if (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftControl))
        {
            var evt = LevelEvents.TypeLevelMoveEvent;
            evt.levelType = _moveType2;
            _levelChannel.RaiseEvent(evt);
        }
        if (Input.GetKeyDown(KeyCode.Y) && Input.GetKey(KeyCode.LeftControl))
        {
            var evt = LevelEvents.TypeLevelMoveEvent;
            evt.levelType = _moveType3;
            _levelChannel.RaiseEvent(evt);
        }
#endif
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            var evt = LevelEvents.TypeLevelMoveEvent;
            evt.levelType = _moveType2;
            _levelChannel.RaiseEvent(evt);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            _levelGridDictionary[_currentGrid].LevelClear();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            var evt = LevelEvents.TypeLevelMoveEvent;
            evt.levelType = _moveType;
            _levelChannel.RaiseEvent(evt);
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            var evt = LevelEvents.TypeLevelMoveEvent;
            evt.levelType = _moveType3;
            _levelChannel.RaiseEvent(evt);
        }
#endif
    }

    private void HandleStartEnterEvent(StageStartEvent evt)
    {
        _currentRoom.EnterEvent();
        Debug.Log("is Working");
    }

    private void HandleBasicLevelMoveEvent(BasicLevelMoveEvent evt)
    {
        if (_isLevelMoving)
            return;

        _currentGrid += LevelModuler.GetNextGrid(evt.enterDoorDir);
        if (!_levelGridDictionary.ContainsKey(_currentGrid))
        {
            _currentGrid -= LevelModuler.GetNextGrid(evt.enterDoorDir);
            return;
        }

        _isLevelMoving = true;

        _beforeDir = evt.enterDoorDir;
        _systemChannel.AddListener<FadeComplete>(HandleBasicFadeOutCompleteEvent);
        FadeOut();
    }

    private void HandlePosLevelMoveEvent(PosLevelMoveEvent evt)
    {
        _currentGrid = evt.pos;
        _systemChannel.AddListener<FadeComplete>(HandleFadeOutCompleteEvent);

        FadeOut();
    }

    private void HandleTypeLevelMoveEvent(TypeLevelMoveEvent evt)
    {
        _currentGrid = _levelGridDictionary.Values.ToList().Find(x => x.levelType == evt.levelType).GridPos;
        _systemChannel.AddListener<FadeComplete>(HandleFadeOutCompleteEvent);


        FadeOut();
    }

    private void HandleBasicFadeOutCompleteEvent(FadeComplete evt)
    {
        _systemChannel.RemoveListener<FadeComplete>(HandleBasicFadeOutCompleteEvent);
        FadeOutCompleteProcess();
        BasicLevelMove();

        if (_currentRoom.HasCutScene == true)
        {
            _currentRoom.EnterEvent();
        }
    }


    private void HandleFadeOutCompleteEvent(FadeComplete evt)
    {
        _systemChannel.RemoveListener<FadeComplete>(HandleFadeOutCompleteEvent);
        FadeOutCompleteProcess();
        LevelMove();

        if (_currentRoom.HasCutScene == true)
        {
            _currentRoom.EnterEvent();
        }
    }

    private void FadeOutCompleteProcess()
    {
        LevelMoveCompleteEvent();
        CurrentRoomChange();

        DoorProcess();

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
        var fadeInEvent = SystemEvents.FadeScreenEvent;
        fadeInEvent.isFadeIn = true;
        fadeInEvent.isCircle = true;
        fadeInEvent.fadeDuration = 0.4f;

        yield return new WaitForSecondsRealtime(0.4f);
        _systemChannel.RaiseEvent(fadeInEvent);
        _isLevelMoving = false;
    }

    private void BasicLevelMove()
    {
        LevelDoor levelDoor = _currentRoom.IsExistDirDoor(LevelModuler.GetConverseDir(_beforeDir));
        PlayerMove(levelDoor);
    }

    private void LevelMove()
    {
        LevelDoor levelDoor = _currentRoom.GetRandomOpenDoor();
        PlayerMove(levelDoor);
    }

    private void PlayerMove(LevelDoor levelDoor)
    {
        _player.PlayerLevelMove(levelDoor.spawnPoint.position);
    }

    private void CurrentRoomChange()
    {
        _currentRoom.gameObject.SetActive(false);
        _currentRoom = _levelGridDictionary[_currentGrid];
        _currentRoom.gameObject.SetActive(true);
    }

    private void DoorProcess()
    {
        if (_levelGridDictionary[_currentGrid].isClear)
            return;

        _currentRoom.ConnectDoorClose();
    }

    private void HandleDataPassEvent(LevelDataPassEvent evt)
    {
        _levelGridDictionary = evt.levelGridDictionary;

        foreach (var room in _levelGridDictionary.Values)
        {
            room.OpenDoors.ForEach(x => x.SetOpen(true));
            room.DoorInvalidateCheck();
            room.gameObject.SetActive(false);
        }

        _currentRoom = _levelGridDictionary[_currentGrid];
        _currentRoom.gameObject.SetActive(true);
        Transform playerTransform;

        if (_currentRoom.levelType == LevelTypeEnum.TutorialLevel)
            playerTransform = (_currentRoom as TutorialLevelRoom).playerSpawnPoint;
        else
            playerTransform = (_currentRoom as StartLevelRoom).playerSpawnPoint;

        _player = Instantiate(_player, playerTransform.position, Quaternion.identity);
    }
    private void LevelMoveCompleteEvent()
    {
        var evt = LevelEvents.LevelMoveCompleteEvent;
        evt.currentPoint = _currentGrid;
        _levelChannel.RaiseEvent(evt);
    }
}
