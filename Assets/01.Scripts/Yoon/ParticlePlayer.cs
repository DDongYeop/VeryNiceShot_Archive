using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    private ParticleSystem effect;

    private void Awake()
    {
        effect = GetComponent<ParticleSystem>();
    }

    public void StartPlay()
    {
        effect.Play();
    }

    public void StopPlay()
    {
        effect.Stop();
    }
}
