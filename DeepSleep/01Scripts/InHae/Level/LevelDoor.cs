using System;
using DG.Tweening;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public enum DoorDir
{
    Left,
    Right,
    Bottom,
    Top,
}

public class LevelDoor : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _levelChannel;
    
    private DoorDir _dirType;
    private LevelRoom _parentRoom;
    [HideInInspector] public Transform visual;
    
    private bool _isOpen;

    public Vector3 defaultValue;

    private void Awake()
    {
        visual = transform.Find("Visual");
        defaultValue = visual.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!_parentRoom.isClear || !_isOpen)
            return;
        
        if (other.TryGetComponent(out Player player))
        {
            Move();
        }
    }

    private void Move()
    {
        var evt = LevelEvents.levelMoveEvent;
        evt.enterDoorDir = GetDir();
        _levelChannel.RaiseEvent(evt);
    }

    public void Open() => visual.DOLocalMoveX(3f, 1f);
    public void Close() => visual.DOLocalMoveX(defaultValue.x, 1f);
    public void DoorEnable(bool isActive) => visual.gameObject.SetActive(isActive);
    
    public void SetRoom(LevelRoom room) => _parentRoom = room;
    public void SetOpen(bool isOpen) => _isOpen = isOpen;
    public void SetDir(DoorDir dir) => _dirType = dir;
    public DoorDir GetDir() => _dirType;
}
