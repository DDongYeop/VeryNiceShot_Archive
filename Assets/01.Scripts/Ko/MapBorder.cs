using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBorder : MonoBehaviour
{
    [SerializeField] private Vector3 RespawnPos;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            other.gameObject.transform.position = RespawnPos;
            other.GetComponent<Movement>().StopMovement();
            other.GetComponentInChildren<TrailRenderer>().Clear();
        }
    }
}
