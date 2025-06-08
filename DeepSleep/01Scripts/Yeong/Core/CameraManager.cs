using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace YH.Core
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        private List<ShakeInfo> _activeShakes = new List<ShakeInfo>();

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

        private class ShakeInfo
        {
            public float StartAmplitude;
            public float StartFrequency;
            public float Duration;
            public float Elapsed;
            public Ease Ease;
        }

        private bool isShaking = false;

        public void ShakeCamera(float amplitude, float frequency, float time, Ease ease = Ease.Linear)
        {
            // ���ο� ����ŷ�� ����Ʈ�� �߰�
            _activeShakes.Add(new ShakeInfo
            {
                StartAmplitude = amplitude,
                StartFrequency = frequency,
                Duration = time,
                Elapsed = 0f,
                Ease = ease
            });

            // ����ŷ�� ������Ʈ�� �ڷ�ƾ ����
            if (!isShaking)
            {
                isShaking = true;
                StartCoroutine(UpdateShake());
            }
        }

        private IEnumerator UpdateShake()
        {
            while (_activeShakes.Count > 0)
            {
                float maxAmplitude = 0f;
                float maxFrequency = 0f;

                for (int i = _activeShakes.Count - 1; i >= 0; i--)
                {
                    ShakeInfo shake = _activeShakes[i];
                    shake.Elapsed += Time.deltaTime;

                    // �ð��� ���� ����ŷ �� ���
                    float time = Mathf.Clamp01(shake.Elapsed / shake.Duration);
                    float eval = DOVirtual.EasedValue(1f, 0f, time, shake.Ease);

                    float currentAmp = shake.StartAmplitude * eval;
                    float currentFreq = shake.StartFrequency * eval;

                    // �ִ� ������ ���ļ� ���� ����
                    if (currentAmp > maxAmplitude) maxAmplitude = currentAmp;
                    if (currentFreq > maxFrequency) maxFrequency = currentFreq;

                    // ����ŷ�� ���� ��� ����Ʈ���� ����
                    if (shake.Elapsed >= shake.Duration)
                        _activeShakes.RemoveAt(i);
                }

                // �ִ밪�� ����
                currentMultiChannel.AmplitudeGain = maxAmplitude;
                currentMultiChannel.FrequencyGain = maxFrequency;

                yield return null;
            }

            // ����ŷ�� ���� �� ���� ������ ����
            currentMultiChannel.AmplitudeGain = 0f;
            currentMultiChannel.FrequencyGain = 0f;
            isShaking = false;
        }
    }
}
