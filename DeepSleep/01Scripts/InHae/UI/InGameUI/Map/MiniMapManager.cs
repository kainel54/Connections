using System.Collections.Generic;
using IH.EventSystem.LevelEvent;
using UnityEngine;
using UnityEngine.Serialization;
using YH.EventSystem;

public class MiniMapManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _levelEventChannel;
    [SerializeField] private RectTransform _mapParent;
    [FormerlySerializedAs("_miniMapIconObj")] [SerializeField] private MiniMapLevelIcon miniMapLevelIconObj;

    [SerializeField] private float _paddingValue = 50f;
    
    private Dictionary<Vector2Int, MiniMapLevelIcon> _mapIcons = new Dictionary<Vector2Int, MiniMapLevelIcon>();
    private Vector2Int _currentPoint;
    
    private void Awake()
    {
        _levelEventChannel.AddListener<LevelDataPassEvent>(HandleDataPassEvent);
        _levelEventChannel.AddListener<LevelMoveCompleteEvent>(HandleLevelMoveCompleteEvent);
    }
    
    private void OnDestroy()
    {
        _levelEventChannel.RemoveListener<LevelDataPassEvent>(HandleDataPassEvent);
        _levelEventChannel.RemoveListener<LevelMoveCompleteEvent>(HandleLevelMoveCompleteEvent);
    }
    
    private void HandleDataPassEvent(LevelDataPassEvent evt)
    {
        foreach (KeyValuePair<Vector2Int, LevelRoom> levelRoom in evt.levelGridDictionary)
        {
            MiniMapLevelIcon miniMapLevelIcon = Instantiate(miniMapLevelIconObj, _mapParent);
            
            miniMapLevelIcon.Init(levelRoom.Value, _paddingValue);
            _mapIcons.Add(levelRoom.Key, miniMapLevelIcon);
        }

        _currentPoint = Vector2Int.zero;
        _mapIcons[_currentPoint].UserPointActive(true);
        
        transform.localEulerAngles = new Vector3(0, 0, -45f);

        MiniMapUpdate();
        IconStateUpdate();
    }
    
    private void HandleLevelMoveCompleteEvent(LevelMoveCompleteEvent evt)
    {
        _mapIcons[_currentPoint].UserPointActive(false);
        _currentPoint = evt.currentPoint;
        _mapIcons[_currentPoint].UserPointActive(true);
        
        Vector2 pos = _mapIcons[_currentPoint].transform.localPosition;
        pos *= -1;
        
        _mapParent.localPosition = pos;

        MiniMapUpdate();
        IconStateUpdate();
    }

    private void IconStateUpdate()
    {
        _mapIcons[_currentPoint].StateChange(MiniMapIconState.Check);
        
        foreach (var grid in _mapIcons[_currentPoint].connectGrid)
        {
            _mapIcons[grid].StateChange(MiniMapIconState.Find);
        }
    }

    private void MiniMapUpdate()
    {
        foreach (KeyValuePair<Vector2Int,MiniMapLevelIcon> icon in _mapIcons)
        {
            if (Vector2Int.Distance(icon.Key, _currentPoint) >= 2f)
                icon.Value.gameObject.SetActive(false);
            else
                icon.Value.gameObject.SetActive(true);
        }
    }
}
