using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum TutorialUIType
{
    // bottom of this enum I'll managed for index in inspector TutorialUI
    // <if you have to touch this or have to touch HelpUI hierarchy say to me>
    Move = 0,   //움직임
    Attack,     //공격
    Dash,       //대쉬
    Skill,      //스킬 상자에서 나와서 얻고 Skill사용
    SkillInfo,  //스킬 파츠 장착 설명 
    Interactive,//업그레이드

}
public class TutorialHelpUI : MonoBehaviour
{
    private CanvasGroup _helpCanvasGroup;
    [SerializeField] private RectTransform _helpPanel;
    [SerializeField] private Transform _helpDescriptionParentPanel;
    [SerializeField] private Image _background;

    private GameObject _currentDescriptionObj;
    private Vector2 _sizeDelta;
    private bool _isHelpPanelOpened = false;

    public event Action closeAction;

    private void Start()
    {
        _helpCanvasGroup = GetComponent<CanvasGroup>();
        _sizeDelta = _helpPanel.sizeDelta;

        _helpCanvasGroup.interactable = false;
        _helpCanvasGroup.blocksRaycasts = false;
        _helpPanel.DOSizeDelta(new Vector2(0, _sizeDelta.y), 0);
        _background.DOFade(0, 0);
    }
    public void OpenHelpPanel(TutorialUIType type)
    {
        if (_isHelpPanelOpened) return;
        ShowTutorial(type);

        _helpPanel.DOSizeDelta(_sizeDelta, 0.3f);
        _background.DOFade(0.8f, 0.3f).OnComplete(
            () =>
            {
                _helpCanvasGroup.interactable = true;
                _helpCanvasGroup.blocksRaycasts = true;
                _isHelpPanelOpened = true;
            });
    }

    public void CloseHelpPanel()
    {
        if (_isHelpPanelOpened == false) return;

        _isHelpPanelOpened = false;
        _helpCanvasGroup.blocksRaycasts = false;
        _helpCanvasGroup.interactable = false;

        _helpPanel.DOSizeDelta(new Vector2(0, _sizeDelta.y), 0.3f);
        _background.DOFade(0, 0.3f);

        closeAction?.Invoke();
    }

    private void ShowTutorial(TutorialUIType uiType)
    {
        GameObject curIdxObj = _helpDescriptionParentPanel.GetChild((int)uiType).gameObject;

        if (_currentDescriptionObj == null)
        {
            _currentDescriptionObj = curIdxObj;
            _currentDescriptionObj.SetActive(true);
            return;
        }
        ChangeTutoObj(curIdxObj);
    }

    private void ChangeTutoObj(GameObject changeObj)
    {
        _currentDescriptionObj.SetActive(false);
        _currentDescriptionObj = changeObj;
        _currentDescriptionObj.SetActive(true);
    }
}
