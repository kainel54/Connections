using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerStatusIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _timerText;

    private char infiniteModeChar = '∞';
    private bool _isTimerMode;
    private bool _isReInit;
    
    private StatusStat _currentStat;
    private StatusDataSO _statusData;
    public event Action<StatusEnum> onStatusEnd; 

    private void OnDestroy()
    {
        onStatusEnd?.Invoke(_statusData.status);
        if (_isTimerMode)
            _currentStat.updateEvent -= HandleStatusTimer;
        
        if(_isReInit)
            _currentStat.endEvent -= HandleEndEvent;
    }

    public void Init(StatusStat stat ,StatusDataSO statusData, bool infiniteMode)
    {
        _currentStat = stat;
        _statusData= statusData;
        _icon.sprite = statusData.icon;

        if (infiniteMode)
        {
            if (_isTimerMode)
            {
                _isTimerMode = false;
                _currentStat.updateEvent -= HandleStatusTimer;
            }
            
            _timerText.SetText(infiniteModeChar.ToString());
        }
        else
        {
            _isTimerMode = true;
            _currentStat.updateEvent += HandleStatusTimer;
        }

        if (_isReInit)
            return;
        
        _isReInit = true;
        _currentStat.endEvent += HandleEndEvent;
    }

    public void RemoveStatus()
    {
        var popUp = UIHelper.Instance.GetPlayerStatusTooltip();
        popUp.EndPopUp();
        
        _currentStat.RemoveStatus();
    }

    private void HandleEndEvent()
    {
        var popUp = UIHelper.Instance.GetPlayerStatusTooltip();
        popUp.EndPopUp();
        
        Destroy(gameObject);
    }

    private void HandleStatusTimer(float current, float total)
    {
        _timerText.SetText(current <= 0 ? "" : current.ToString("F1"));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var popUp = UIHelper.Instance.GetPlayerStatusTooltip();
        
        Vector3 pos = eventData.position;
        pos.x += 10;
        pos.y -= 5;
        popUp.transform.position = pos;
        
        popUp.Init(_statusData);
        popUp.OnPopUp();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var popUp = UIHelper.Instance.GetPlayerStatusTooltip();
        popUp.EndPopUp();
    }
}
