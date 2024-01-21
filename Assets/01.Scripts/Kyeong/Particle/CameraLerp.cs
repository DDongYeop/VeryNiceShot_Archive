using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraLerp : MonoBehaviour
{
    [SerializeField] private float _duraction;                  //전환되는데 걸리는 시간  
    [SerializeField] private float _bigSize;                  //몇에서 몇으로 전환 될지 x -> y
    private float _smallSize;
    private CinemachineVirtualCamera _cinemachineVirtualCamera;

    private void Awake()
    {
        _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    private IEnumerator Start()
    {
        _smallSize = _cinemachineVirtualCamera.m_Lens.OrthographicSize;
        _cinemachineVirtualCamera.m_Lens.OrthographicSize = _bigSize;

        while (!Input.GetMouseButton(0)) 
            yield return null;

        float currentTime = 0;
        while (currentTime < _duraction)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float time = currentTime / _duraction;
            time = 1 - Mathf.Pow(1 - time, 3);
            _cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(_bigSize, _smallSize, time);
        }
        _cinemachineVirtualCamera.m_Lens.OrthographicSize = _smallSize;
    }
}
