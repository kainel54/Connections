using DG.Tweening;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;
using YH.Players;

public class Stage1LevelDoorVisual : LevelDoorVisual
{
    [SerializeField] private GameEventChannelSO _soundChannel;

    [Header("Door Sound")]
    [SerializeField] private SoundSO _openSound;
    [SerializeField] private SoundSO _closeSound;

    [SerializeField] private Transform _leftVisual;
    [SerializeField] private Transform _rightVisual;

    [SerializeField] private float _openAngle;
    [SerializeField] private float _speed;
    
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _door.GetParentRoom().ClearEvent += HandleClearCheck;
    }

    private void OnDestroy()
    {
        _door.GetParentRoom().ClearEvent -= HandleClearCheck;
    }

    private void HandleClearCheck()
    {
        _collider.enabled = false;
        _collider.enabled = true;
    }

    public override void Open()
    {
        _leftVisual.DOLocalRotate(new Vector3(0, _openAngle, 0), _speed);
        _rightVisual.DOLocalRotate(new Vector3(0, -_openAngle, 0), _speed);

        // var evt = SoundEvents.PlaySfxEvent;
        // evt.clipData = _openSound;
        // evt.position = transform.position;

        //_soundChannel.RaiseEvent(evt);
    }

    public override void Close()
    {
        _leftVisual.DOLocalRotate(new Vector3(0, 0, 0), _speed);
        _rightVisual.DOLocalRotate(new Vector3(0, 0, 0), _speed);

        // var evt = SoundEvents.PlaySfxEvent;
        // evt.clipData = _closeSound;
        // evt.position = transform.position;

        //_soundChannel.RaiseEvent(evt);
    }

    public override void DoorEnable(bool isActive)
    {
        _leftVisual.gameObject.SetActive(isActive);
        _rightVisual.gameObject.SetActive(isActive);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent(out Player player) || !_door.CanDoorOpen || _autoOpenDoorCheck)
            return;

        if (_door.GetParentRoom().isAutoOpen)
            _autoOpenDoorCheck = true;
        
        Open();
    }

    private void OnTriggerExit(Collider other)
    {
        if(!_door.CanDoorOpen || _door.GetParentRoom().isAutoOpen)
            return;
        Close();
    }
}
