using System;
using UnityEngine;
using YH.Players;

[Serializable]
public class TutorialInfo
{
    public TutorialInfoSO infoSO;
    public float delay;
}

public abstract class TutorialStep : MonoBehaviour
{
    protected MOBAPlayer _player;

    [SerializeField]
    private TutorialStep[] _withActiveStep;


    public virtual void OnEnter()
    {
        //foreach (var step in _withActiveStep)
        // _tutorialManager.SetActiveStep(step, true);
    }
    public virtual void OnUpdate() { }
    public virtual void OnExit()
    {
        // foreach (var step in _withActiveStep)
        //_tutorialManager.SetActiveStep(step, false);
    }
}
