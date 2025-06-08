using IH.EventSystem.LevelEvent;
using UnityEngine;
using YH.EventSystem;

public class BarrierTrigger : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _startStageEventChannel;

    private void OnEnable()
    {
       // _startStageEventChannel.AddListener<StageStartEvent>((evt) => Destroy(this.gameObject));
    }

    private void OnDestroy()
    {
        //.RemoveListener<StageStartEvent>((evt) => Destroy(this.gameObject));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
           //var evt = LevelEvents.StageStartEvent;
           //
           //_startStageEventChannel.RaiseEvent(evt);
        }
    }
}
