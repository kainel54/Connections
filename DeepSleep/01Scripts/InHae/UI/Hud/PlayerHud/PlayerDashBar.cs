using UnityEngine;
using YH.Players;

public class PlayerDashBar : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSO;
    [SerializeField] private RectTransform _visual;
    
    private Player _player;
    
    private void Awake()
    {
        _playerManagerSO.SetUpPlayerEvent += HandleSetUpPlayer;
    }

    private void OnDestroy()
    {
        _player.GetCompo<PlayerMovement>().DashCoolEvent -= HandleDashCooldown;
        _playerManagerSO.SetUpPlayerEvent -= HandleSetUpPlayer;
    }

    private void HandleSetUpPlayer()
    {
        _player = _playerManagerSO.Player;
        _player.GetCompo<PlayerMovement>().DashCoolEvent += HandleDashCooldown;
    }

    private void HandleDashCooldown(float currentCool, float maxCool)
    {
        float fill = currentCool / maxCool;
        fill = Mathf.Clamp01(fill);
        _visual.localScale = new Vector3(1 - fill, _visual.localScale.y, _visual.localScale.z);
    }
}
