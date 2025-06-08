using IH.EventSystem.LevelEvent;
using IH.Level;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YH.EventSystem;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public StageDataSO stageDataSO;
    [SerializeField] private GameEventChannelSO _levelChannel;
    [SerializeField] private float _offset;

    private int CurrentSpecialRoomCount =>
        _levelGridDictionary.Count(x => x.Value.levelType == LevelTypeEnum.SpecialLevel);

    private int _specialRoomMaxFail;
    private int _specialRoomTryCnt;

    private Dictionary<Vector2Int, LevelRoom> _levelGridDictionary;
    private Dictionary<LevelTypeEnum, List<LevelRoom>> _levelRoomDictionary;

    private List<Vector2Int> _selectAbleRoomGrid;
    private LevelRoom _currentRoom;

    public event Action GenerateCompleteAction;

    private void Awake()
    {
        _levelRoomDictionary = new Dictionary<LevelTypeEnum, List<LevelRoom>>();
        stageDataSO.roomPairs.ForEach(x => _levelRoomDictionary.Add(x.levelTypeEnum, x.levelRooms));

        _levelGridDictionary = new Dictionary<Vector2Int, LevelRoom>();
        _selectAbleRoomGrid = new List<Vector2Int>();

        _specialRoomMaxFail = stageDataSO.roomCount / stageDataSO.specialRoomCount;
    }

    private void Start()
    {
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
        while (_levelGridDictionary.Count < stageDataSO.roomCount)
        {
            if (isComplete)
                break;

            _currentRoom = RandomRoomSelect();
            isComplete = OpenDoorRoomGenerate(_currentRoom);

            _selectAbleRoomGrid.Remove(_currentRoom.GridPos);
            yield return null;
        }

        // 연결이 안된 문들 비활성화 및 방 포지션 설정
        foreach (var levelRoom in _levelGridDictionary.Values)
        {
            NoConnectDoorDisable(levelRoom);
            AddConnectGrid(levelRoom);
        }

        DeadEndRoomCheck();

        PassData();
        GenerateCompleteAction?.Invoke();
    }

    private void DeadEndRoomCheck()
    {
        int deadEndRoomCount = CurrentDeadEndRoomsCount();

        if (deadEndRoomCount < stageDataSO.minDeadEndRoom)
            GenerateDeadEndRoom();

        var deadEndRoomSortDistance = LevelModuler.BFS(_levelGridDictionary)
            .Where(x =>
            {
                LevelRoom currentRoom = _levelGridDictionary[x.Key];
                return currentRoom.connectGrid.Count == 1 && currentRoom.levelType == LevelTypeEnum.DefaultLevel;
            }).OrderByDescending(x => x.Value).ToList();

        int currentDeadEndRoomOrder = 0;
        foreach (var pair in deadEndRoomSortDistance)
        {
            if (currentDeadEndRoomOrder >= stageDataSO.deadEndOrder.Count)
                return;

            ReplaceRoom(pair.Key, stageDataSO.deadEndOrder[currentDeadEndRoomOrder]);
            currentDeadEndRoomOrder++;
        }
    }

    private void GenerateDeadEndRoom()
    {
        List<LevelRoom> addDeadEndAbleRooms = _levelGridDictionary.Values.Where(x
            => x.connectGrid.Count != 1
               && x.CanAddDoor
               && x.levelType != LevelTypeEnum.StartLevel
               && CheckCanAddRoom(x.GridPos) > 0).ToList();

        // 만들 수 있는 모든 데드 엔드 방을 만들었는데 개수가 부족한 경우
        if (!TryDeadEndRoomGenerate(addDeadEndAbleRooms))
            FillInDeadEndRoom();
    }

    private bool TryDeadEndRoomGenerate(List<LevelRoom> addDeadEndAbleRooms)
    {
        foreach (var levelRoom in addDeadEndAbleRooms)
        {
            int closeDoorCount = levelRoom.CloseDoors.Count;
            for (int i = 0; i < closeDoorCount; i++)
            {
                LevelDoor closeDoor = levelRoom.GetRandomCloseDoor();
                DoorDir dir = closeDoor.GetDir();
                Vector2Int nextGrid = levelRoom.GridPos + LevelModuler.GetNextGrid(dir);

                if (_levelGridDictionary.ContainsKey(nextGrid))
                    continue;

                levelRoom.AddOpenDoor(dir);
                levelRoom.connectGrid.Add(nextGrid);

                AddRoom(LevelTypeEnum.DefaultLevel, nextGrid, false, 1);
                _levelGridDictionary[nextGrid].AlignRoom(closeDoor);
                _levelGridDictionary[nextGrid].connectGrid.Add(levelRoom.GridPos);

                int currentDeadEndCount = CurrentDeadEndRoomsCount();
                if (stageDataSO.minDeadEndRoom == currentDeadEndCount)
                    return true;
            }
        }

        return false;
    }

    private void FillInDeadEndRoom()
    {
        bool isComplete = false;
        while (!isComplete)
        {
            List<LevelRoom> addDeadEndAbleDeadEndRooms = _levelGridDictionary.Values.Where(x
                => x.connectGrid.Count == 1
                   && x.CanAddDoor
                   && x.levelType != LevelTypeEnum.StartLevel
                   && CheckCanAddRoom(x.GridPos) > 0).ToList();

            isComplete = TryDeadEndRoomGenerate(addDeadEndAbleDeadEndRooms);
        }
    }

    private int CurrentDeadEndRoomsCount()
    {
        return _levelGridDictionary.Values.Count(
            x => x.connectGrid.Count == 1 && x.levelType == LevelTypeEnum.DefaultLevel);
    }

    private int CheckCanAddRoom(Vector2Int levelGrid)
    {
        int canAddRoomCount = 0;
        for (int i = 0; i < 4; i++)
        {
            Vector2Int pos = levelGrid + LevelModuler.GetNextGrid((DoorDir)i);
            if (!_levelGridDictionary.ContainsKey(pos))
                canAddRoomCount++;
        }
        return canAddRoomCount;
    }

    private void PassData()
    {
        var levelEvent = LevelEvents.levelDataPassEvent;
        levelEvent.levelGridDictionary = _levelGridDictionary;
        _levelChannel.RaiseEvent(levelEvent);
    }

    private void AddConnectGrid(LevelRoom levelRoom)
    {
        foreach (LevelDoor door in levelRoom.OpenDoors)
            levelRoom.connectGrid.Add(levelRoom.GridPos + LevelModuler.GetNextGrid(door.GetDir()));
    }

    //현재 방의 열린(활성화된) 문 방향으로 방을 만듬
    private bool OpenDoorRoomGenerate(LevelRoom currentRoom)
    {
        foreach (var openDoor in currentRoom.OpenDoors)
        {
            if (_levelGridDictionary.Count >= stageDataSO.roomCount)
                return true;

            Vector2Int nextGridPos = currentRoom.GridPos + LevelModuler.GetNextGrid(openDoor.GetDir());
            if (_levelGridDictionary.ContainsKey(nextGridPos))
                continue;

            if (CurrentSpecialRoomCount != stageDataSO.specialRoomCount && Vector2Int.Distance(Vector2Int.zero, nextGridPos) > 1)
                RandomSpecialRoom(nextGridPos);
            else
                AddRoom(LevelTypeEnum.DefaultLevel, nextGridPos, true);

            _levelGridDictionary[nextGridPos].AlignRoom(openDoor);
        }

        return false;
    }

    private void RandomSpecialRoom(Vector2Int nextGridPos)
    {
        if (_specialRoomTryCnt >= _specialRoomMaxFail - CurrentSpecialRoomCount)
        {
            AddRoom(LevelTypeEnum.SpecialLevel, nextGridPos, true);
            _specialRoomTryCnt = 0;
        }
        else
        {
            // 1 = 일반 방 , 2 = 스페셜 룸
            int randRoom = Random.Range(1, 3);

            if ((LevelTypeEnum)randRoom == LevelTypeEnum.SpecialLevel)
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
        if (!_levelGridDictionary.ContainsKey(gridPos))
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
