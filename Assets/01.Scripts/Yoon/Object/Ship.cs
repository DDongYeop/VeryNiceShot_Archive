using System.Collections;
using UnityEngine;

public sealed class Ship : ObjectMovement
{
    [SerializeField] private float speed;
    [SerializeField] private float animationSpeed;

    private GameObject lastHitTrigger;

    private Vector3 eulerAngle = new Vector3(90f, 0f, 0f);

    public override void Active(bool active)
    {
        if (active)
        {
            // transform.position = initPos;
        }
    }

    protected override void SetDirectionAndSpeed()
    {
        moveDirection = Vector3.forward;
        moveSpeed = speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TurnArea") && lastHitTrigger != other.gameObject)
        {
            lastHitTrigger = other.gameObject;
            StartCoroutine(RotateCor(eulerAngle, animationSpeed));
        }
    }

    private IEnumerator RotateCor(Vector3 angle, float lerpSpeed)
    {
        float moveTime = 0;
        float value = 0;

        Quaternion targetRotation = transform.rotation * Quaternion.Euler(angle);

        while (moveTime <= lerpSpeed)
        {
            moveTime += Time.deltaTime;
            value = moveTime / lerpSpeed;

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, value);

            yield return null;
        }

        transform.rotation = targetRotation;
    }
}