using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStepSO", menuName = "SO/TutorialStepSO")]
public class TutorialInfoSO : ScriptableObject
{
    [TextArea]
    public string tutorialText;
    [Tooltip("�ʴ� ���� ���� ��")]
    public float textSpeed = 15f;
}
