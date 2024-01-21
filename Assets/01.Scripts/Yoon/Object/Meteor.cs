using UnityEngine;

public sealed class Meteor : ObjectMovement
{
    [SerializeField] private float speed;
    [SerializeField] GameObject explosionEffect;

    private Rigidbody _rigidBody;
    private ParticlePlayer _particlePlayer;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _particlePlayer = GetComponentInChildren<ParticlePlayer>();
    }

    // 해당 오브젝트가 재생성 됐을 때 (초기화)
    public override void Active(bool active)
    {
        if (active)
        {
            // explosionEffect.SetActive(false);
            // _rigidBody.isKinematic = false;
            // transform.position = initPos;
        }
    }

    protected override void SetDirectionAndSpeed()
    {
        moveDirection = Vector3.down;
        moveSpeed = speed;
    }

    protected override void MoveStart()
    {
        base.MoveStart();
        _particlePlayer.StartPlay();
    }

    protected override void MoveStop()
    {
        base.MoveStop();
        _particlePlayer.StopPlay();
        explosionEffect.SetActive(true);
        _rigidBody.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            MoveStop();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.localRotation * Vector3.down * transform.position.y);
        Gizmos.DrawWireSphere(transform.rotation * Vector3.down * (-1f), 3f);
    }
}
