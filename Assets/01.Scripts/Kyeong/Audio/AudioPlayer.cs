using System.Collections;
using UnityEngine;

public class AudioPlayer : PoolableMono
{
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public override void Init()
    {
        _audioSource.clip = null;
    }

    public void AudioPlay(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
        StartCoroutine(AudioPlayCo(clip.length + 1));
    }

    public IEnumerator AudioPlayCo(float time)
    {
        yield return new WaitForSeconds(time);
        PoolManager.Instance.Push(this);
    }
}
