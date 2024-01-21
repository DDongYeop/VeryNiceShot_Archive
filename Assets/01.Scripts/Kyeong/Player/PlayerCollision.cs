using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private AudioClip _whiteBlockBreakClip;
    
    private bool _isHighShotCollision = false;
    private MeshRenderer _meshRenderer;
    
    private readonly int _hashDissolved = Shader.PropertyToID("_Dissolved");

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

	private void OnCollisionEnter(Collision other)
    {
        other.transform.TryGetComponent(out WhiteBlock whiteBlock);
        ContactPoint contact = other.contacts[0];
        if (whiteBlock != null)
        {
            Transform obj = PoolManager.Instance.Pop("CollisionParticle").transform;
            obj.position = contact.point;
            obj.LookAt(transform.position);
            SoundManager.Instance.PlaySound(_whiteBlockBreakClip);
        }
        
        if (!_isHighShotCollision)
            return;
        _isHighShotCollision = false;
        Transform circle = PoolManager.Instance.Pop("CircleParticle").transform;
        circle.position = contact.point + (contact.normal * 0.2f);
        circle.rotation = Quaternion.LookRotation(contact.normal);
    }

    public void HighShotStart(bool value)
    {
        _isHighShotCollision = value;
    }

    /// <summary>
    /// Player Dissolve 해주는 함수
    /// </summary>
    /// <param name="value"> -1~1 사이로 해주어야함. </param>
    /// <param name="duration"> 변경 되는데 걸리는 시간. </param>
    public void PlayerDissolve(float value, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(PlayerDissolveCo(value, duration));
    }


    public IEnumerator PlayerDissolveCo(float value, float duration)
    {
        float currentTime = 0;
        float startValue = _meshRenderer.material.GetFloat(_hashDissolved);
        while (currentTime < duration)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float time = currentTime / duration;
            _meshRenderer.material.SetFloat(_hashDissolved, Mathf.Lerp(startValue, value, time));
        }
        
        _meshRenderer.material.SetFloat(_hashDissolved, value);
    }
}
