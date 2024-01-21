using UnityEngine;

public class HumanRotate : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.down * rotateSpeed * Time.deltaTime);
    }
}
