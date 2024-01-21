using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChanger : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vCam;
	private CinemachineBrain brain;

	private void Awake()
	{
		brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (vCam == null) return;
			vCam.Priority = 15;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			if (vCam == null) return;
			vCam.Priority = 10;
		}
	}
}
