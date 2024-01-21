using UnityEngine;

public class Train : ObjectMovement
{
    [SerializeField] private float speed;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public override void Active(bool active)
    {
        if (active)
        {
            transform.position = initPos;
        }
    }

    protected override void SetDirectionAndSpeed()
    {
        moveDirection = Vector3.forward;
        moveSpeed = speed;
    }

    protected override void MoveStop()
    {
        base.MoveStop();
        _rigidBody.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EventTrigger"))
        {
            MoveStop();
        }
    }
}
