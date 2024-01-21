using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Debug.LogError("Multiple SoundManager is running");
        Instance = this;
    }

    public void PlaySound(AudioClip clip) 
    {
        AudioPlayer audioPlayer = PoolManager.Instance.Pop("AudioPlayer") as AudioPlayer;
        audioPlayer.AudioPlay(clip);
    }
}
