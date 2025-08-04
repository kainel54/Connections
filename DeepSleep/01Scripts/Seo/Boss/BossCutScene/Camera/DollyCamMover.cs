using System;
using Unity.Cinemachine;
using UnityEngine;

public class DollyCamMover : MonoBehaviour
{
    private CameraSwitcher _cameraSwitcher;
    private CinemachineSplineDolly _currentCamera;

    private float _holdingCamTime;
    private bool _camStartCam = false;
    void Start()
    {
        _currentCamera = GetComponent<CinemachineSplineDolly>();
        _cameraSwitcher = transform.parent.GetComponent<CameraSwitcher>();

        _cameraSwitcher.CurrentCamStarted += HandleStartDollyCamEvent;
    }

    private void HandleStartDollyCamEvent(CinemachineCamera currnetCam, float holdTime) // TODO_SE 여기 이거 구조 좀 너무 많은 데이터를 제공 하는 거 같음 그리고 처음에 모두 구독을 하니까 모두 이 함수가 실행됨.
    {                                                                                   // 그래서 현재 사용되는 카메라를 스위처에서 이 코드 함수를 실행 시키게 하는게 좋을 거 같음.
        if (currnetCam == _currentCamera.VirtualCamera)
        {
            _camStartCam = true;
            _holdingCamTime = holdTime;
        }
    }

    void Update()
    {
        if (_camStartCam)
        {

            _currentCamera.CameraPosition += 1f / _holdingCamTime * Time.unscaledDeltaTime;
            if (_currentCamera.CameraPosition >= 1f)
            {
                _camStartCam = false;
                _cameraSwitcher.CurrentCamStarted -= HandleStartDollyCamEvent;
            }
        }
    }
}
