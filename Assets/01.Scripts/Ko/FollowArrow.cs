using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowArrow : MonoBehaviour
{
    [Tooltip("음수면 오브젝트가 삭제 X")][SerializeField] private float m_lifeTime;
    [SerializeField] private Vector3 m_offset;

    [SerializeField] private float m_duration;
    [SerializeField] private float m_movementValue;
    private Transform m_playerPos;
    
    private void Start()
    {
        m_playerPos = GameObject.Find("Player").transform;

        if(m_lifeTime >= 0)
            Destroy(gameObject, m_lifeTime);

        StartCoroutine(MovementCo(1));
    }


    private IEnumerator MovementCo(int direction)
    {
        float currentTime = 0;
        Vector3 startPos = m_playerPos.position + m_offset;
        Vector3 endPos = startPos;
        endPos.y += direction * m_movementValue;

        while (currentTime < m_duration)
        {
            yield return null;
            startPos = m_playerPos.position + m_offset;
            endPos = startPos;
            endPos.y += direction * m_movementValue;

            currentTime += Time.deltaTime;
            float time = currentTime / m_duration;
            transform.position = Vector3.Lerp(startPos, endPos, time);
        }

        //transform.position = endPos;
        StartCoroutine(MovementCo(direction * -1));
    }
}
