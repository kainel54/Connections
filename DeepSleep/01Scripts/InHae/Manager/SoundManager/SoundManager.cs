using ObjectPooling;
using UnityEngine;
using YH.EventSystem;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _soundChannel;

    private SoundPlayer _currentBGMPlayer = null;
    private SoundPlayer _currentSFXPlayer = null;
    
    private void Awake()
    {
        _soundChannel.AddListener<PlaySFXEvent>(HandlePlaySFXEvent);
        _soundChannel.AddListener<PlayBGMEvent>(HandlePlayBGMEvent);
        _soundChannel.AddListener<StopBGMEvent>(HandleStopBGMEvent);
        _soundChannel.AddListener<PlayLoopSFXEvent>(HandlePlayLoopSFXEvent);
        _soundChannel.AddListener<StopLoopSFXEvent>(HandleStopLoopSFXEvent);
    }
    
    private void OnDestroy()
    {
        _soundChannel.RemoveListener<PlaySFXEvent>(HandlePlaySFXEvent);
        _soundChannel.RemoveListener<PlayBGMEvent>(HandlePlayBGMEvent);
        _soundChannel.RemoveListener<StopBGMEvent>(HandleStopBGMEvent);
        _soundChannel.RemoveListener<PlayLoopSFXEvent>(HandlePlayLoopSFXEvent);
        _soundChannel.RemoveListener<StopLoopSFXEvent>(HandleStopLoopSFXEvent);
    }

    private void HandleStopBGMEvent(StopBGMEvent evt)
    {
        _currentBGMPlayer?.StopAndGotoPool(true); //fade아웃 시켜서 보내고
        _currentBGMPlayer = null;
    }

    private void HandlePlayBGMEvent(PlayBGMEvent evt)
    {
        _currentBGMPlayer?.StopAndGotoPool(true); //fade아웃 시켜서 보내고
        _currentBGMPlayer = PoolManager.Instance.Pop(PoolingType.SoundPlayer) as SoundPlayer;
        _currentBGMPlayer.PlaySound(evt.clipData);
    }
    
    private void HandleStopLoopSFXEvent(StopLoopSFXEvent evt)
    {
        _currentSFXPlayer?.StopAndGotoPool(true); //fade아웃 시켜서 보내고
        _currentSFXPlayer = null;
    }
    
    private void HandlePlayLoopSFXEvent(PlayLoopSFXEvent evt)
    {
        _currentSFXPlayer?.StopAndGotoPool(true); //fade아웃 시켜서 보내고
        _currentSFXPlayer = PoolManager.Instance.Pop(PoolingType.SoundPlayer) as SoundPlayer;
        _currentSFXPlayer.PlaySound(evt.clipData);
    }

    private void HandlePlaySFXEvent(PlaySFXEvent evt)
    {
        SoundPlayer player = PoolManager.Instance.Pop(PoolingType.SoundPlayer) as SoundPlayer;
        player.transform.position = evt.position;
        player.PlaySound(evt.clipData);
    }
}
