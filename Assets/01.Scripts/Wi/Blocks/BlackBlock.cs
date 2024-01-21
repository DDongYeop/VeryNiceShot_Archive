using UnityEngine;

public class BlackBlock : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			GameManager.Instance.LevelManager.ResetGame();
		}
	}
}
