using DG.Tweening;
using IH.EventSystem.SystemEvent;
using UnityEngine;
using UnityEngine.SceneManagement;
using YH.EventSystem;

public class ClearPortal : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _systemEventChannel;
    
    private Collider _collider;
    private bool _isTriggered;

    private void Awake()
    {
        _collider = GetComponent<Collider>();        
    }

    public void Init()
    {
        gameObject.SetActive(true);
        transform.DOScale(Vector3.one, 2f).OnComplete(() => _collider.enabled = true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(_isTriggered)
                return;
            
            _isTriggered = true;
            _systemEventChannel.AddListener<FadeComplete>(HandleFadeComplete);
            
            var evt = SystemEvents.FadeScreenEvent;
            evt.fadeDuration = 0.5f;
            evt.isCircle = true;
            evt.isFadeIn = false;

            _systemEventChannel.RaiseEvent(evt);
        }
    }

    private void HandleFadeComplete(FadeComplete evt)
    {
        _systemEventChannel.RemoveListener<FadeComplete>(HandleFadeComplete);
        SceneManager.LoadScene("TitleScene");
    }
}
