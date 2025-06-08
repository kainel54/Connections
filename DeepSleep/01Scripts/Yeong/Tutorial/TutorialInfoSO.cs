using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TutorialStepSO", menuName = "SO/TutorialStepSO")]
public class TutorialInfoSO : ScriptableObject
{
    [TextArea]
    public string tutorialText;
    [Tooltip("초당 나올 글자 수")]
    public float textSpeed = 15f;
}
