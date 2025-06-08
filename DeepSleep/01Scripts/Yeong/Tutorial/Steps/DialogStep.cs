using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PanelRect
{
    public float top, down, left, right;
    public DirectionType pivot;
    [Header("Unused if Pivot is Zero")]
    public Vector2 size;

    private float _Top => 1080f - top;
    private float _Down => down;
    private float _Left => left;
    private float _Right => 1920f - right;

    public Vector2 GetPosition()
    {
        switch (pivot)
        {
            case DirectionType.Zero:
                return GetCenterPosition();
            case DirectionType.Up:
                return new Vector2(1920f / 2, _Top);
            case DirectionType.UpperRight:
                return new Vector2(_Right, _Top);
            case DirectionType.Right:
                return new Vector2(_Right, 1080f / 2);
            case DirectionType.LowerRight:
                return new Vector2(_Right, _Down);
            case DirectionType.Down:
                return new Vector2(1920f / 2, _Down);
            case DirectionType.LowerLeft:
                return new Vector2(_Left, _Down);
            case DirectionType.Left:
                return new Vector2(_Left, 1080f / 2);
            case DirectionType.UpperLeft:
                return new Vector2(_Left, _Top);
            default:
                return Vector2.zero;
        }
    }

    public Vector2 GetSize()
    {
        return size;
    }

    public Vector2 GetCenterPosition()
    {
        Vector2 panelSize = GetSize();

        if (pivot == DirectionType.Zero)
        {
            return Vector2.zero;
        }
        else
        {
            // 피봇 위치에 따라 중앙 위치를 계산
            Vector2 pivotPos = GetPosition();
            Vector2 finalPos = Vector2.zero;

            switch (pivot)
            {
                case DirectionType.Up:
                    finalPos = new Vector2(pivotPos.x, pivotPos.y - panelSize.y * 0.5f);
                    break;
                case DirectionType.UpperRight:
                    finalPos = new Vector2(pivotPos.x - panelSize.x * 0.5f, pivotPos.y - panelSize.y * 0.5f);
                    break;
                case DirectionType.Right:
                    finalPos = new Vector2(pivotPos.x - panelSize.x * 0.5f, pivotPos.y);
                    break;
                case DirectionType.LowerRight:
                    finalPos = new Vector2(pivotPos.x - panelSize.x * 0.5f, pivotPos.y + panelSize.y * 0.5f);
                    break;
                case DirectionType.Down:
                    finalPos = new Vector2(pivotPos.x, pivotPos.y + panelSize.y * 0.5f);
                    break;
                case DirectionType.LowerLeft:
                    finalPos = new Vector2(pivotPos.x + panelSize.x * 0.5f, pivotPos.y + panelSize.y * 0.5f);
                    break;
                case DirectionType.Left:
                    finalPos = new Vector2(pivotPos.x + panelSize.x * 0.5f, pivotPos.y);
                    break;
                case DirectionType.UpperLeft:
                    finalPos = new Vector2(pivotPos.x + panelSize.x * 0.5f, pivotPos.y - panelSize.y * 0.5f);
                    break;
                default:
                    finalPos = pivotPos;
                    break;
            }
            finalPos -= new Vector2(1920f * 0.5f, 1080f * 0.5f);

            return finalPos;
        }
    }
}

public class DialogStep : TutorialStep
{
    [SerializeField] private List<TutorialInfo> _infoList;
    [SerializeField] private bool _onEnterTextToEmpty = true, _onExitTextToEmpty = false;
    [SerializeField] private bool _callNextStep = true;
    [SerializeField] private float _duration;
    [SerializeField] private string _panelKey;
    private TutorialPanel _panel;

    [SerializeField]
    private PanelRect _panelRect;

    private Vector2 _panelPosition;
    private Vector2 _panelSize;

    public override void Initialize(TutorialManager tutorialManager)
    {
        base.Initialize(tutorialManager);

    }

    public override void OnEnter()
    {
        base.OnEnter();
        if (_onEnterTextToEmpty)
            _panel.SetText("", false, false);
        StartCoroutine(DialogCoroutine());

        _panel = _tutorialManager.GetTutorialPanel(_panelKey);
        if (_panel == null)
        {
            _panel = _tutorialManager.GenerateTutorialPanel(_panelKey);
            _panel.Open(_panelPosition, _panelSize, _duration);
        }
    }

    private IEnumerator DialogCoroutine()
    {
        yield return new WaitForSeconds(_duration);
        foreach (var info in _infoList)
        {
            float duration = info.infoSO.tutorialText.GetAnimationDuration(info.infoSO.textSpeed);
            _panel.SetText(info.infoSO.tutorialText, speed: info.infoSO.textSpeed);
            yield return new WaitForSeconds(duration + info.delay);
        }
        if (_callNextStep)
            _tutorialManager.NextStep();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (_onExitTextToEmpty)
            _panel.SetText("", false, false);
    }
}


public enum DirectionType
{
    Zero,
    Up,
    UpperRight,
    Right,
    LowerRight,
    Down,
    LowerLeft,
    Left,
    UpperLeft,
}
