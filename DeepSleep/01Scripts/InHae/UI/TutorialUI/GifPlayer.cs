using System;
using UnityEngine;
using UnityEngine.UI;

public class GifPlayer : MonoBehaviour
{
    [SerializeField] private TutorialHelpUI _helpUi;
    [SerializeField] private Texture2D[] _frames;
    private RawImage _gifShowImage;

    public float frameRate = 15f;

    private int index = 0;
    private float timer = 0f;

    private bool _playGif = false;

    private void Start()
    {
        _gifShowImage = GetComponent<RawImage>();
        _playGif = true;
        _helpUi.closeAction += HandleStopPlaying;
    }

    private void OnDestroy()
    {
        _helpUi.closeAction -= HandleStopPlaying;
    }

    private void HandleStopPlaying()
    {
        _playGif = false;
    }

    void Update()
    {
        if (!_playGif) return;

        timer += Time.deltaTime;
        if (timer >= 0.5f / frameRate)
        {
            index = (index + 1) % _frames.Length;
            _gifShowImage.texture = _frames[index];
            timer = 0f;
        }
    }

}


