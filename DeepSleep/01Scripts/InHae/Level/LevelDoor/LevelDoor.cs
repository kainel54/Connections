using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DoorDir
{
    Left,
    Right,
    Bottom,
    Top,
}

public class LevelDoor : MonoBehaviour
{
    public Transform spawnPoint;
    [SerializeField] private GameObject _openObj;    
    [SerializeField] private GameObject _closeObj;    
    
    private DoorDir _dirType;
    private LevelRoom _parentRoom;
    [HideInInspector] public bool isOpen;
    
    private Dictionary<Type, ILevelDoorCompo> _doorCompos = new ();
    private LevelDoorVisual _doorVisual;
    
    public bool CanDoorOpen => isOpen && _parentRoom.isClear;

    private void Awake()
    {
        GetComponentsInChildren<ILevelDoorCompo>().ToList().ForEach(x=>
        {
            x.Initialize(this);
            _doorCompos.Add(x.GetType(), x);
        });

        _doorVisual = GetComponentInChildren<LevelDoorVisual>();
    }

    public void InvalidateMode()
    {
        _closeObj.SetActive(true);
        _openObj.SetActive(false);
    }

    public void Open() => _doorVisual.Open();
    public void Close() => _doorVisual.Close();
    public void DoorEnable(bool isActive) => _doorVisual.DoorEnable(isActive); 

    public void SetRoom(LevelRoom room) => _parentRoom = room;
    public void SetOpen(bool isOpen) => this.isOpen = isOpen;
    
    public void SetDir(DoorDir dir) => _dirType = dir;
    public DoorDir GetDir() => _dirType;
    
    public LevelRoom GetParentRoom() => _parentRoom;
}
