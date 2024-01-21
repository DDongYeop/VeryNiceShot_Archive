using System.Collections;
using UnityEngine;
using System;

public abstract class ObjectMovement : LevelObject
{
    [Header("Option")]
    [SerializeField] private bool moveOnAwake;

    private Action timeToEvent;

    protected Vector3 moveDirection = Vector3.zero;
    protected float moveSpeed = 0;

    private Coroutine activateCoroutine = null;

    protected Vector3 initPos;

    // MoveDirection, MoveSpeed Setting
    protected abstract void SetDirectionAndSpeed();

    protected virtual void Start()
    {
        initPos = transform.position;

        SetDirectionAndSpeed();

        if (moveOnAwake)
        {
            MoveStart();
        }
        else
        {
            timeToEvent += MoveStart;
        }
    }

    protected virtual void MoveStart()
    {
        if (activateCoroutine != null)
        {
            MoveStop();
        }
        activateCoroutine = StartCoroutine(MoveCor());
    }

    protected virtual void MoveStop()
    {
        if (activateCoroutine != null)
        {
            StopCoroutine(activateCoroutine);
        }
    }

    public void EventExcute()
    {
        timeToEvent?.Invoke();
    }

    private IEnumerator MoveCor()
    {
        while (true)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

}
