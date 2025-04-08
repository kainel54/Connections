using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

namespace YH.Core
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        private Sequence _shakeSequence;

        private CinemachineBasicMultiChannelPerlin _currentMultiChannel;

        private CinemachineVirtualCameraBase _currentCamera;
        public CinemachineVirtualCameraBase currentCamera
        {
            get
            {
                if (_currentCamera == null)
                {
                    CinemachineVirtualCameraBase currentCam = CinemachineCore.GetVirtualCamera(0);
                    _currentCamera = currentCam;
                }

                return _currentCamera;
            }
            private set
            {
                _currentCamera = value;
                currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            }
        }

        private CinemachineBasicMultiChannelPerlin currentMultiChannel
        {
            get
            {
                if (_currentMultiChannel == null)
                    _currentMultiChannel = currentCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
                return _currentMultiChannel;
            }
            set => _currentMultiChannel = value;
        }

        public void ShakeCamera(float amplitude, float frequency, float time, Ease ease = Ease.Linear)
        {
            if (_shakeSequence != null && _shakeSequence.IsActive()) _shakeSequence.Kill();
            _shakeSequence = DOTween.Sequence();

            _shakeSequence
                .Append(
                    DOTween.To(() => amplitude,
                    value => currentMultiChannel.AmplitudeGain = value,
                    0, time).SetEase(ease))
                .Join(
                    DOTween.To(() => frequency,
                    value => currentMultiChannel.FrequencyGain = value,
                    0, time).SetEase(ease));
        }
    }
}


