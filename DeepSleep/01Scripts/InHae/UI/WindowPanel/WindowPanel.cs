using UnityEngine;
using YH.EventSystem;

public abstract class WindowPanel : MonoBehaviour
{
    [SerializeField] protected GameEventChannelSO _uiEventChannel;

    public abstract void OpenWindow();
    public abstract void CloseWindow();
}
