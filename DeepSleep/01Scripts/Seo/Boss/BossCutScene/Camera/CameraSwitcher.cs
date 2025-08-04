using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;


[System.Serializable]
public struct CinemachineCamData
{
    [Tooltip("여기에 보여줄 카메라를 설정")]
    public CinemachineCamera camera;
    [Tooltip("이전 카메라로부터 블렌드될 방법을 설정")]
    public CinemachineBlendDefinition.Styles switchType;
    [Tooltip("이 카메라가 얼마동안 유지될건지 설정")]
    public float holdTime;
    [Tooltip("얼마동안 블렌딩이 될건지 설정")]
    public float switchTime;
    [Tooltip("얼마동안 카메라 유지 하며 대기할 건지 설정")]
    public float waitTime;
}
public class CameraSwitcher : MonoBehaviour
{
    public Action<CinemachineCamera, float> CurrentCamStarted;

    private CinemachineBrain _cinemachineBrain;

    [SerializeField] private CinemachineCamData[] _cinemachineCams;

    private List<CinemachineBlenderSettings.CustomBlend> customBlend = new();
    public void StartCinematic()
    {

        _cinemachineBrain = FindAnyObjectByType<CinemachineBrain>();

        for (int i = 1; i < _cinemachineCams.Length; i++)
        {
            CinemachineCamData beforeData = _cinemachineCams[i - 1];
            CinemachineCamData currentData = _cinemachineCams[i];
            SettingBlending(beforeData.camera, currentData.camera, currentData.switchType, currentData.holdTime);
        }

        CinemachineBlenderSettings blendSetting = new CinemachineBlenderSettings()
        {
            CustomBlends = customBlend.ToArray()
        };
        _cinemachineBrain.CustomBlends = blendSetting;  // 이걸로 어떤 카메라에 따라 변경하는 Ease 라던가 가 바뀐다. 

        StartCoroutine(CamSwitching());
    }

    public void SetCameraBrainSetting(bool starting)
    {
        if (starting)
        {
            _cinemachineBrain.IgnoreTimeScale = true;
            _cinemachineBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.Cut;
        }
        else
        {
            _cinemachineBrain.IgnoreTimeScale = false;
            _cinemachineBrain.DefaultBlend.Style = CinemachineBlendDefinition.Styles.EaseInOut;
            _cinemachineBrain.DefaultBlend.Time = 2f;
        }
    }


    private void SettingBlending(CinemachineCamera currentCam, CinemachineCamera nextCam, CinemachineBlendDefinition.Styles switchType, float holdTime)
    {
        customBlend.Add(new CinemachineBlenderSettings.CustomBlend
        {
            From = currentCam.Name,
            To = nextCam.Name,
            Blend = new CinemachineBlendDefinition(style: switchType, time: holdTime)
        });
    }

    private IEnumerator CamSwitching()
    {
        for (int i = 0; i < _cinemachineCams.Length; i++)
        {
            _cinemachineCams[i].camera.Priority = 20;
            yield return new WaitForSecondsRealtime(_cinemachineCams[i].switchTime);
            CurrentCamStarted?.Invoke(_cinemachineCams[i].camera, _cinemachineCams[i].holdTime);
            yield return new WaitForSecondsRealtime(_cinemachineCams[i].holdTime + _cinemachineCams[i].waitTime);

            if (i < _cinemachineCams.Length - 1)
            {
                _cinemachineCams[i].camera.Priority = 10;
            }
        }
        yield return null;
    }
}
