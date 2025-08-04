using IH.EventSystem.NodeEvent.PartNodeEvents;
using ObjectPooling;
using UnityEngine;
using YH.EventSystem;

public class SkillNodeContent : BaseNodeContent
{
    [SerializeField] private GameEventChannelSO _partNodeEventChannel;
    
    private void Awake()
    {
        _partNodeEventChannel.AddListener<InViewPortNodeParticleEvent>(HandleViewPortEvent);
    }

    private void OnDestroy()
    {
        _partNodeEventChannel.RemoveListener<InViewPortNodeParticleEvent>(HandleViewPortEvent);
    }
    
    private void HandleViewPortEvent(InViewPortNodeParticleEvent evt)
    {
        Rect thisRect = GetRect(transform as RectTransform);
        Rect nodeRect = GetRect(evt.nodeRectTrm);

        if (thisRect.Contains(nodeRect.min) && thisRect.Contains(nodeRect.max))
        {
            var effect = PoolManager.Instance.Pop(EffectPoolingType.PartNodeEquipEffect) as PartNodeEquipEffect;
            effect.Init(evt.nodeRectTrm, false);
        }
    }

    private Rect GetRect(RectTransform targetRect)
    {
        Vector3[] corners = new Vector3[4];
        targetRect.GetWorldCorners(corners);

        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];

        return new Rect(bottomLeft, topRight - bottomLeft);
    }

}
