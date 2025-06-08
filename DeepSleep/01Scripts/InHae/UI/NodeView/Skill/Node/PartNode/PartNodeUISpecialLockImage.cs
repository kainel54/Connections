using IH.EventSystem.NodeEvent.SpecialPartNodeEvent;
using UnityEngine;
using UnityEngine.UI;
using YH.EventSystem;

public class PartNodeUISpecialLockImage : MonoBehaviour, IPartNodeUIComponent
{
    [SerializeField] private Image _lockImage;
    private PartNodeUI _partNodeUI;
    
    public void Initialize(PartNodeUI partNodeUI)
    {
        _partNodeUI = partNodeUI;
        _partNodeUI.SpecialModeChangedAction += HandleLockImage;
    }

    private void HandleLockImage(bool isSpecial)
    {
        _lockImage.gameObject.SetActive(isSpecial);
        float alpha = isSpecial ? 0.3f : 1f;
        _partNodeUI.UpdateSlotOnlyAlpha(alpha, true);
    }
}
