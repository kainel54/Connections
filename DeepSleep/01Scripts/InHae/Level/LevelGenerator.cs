using System.Collections;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem;
using IH.Level;
using UnityEngine;
using YH.EventSystem;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private StageDataSO _stageDataSO;
    [SerializeField] private GameEventChannelSO _systemChannel;
    [SerializeField] private GameEventChannelSO _levelChannel;
    [SerializeField] private GameEventChannelSO _soundChannel;
    
    [SerializeField] private float _delay = 0.1f;
    [SerializeField] private float _offset;

    private int _currentSpecialRoomCount => 
        _levelGridDictionary.Count(x => x.Value.levelType == LevelTypeEnum.SpecialLevel);
    private int _specialRoomMaxFail => _stageDataSO.roomCount / _stageDataSO.specialRoomCount;
    private int _specialRoomTryCnt;
    
    private Dictionary<Vector2Int, LevelRoom> _levelGridDictionary;
    private Dictionary<LevelTypeEnum, List<LevelRoom>> _levelRoomDictionary;
    private Dictionary<Vector2Int, int> _distance;
    
    private List<Vector2Int> _selectAbleRoomGrid;
    private LevelRoom _currentRoom;

    private void Awake()
    {
        _levelRoomDictionary = new Dictionary<LevelTypeEnum, List<LevelRoom>>();
        _stageDataSO.roomPairs.ForEach(x => _levelRoomDictionary.Add(x.levelTypeEnum, x.levelRooms));
        
        _levelGridDictionary = new Dictionary<Vector2Int, LevelRoom>();
        _selectAbleRoomGrid = new List<Vector2Int>();
    }

    private void Start()
    {
        var evt = SystemEvents.FirstFadeSetting;
        _systemChannel.RaiseEvent(evt);
        
        RoomGenerating();
    }

    [ContextMenu("StartGenerating")]
    private void RoomGenerating()
    {
        StartCoroutine(RoomGeneratingRoutine());
    }
    
    // 방 개수가 다 될 때까지 만큼 while
    private IEnumerator RoomGeneratingRoutine()
    {
        AddRoom(LevelTypeEnum.StartLevel, Vector2Int.zero, false, 4);
        bool isComplete = false;
        
        // 기본, 스페셜 방 생성
        while (_levelGridDictionary.Count < _stageDataSO.roomCount)
        {
            if (isComplete)
                break;
            
            _currentRoom = RandomRoomSelect();
            isComplete = OpenDoorRoomGenerate();
            
            _selectAbleRoomGrid.Remove(_currentRoom.GridPos);
            yield return new WaitForSeconds(_delay);
        }
        
        // 연결이 안된 문들 비활성화 및 방 포지션 설정
        foreach (KeyValuePair<Vector2Int,LevelRoom> levelRoom in _levelGridDictionary)
        {
            NoConnectDoorDisable(levelRoom.Value);
            AddConnectGrid(levelRoom.Value);
        }
        
        GenerateBossRoom();
        GenerateExceptionRoom(LevelTypeEnum.ShopLevel);
        GenerateExceptionRoom(LevelTypeEnum.BlacksmithLevel);
        
        PassData();
    }
    
    private void PassData()
    {
        var levelEvent = LevelEvents.levelDataPassEvent;
        levelEvent.levelGridDictionary = _levelGridDictionary;
        _levelChannel.RaiseEvent(levelEvent);

        StartCoroutine(FadeIn());
    }
    
    private IEnumerator FadeIn()
    {
        var fadeOutEvent = SystemEvents.FadeScreenEvent;
        fadeOutEvent.isFadeIn = true;
        fadeOutEvent.isCircle = true;
        fadeOutEvent.fadeDuration = 0.5f;
        
        _systemChannel.AddListener<FadeComplete>(MusicPlay);

        yield return new WaitForSeconds(0.5f);
        _systemChannel.RaiseEvent(fadeOutEvent);
    }

    // 추후 수정
    private void MusicPlay(FadeComplete evt)
    {
        _systemChannel.RemoveListener<FadeComplete>(MusicPlay);
        
        var soundEvt = SoundEvents.PlayBGMEvent;
        soundEvt.clipData = _stageDataSO.DefaultSoundSo;
        _soundChannel.RaiseEvent(soundEvt);
    }

    private void AddConnectGrid(LevelRoom levelRoom)
    {
        foreach (LevelDoor door in levelRoom.OpenDoors)
        {
            levelRoom.connectGrid.Add(levelRoom.GridPos + LevelModuler.GetNextGrid(door.GetDir()));
        }
    }

    private void GenerateExceptionRoom(LevelTypeEnum typeEnum)
    {
        var defaultLevels = _levelGridDictionary.Values
            .Where(x => x.levelType == LevelTypeEnum.DefaultLevel)
            .ToList();

        if (defaultLevels.Count > 0)
            ReplaceRoom(defaultLevels[Random.Range(0, defaultLevels.Count)].GridPos, typeEnum);
    }
    
    private void GenerateBossRoom()
    {
        _distance = LevelModuler.Bfs(_levelGridDictionary);
        
         Vector2Int mostFarGrid = Vector2Int.zero;
         int mostFarDistance = 0;
         
         foreach (var i in _distance)
         {
             if (_levelGridDictionary[i.Key].levelType != LevelTypeEnum.DefaultLevel)
                 continue;
             
             if (i.Value > mostFarDistance)
             {
                 mostFarDistance = i.Value;
                 mostFarGrid = i.Key;
             }
         }
         
        ReplaceRoom(mostFarGrid, LevelTypeEnum.BossLevel);
    }

    //현재 방의 열린(활성화된) 문 방향으로 방을 만듬
    private bool OpenDoorRoomGenerate()
    {
        foreach (var openDoor in _currentRoom.OpenDoors)
        {
            if (_levelGridDictionary.Count >= _stageDataSO.roomCount)
            {
                return true;
            }
                
            Vector2Int nextGridPos = _currentRoom.GridPos + LevelModuler.GetNextGrid(openDoor.GetDir());
            if(_levelGridDictionary.ContainsKey(nextGridPos))
                continue;

            if (_currentSpecialRoomCount != _stageDataSO.specialRoomCount && Vector2Int.Distance(Vector2Int.zero, nextGridPos) > 1)
                RandomSpecialRoom(nextGridPos);
            else
                AddRoom(LevelTypeEnum.DefaultLevel, nextGridPos, true);
                
            _levelGridDictionary[nextGridPos].AlignRoom(openDoor);
        }
        
        return false;
    }

    private void RandomSpecialRoom(Vector2Int nextGridPos)
    {
        if (_specialRoomTryCnt >= _specialRoomMaxFail - _currentSpecialRoomCount)
        {
            AddRoom(LevelTypeEnum.SpecialLevel, nextGridPos, true);
            _specialRoomTryCnt = 0;
        }
        else
        {
            // 1 = 일반 방 , 2 = 스페셜 룸
            int randRoom = Random.Range(1, 3);
            
            if((LevelTypeEnum)randRoom == LevelTypeEnum.SpecialLevel)
                _specialRoomTryCnt = 0;
            else
                _specialRoomTryCnt++;
            
            AddRoom((LevelTypeEnum)randRoom, nextGridPos, true);
        }
    }
    
    private void NoConnectDoorDisable(LevelRoom currentRoom)
    {
        List<LevelDoor> removeList = new List<LevelDoor>();
        
        for (int i = 0; i < currentRoom.OpenDoors.Count; i++)
        {
            var levelDoor = currentRoom.OpenDoors[i];
            Vector2Int nextGridPos = currentRoom.GridPos + LevelModuler.GetNextGrid(levelDoor.GetDir());

            // 문 방향에 방이 없을 때
            if (!_levelGridDictionary.ContainsKey(nextGridPos))
            {
                removeList.Add(levelDoor);
                continue;
            }

            DoorDir converseDir = LevelModuler.GetConverseDir(levelDoor.GetDir());
            if (_levelGridDictionary[nextGridPos].IsExistDirDoor(converseDir) == null)
            {
                removeList.Add(levelDoor);
            }
        }

        foreach (var removeDoor in removeList)
            currentRoom.OpenDoors.Remove(removeDoor);
    }

    //현재 선택 가능한 방들(이미 한 번 선택되어 방을 생성한 방들 빼고)중 랜덤한 방 선택
    private LevelRoom RandomRoomSelect()
    {
        if (_selectAbleRoomGrid.Count == 0)
        {
            List<Vector2Int> keyList = _levelGridDictionary.Keys.ToList();
            Vector2Int randKey = keyList[Random.Range(0, keyList.Count)];
            
            _levelGridDictionary[randKey].AddOpenDoor(1);
            _selectAbleRoomGrid.Add(randKey);
            
        }
        
        Vector2Int randomSelectRoom = _selectAbleRoomGrid[Random.Range(0, _selectAbleRoomGrid.Count)];
        return _levelGridDictionary[randomSelectRoom];
    }  

    //방을 만들고 데이터에 추가해주는 함수
    private void AddRoom(LevelTypeEnum typeEnum, Vector2Int gridPos, bool isRandomDoor, int doorCount = 0)
    {
        LevelRoom levelRoom = null;
        
        levelRoom = Instantiate(ListRandomRoom(_levelRoomDictionary[typeEnum]), transform);

        doorCount = isRandomDoor ? Random.Range(2, levelRoom.GetDoorCount() + 1) : doorCount;
        levelRoom.Init(typeEnum, gridPos, doorCount);
        levelRoom.transform.position = new Vector3(levelRoom.GridPos.x * _offset, 0, levelRoom.GridPos.y * _offset);
        
        _selectAbleRoomGrid.Add(gridPos);
        _levelGridDictionary.Add(gridPos, levelRoom);
    }

    private void RemoveRoom(Vector2Int gridPos)
    {
        if(!_levelGridDictionary.ContainsKey(gridPos))
            return;

        Destroy(_levelGridDictionary[gridPos].gameObject);
        _levelGridDictionary.Remove(gridPos);
    }

    private void ReplaceRoom(Vector2Int gridPos, LevelTypeEnum typeEnum)
    {
        LevelRoom newRoom = Instantiate(ListRandomRoom(_levelRoomDictionary[typeEnum]), transform);
        newRoom.ReplaceSetting(_levelGridDictionary[gridPos], typeEnum);
        RemoveRoom(gridPos);
        
        _levelGridDictionary.Add(gridPos, newRoom);
    }

    private LevelRoom ListRandomRoom(List<LevelRoom> levelRooms)
    {
        return levelRooms[Random.Range(0, levelRooms.Count)];
    }
}
