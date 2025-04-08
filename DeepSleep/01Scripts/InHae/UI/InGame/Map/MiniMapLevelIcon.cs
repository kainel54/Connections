using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public enum MiniMapIconState
{
    //Hiding = 근처에 가지 못해서 숨겨진 상태
    Hiding,
    //Find = 근처에 가서 발견됐으나 들어가지 않은 상태
    Find,
    //Check = 플레이어가 방에 들어간 적이 있는 상태
    Check
}

public class MiniMapLevelIcon : MonoBehaviour
{
    [SerializeField] private RectTransform _findSetting;
    [SerializeField] private RectTransform _checkSetting;
    
    private GameObject _userPoint;
    private RectTransform _doorParent;

    private List<MiniMapDoorIcon> _doorIcons;
    
    private MiniMapIconState _currentState = MiniMapIconState.Hiding;
    private Dictionary<MiniMapIconState, RectTransform> _stateAndSettingDic;
    
    public List<Vector2Int> connectGrid;

    private LevelRoom _levelRoom;
    private MiniMapTypeIconManager _miniMapTypeIconManager;
    
    private void Awake()
    {
        _stateAndSettingDic = new Dictionary<MiniMapIconState, RectTransform>
        {
            { MiniMapIconState.Find, _findSetting },
            { MiniMapIconState.Check, _checkSetting }
        };

        _userPoint = _checkSetting.Find("UserPoint").gameObject;
        _doorParent = _checkSetting.Find("Doors") as RectTransform;

        _miniMapTypeIconManager = GetComponentInChildren<MiniMapTypeIconManager>();
        _doorIcons = _doorParent.GetComponentsInChildren<MiniMapDoorIcon>().ToList();

        foreach (MiniMapDoorIcon doorIcon in _doorIcons)
        {
            doorIcon.Init();
            doorIcon.ActiveChange(false);
        }
    }

    public void Init(LevelRoom levelRoom, float paddingValue)
    {
        _levelRoom = levelRoom;
        
        RectTransform rectTransform = transform as RectTransform;
        float rot = levelRoom.transform.localEulerAngles.y;
        rectTransform.localEulerAngles = new Vector3(0, 0, rot);
        
        AutoSetDoorDir();

        connectGrid = levelRoom.connectGrid;

        foreach (var door in levelRoom.OpenDoors)
        {
            MiniMapDoorIcon doorIcon = IsExistDirDoor(door.GetDir());
            if (doorIcon != null)
            {
                doorIcon.ActiveChange(true);
            }
        }
        
        rectTransform.localPosition = new Vector3(levelRoom.GridPos.x * paddingValue, levelRoom.GridPos.y * paddingValue, 0);
    }

    public void UserPointActive(bool active)
    {
        _userPoint.SetActive(active);
    }
    
    private void AutoSetDoorDir()
    {
        foreach (var door in _doorIcons)
        {
            door.SetDir(GetDoorDir(door.transform.right));
        }
    }

    private DoorDir GetDoorDir(Vector2 dir)
    {
        if (dir == Vector2.right)
            return DoorDir.Right;
        if (dir == Vector2.left)
            return DoorDir.Left;
        if (dir == Vector2.up)
            return DoorDir.Top;
    
        return DoorDir.Bottom;
    }

    public void StateChange(MiniMapIconState state)
    {
        if(_currentState == MiniMapIconState.Check)
            return;

        if (_stateAndSettingDic.ContainsKey(_currentState))
            _stateAndSettingDic[_currentState].gameObject.SetActive(false);
        
        _currentState = state;
        _miniMapTypeIconManager.Init(_levelRoom.levelType);
        _stateAndSettingDic[_currentState].gameObject.SetActive(true);
    }
    
    private MiniMapDoorIcon IsExistDirDoor(DoorDir dir) => _doorIcons.Find(levelDoor => dir == levelDoor.DoorDir);
}

