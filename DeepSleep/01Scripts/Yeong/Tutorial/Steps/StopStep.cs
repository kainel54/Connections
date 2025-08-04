using UnityEngine;

public class StopStep : TutorialStep
{
    private bool _isStoped;

    public override void OnEnter()
    {
        base.OnEnter();
        _player.StopEvent += HandleStop;
    }

    private void HandleStop()
    {
        _isStoped = true;
        if (_isStoped)
        {
            _player.StopEvent -= HandleStop;
        }
    }
}
