using System;
using System.Collections;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float _duration;
    [SerializeField] private float _movementValue;

    private void Start()
    {
        StartCoroutine(MovementCo(1));
    }

    private IEnumerator MovementCo(int direction)
    {
        float currentTime = 0;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos;
        endPos.y += direction * _movementValue;
        
        while (currentTime < _duration)
        {
            yield return null;
            currentTime += Time.deltaTime;
            float time = currentTime / _duration;
            transform.position = Vector3.Lerp(startPos, endPos, time);
        }

        transform.position = endPos;
        StartCoroutine(MovementCo(direction * -1));
    }
}
