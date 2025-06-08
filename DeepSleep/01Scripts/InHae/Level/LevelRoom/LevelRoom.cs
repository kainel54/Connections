using IH.EventSystem.LevelEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;
using Random = UnityEngine.Random;

public enum LevelTypeEnum
{
    StartLevel = 0,
    DefaultLevel = 1,
    SpecialLevel = 2,
    ShopLevel = 3,
    BossLevel = 4,
    BlacksmithLevel = 5,
}

public abstract class LevelRoom : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _endStageEventChannel;

    public LevelTypeEnum levelType;
    public bool isAutoOpen;

    private List<LevelDoor> _doors;
    public List<LevelDoor> OpenDoors { get; private set; } = new();
    public List<LevelDoor> CloseDoors { get; private set; } = new();

    public Vector2Int GridPos { get; private set; }
    public List<Vector2Int> connectGrid = new();

    public bool isClear;

    private LevelDoor _selectDoor;
    private LevelDoor _beforeDoor;

    public event Action ClearEvent;

    public bool CanAddDoor => _doors.Count != OpenDoors.Count;

    protected virtual void Awake()
    {
        _doors = new List<LevelDoor>();
        _doors = GetComponentsInChildren<LevelDoor>().ToList();
        _doors.ForEach(x =>
        {
            x.SetRoom(this);
        });
    }

    public void Init(LevelTypeEnum typeEnum, Vector2Int gridPos, int doorCount)
    {
        levelType = typeEnum;
        GridPos = gridPos;

        doorCount = Mathf.Clamp(doorCount, 1, _doors.Count);

        AddOpenDoor(doorCount);
    }

    public void ReplaceSetting(LevelRoom beforeDoor, LevelTypeEnum typeEnum)
    {
        levelType = typeEnum;
        GridPos = beforeDoor.GridPos;

        transform.position = beforeDoor.transform.position;
        transform.rotation = beforeDoor.transform.rotation;
        AutoSetDoorDir();

        connectGrid = beforeDoor.connectGrid;

        foreach (LevelDoor beforeOpen in beforeDoor.OpenDoors)
        {
            LevelDoor door = _doors.Find(x => x.GetDir() == beforeOpen.GetDir());
            OpenDoors.Add(door);
        }
    }

    public void AddOpenDoor(int doorCount)
    {
        //이미 다 오픈돼있으면 리턴
        if (!CanAddDoor)
            return;

        //만약 현재 오픈된 문 + 추가하려는 문 개수가 최대 문 개수를 넘어서면
        if (OpenDoors.Count + doorCount >= _doors.Count)
            doorCount = _doors.Count - OpenDoors.Count;

        for (int i = 0; i < doorCount; i++)
        {
            if (_doors.Count == OpenDoors.Count)
                break;

            int randIdx = Random.Range(0, _doors.Count - OpenDoors.Count);
            int lastIdx = _doors.Count - OpenDoors.Count - 1;

            OpenDoors.Add(_doors[randIdx]);
            (_doors[randIdx], _doors[lastIdx]) = (_doors[lastIdx], _doors[randIdx]);
        }

        CloseDoorUpdate();
        AutoSetDoorDir();
    }

    public void AddOpenDoor(DoorDir doorDir)
    {
        OpenDoors.Add(_doors.Find(x => x.GetDir() == doorDir));
    }

    public void AlignRoom(LevelDoor beforeRoomDoor)
    {
        _beforeDoor = beforeRoomDoor;
        _selectDoor = GetRandomOpenDoor();

        float angle = Quaternion.Angle(_beforeDoor.transform.rotation, _selectDoor.transform.rotation);
        angle = Mathf.RoundToInt(angle);

        if ((int)angle == 90)
        {
            Vector3 cross = Vector3.Cross(_beforeDoor.transform.forward, _selectDoor.transform.forward);
            angle *= cross.y;
        }
        else
            angle += 180f;

        transform.rotation = Quaternion.Euler(0, angle, 0);
        AutoSetDoorDir();
    }

    private void AutoSetDoorDir()
    {
        foreach (var door in _doors)
            door.SetDir(GetDoorDir(door.transform.forward));
    }

    private DoorDir GetDoorDir(Vector3 dir)
    {
        if (dir == Vector3.right)
            return DoorDir.Right;
        if (dir == Vector3.left)
            return DoorDir.Left;
        if (dir == Vector3.forward)
            return DoorDir.Top;

        return DoorDir.Bottom;
    }

    public void DoorInvalidateCheck()
    {
        foreach (var door in _doors.Where(door => !door.isOpen))
            door.InvalidateMode();
    }

    public virtual void EnterEvent() { }

    public virtual void LevelClear()
    {
        if (isClear)
            return;

        isClear = true;
        ClearEvent?.Invoke();

        //var evt = LevelEvents.StageEndEvent;

       // if (!(levelType == LevelTypeEnum.StartLevel))
       //     _endStageEventChannel.RaiseEvent(evt);

        if (isAutoOpen) 
            ConnectDoorOpen();
    }

    public LevelDoor GetRandomCloseDoor()
    {
        CloseDoorUpdate();
        return CloseDoors[Random.Range(0, CloseDoors.Count)];
    }

    private void CloseDoorUpdate()
    {
        var newCloseDoors = _doors.Except(OpenDoors);

        if (CloseDoors.Count == 0 || newCloseDoors.Count() != CloseDoors.Count)
            CloseDoors = newCloseDoors.ToList();
    }

    public void ConnectDoorOpen() => OpenDoors.ForEach(x => x.Open());
    public void ConnectDoorClose() => OpenDoors.ForEach(x => x.Close());
    protected void ConnectDoorDisable() => OpenDoors.ForEach(x => x.DoorEnable(false));

    public int GetDoorCount() => _doors.Count;
    public LevelDoor GetRandomOpenDoor() => OpenDoors[Random.Range(0, OpenDoors.Count)];
    public LevelDoor IsExistDirDoor(DoorDir dir) => OpenDoors.Find(levelDoor => dir == levelDoor.GetDir());
}