using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LevelTypeEnum
{
    StartLevel = 0,
    DefaultLevel = 1,
    SpecialLevel = 2,
    ShopLevel = 3,
    BossLevel= 4,
    RewardLevel = 5,
    BlacksmithLevel = 6,
}

public abstract class LevelRoom : MonoBehaviour
{
    [SerializeField] private List<FadeWall> fadeWalls = new List<FadeWall>();
    
    public LevelTypeEnum levelType;
    public Vector2Int GridPos { get; private set; }

    private List<LevelDoor> _doors;
    public List<LevelDoor> OpenDoors { get; private set; }

    public List<Vector2Int> connectGrid;

    public bool isClear;

    private LevelDoor _selectDoor;
    private LevelDoor _beforeDoor;

    protected virtual void Awake()
    {
        OpenDoors = new List<LevelDoor>();
        connectGrid = new List<Vector2Int>();
        
        _doors = new List<LevelDoor>();
        _doors = GetComponentsInChildren<LevelDoor>().ToList();
        _doors.ForEach(x=>
        {
            x.SetRoom(this);
        });
    }
    
    public void Init(LevelTypeEnum typeEnum, Vector2Int gridPos, int doorCount)
    {
        levelType = typeEnum;
        GridPos = gridPos;
        
        doorCount = Mathf.Clamp(doorCount,2, _doors.Count);
        
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

    private void WallDisable()
    {
        foreach (var wall in fadeWalls)
        {
            wall.visual.SetActive(false);
            wall.gameObject.layer = LayerMask.NameToLayer("Wall");

            if (wall.transform.forward == Vector3.left || wall.transform.forward == Vector3.forward)
            {
                wall.gameObject.layer = LayerMask.NameToLayer("DetectWall");
                wall.visual.SetActive(true);
            }
        }
    }

    public void AddOpenDoor(int doorCount)
    {
        //이미 다 오픈돼있으면 리턴
        if(OpenDoors.Count == _doors.Count)
            return;
        
        //만약 현재 오픈된 문 + 추가하려는 문 개수가 최대 문 개수를 넘어서면
        if (OpenDoors.Count + doorCount >= _doors.Count)
            doorCount = _doors.Count - OpenDoors.Count;
        
        for (int i = 0; i < doorCount; i++)
        {   
            if(_doors.Count == OpenDoors.Count)
                break;
            
            int randIdx = Random.Range(0, _doors.Count - OpenDoors.Count);
            int lastIdx = _doors.Count - OpenDoors.Count - 1;
            
            OpenDoors.Add(_doors[randIdx]);
            (_doors[randIdx], _doors[lastIdx]) = (_doors[lastIdx], _doors[randIdx]);
        }
        AutoSetDoorDir();
    }

    public void AlignRoom(LevelDoor beforeRoomDoor)
    {
        _beforeDoor = beforeRoomDoor;
        _selectDoor = GetRandomDoor();

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
        WallDisable();
        foreach (var door in _doors)
        {
            door.SetDir(GetDoorDir(door.transform.forward));
        }
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

    public virtual void EnterEvent()
    {
        
    }
    
    public virtual void LevelClear()
    {
        if(isClear)
            return;
        
        isClear = true;
        ConnectDoorOpen();
    }

    public void ConnectDoorOpen() => OpenDoors.ForEach(x => x.Open());
    public void ConnectDoorClose() => OpenDoors.ForEach(x => x.Close());
    protected void ConnectDoorDisable() => OpenDoors.ForEach(x => x.DoorEnable(false));
    
    public int GetDoorCount() => _doors.Count;
    public LevelDoor GetRandomDoor() => OpenDoors[Random.Range(0, OpenDoors.Count)];
    public LevelDoor IsExistDirDoor(DoorDir dir) => OpenDoors.Find(levelDoor => dir == levelDoor.GetDir());
}