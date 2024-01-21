using UnityEngine;

public class Hole : MonoBehaviour
{
	[SerializeField] private string nextScene;
	[SerializeField] private MeshRenderer flagMeshRenderer;
	[SerializeField] private ParticleSystem particle;

	private MaterialPropertyBlock matPB;

	private readonly int colorHash = Shader.PropertyToID("_BaseColor");

	private bool isActive;

	private void Awake()
	{
		matPB = new MaterialPropertyBlock();
		Active(false);
	}

	public void Active(bool active)
	{
		isActive = active;
		Color flagColor = active ? Color.red : new Color(0.6f, 0.6f, 0.6f);
		matPB.SetColor(colorHash, flagColor);
		flagMeshRenderer.SetPropertyBlock(matPB);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (!isActive) return;
		if (other.CompareTag("Player"))
		{
			particle.gameObject.SetActive(true);
			Movement move = other.GetComponent<Movement>();
			move.StopMovement();
			move.enabled = false;
			GameManager.Instance.ClearGame();
			if (string.IsNullOrEmpty(nextScene))
				GameManager.Instance.MoveToSelect(2f);
			else
				GameManager.Instance.MoveToStage(nextScene);
		}
	}
}
