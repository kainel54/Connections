using Unity.Cinemachine;
using UnityEngine;
using YH.Players;

public class PlayerFollowCam : MonoBehaviour
{
    private CinemachineCamera _vCam;
    
    [SerializeField] private PlayerManagerSO _playerManagerSo;

    private void Awake()
    {
        _vCam = GetComponent<CinemachineCamera>();
        
        _playerManagerSo.SetUpPlayerEvent += HandleCamTargetSetting;
    }

    private void OnDestroy()
    {
        _playerManagerSo.SetUpPlayerEvent -= HandleCamTargetSetting;
    }

    private void HandleCamTargetSetting()
    {
        _vCam.Follow = _playerManagerSo.PlayerTrm;
    }
}
