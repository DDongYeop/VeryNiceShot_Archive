using UnityEngine;

public class StarParticle : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;

    private void OnEnable()
    {
        SoundManager.Instance.PlaySound(_audioClip);
    }
}
