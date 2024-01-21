using UnityEngine;

public class Tutorial3 : MonoBehaviour
{
	private void Start()
	{
		GameManager.Instance.transform.Find("Canvas/ShotMode").gameObject.SetActive(false);
	}

	public void TurnOff()
	{
		GameManager.Instance.transform.Find("Canvas/ShotMode").gameObject.SetActive(true);
	}
}
