using UnityEngine;

[CreateAssetMenu(menuName = "SO/Sound/ClipSO")]
public class SoundSO : ScriptableObject
{
    public enum AudioType
    {
        SFX, Music
    }

    public AudioType audioType;
    public AudioClip clip;
    public bool isLoop = false;
    public bool isRandomizePitch = false;
    public bool is3DSound;

    [Range(0, 1f)]
    public float randomPitchModifier = 0.1f;
    [Range(0.1f, 1f)]
    public float volume = 1f;
    [Range(0.1f, 3f)] 
    public float pitch = 1f;
    
    // 스타트 시간
    public float startTime = 0f;
}
