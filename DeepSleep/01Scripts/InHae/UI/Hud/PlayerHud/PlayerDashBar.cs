using UnityEngine;
using UnityEngine.UI;
using YH.Players;

public class PlayerDashBar : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private Image _countVisual;
    [SerializeField] private Image _chargeVisual;
    [SerializeField] private Camera _uiCamera;
    [SerializeField] private Vector3 _offset;
    
    [SerializeField] private Player _debugPlayer;
    private Player _player;
    private Transform _dashBarTransform;
    
    private RectTransform _rectTransform => transform as RectTransform;
    private Transform _mainCamTrm;

    private void Awake()
    {
        _mainCamTrm = Camera.main.transform;
        
        _player = _debugPlayer;
        _dashBarTransform = _player.transform.Find("DashBarTrm");
        _mainCamTrm = Camera.main.transform;

        if(_player.PlayerInput.ControlType == ControlType.WASD)
        {
            _player.GetCompo<PlayerMovement>().DashCoolEvent += HandleDashCooldown;
        }
        else
        {
            MOBAPlayer player = _player as MOBAPlayer;
            player.DashCoolEvent += HandleDashCooldown;
        }
    }

    private void OnDestroy()
    {
        if (_player.PlayerInput.ControlType == ControlType.WASD)
        {
            _player.GetCompo<PlayerMovement>().DashCoolEvent -= HandleDashCooldown;
        }
        else
        {
            MOBAPlayer player = _player as MOBAPlayer;
            player.DashCoolEvent -= HandleDashCooldown;
        }
    }

    private void LateUpdate()
    {
        if(!gameObject.activeInHierarchy)
            return;
        
        // Vector3 pos = Camera.main.WorldToViewportPoint(_dashBarTransform.position);
        // pos += _offset;
        // _rectTransform.anchoredPosition = pos;
        
        Vector3 camDirection = transform.position - _mainCamTrm.position;
        transform.forward = camDirection;
    }

    private void HandleDashCooldown(float currentCool, float maxCool, int count)
    {
        float countValue = Mathf.InverseLerp(0, 2, count);
        _countVisual.fillAmount = countValue;
        _chargeVisual.fillAmount = countValue + (currentCool / maxCool * 0.5f);
    }
}
