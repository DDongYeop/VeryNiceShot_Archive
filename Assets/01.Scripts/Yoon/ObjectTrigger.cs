using UnityEngine.Events;
using UnityEngine;

public class ObjectTrigger : MonoBehaviour
{
    public UnityEvent collisionEvent;

    private bool isEvented = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) && (false == isEvented))
        {
            isEvented = true;
            collisionEvent?.Invoke();
        }
    }
}
