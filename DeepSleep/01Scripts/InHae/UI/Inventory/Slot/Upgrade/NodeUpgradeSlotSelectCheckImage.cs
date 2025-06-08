using UnityEngine;
using YH.EventSystem;

public abstract class NodeUpgradeSlotSelectCheckImage : MonoBehaviour
{
    [Header("Default Upgrade or Special Upgrade")]
    [SerializeField] protected GameEventChannelSO _upgradeEventChannel;

    protected void Init(RectTransform targetTrm)
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);
        
        transform.SetParent(targetTrm);
        transform.localPosition = Vector3.zero;
        (transform as RectTransform).sizeDelta = targetTrm.sizeDelta;
        transform.localScale = Vector3.one * 0.8f;
    }
}
