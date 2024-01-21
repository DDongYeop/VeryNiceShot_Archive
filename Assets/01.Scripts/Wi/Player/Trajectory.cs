using UnityEngine;

public class Trajectory : MonoBehaviour
{
	[SerializeField] private float timeInterval;
	[SerializeField] private Transform[] dots;
	[SerializeField] private Color dotColor;
	private readonly int colorHash = Shader.PropertyToID("_BaseColor");

	private float camDist;
	private Transform camTrm;
	private MaterialPropertyBlock matPB;

	private void Awake()
	{
		camTrm = Camera.main.transform;
		camDist = Vector3.Distance(transform.position, camTrm.position) * 0.5f;
		matPB = new MaterialPropertyBlock();

		//for (int i = 0; i < dots.Length; ++i)
		//{
		//	Color color = dotColor;
		//	color.a = (dots.Length - i) / (float)dots.Length;
		//	matPB.SetColor(colorHash, color);
		//	dots[i].GetComponent<MeshRenderer>().SetPropertyBlock(matPB);
		//}
	}

	public void ShowTrajectory(bool show)
	{
		foreach (Transform trm in dots)
		{
			trm.gameObject.SetActive(show);
		}
	}

    public void DrawTrajectory(Vector3 power, bool applyYGravity = true)
	{
		Vector3 origin = transform.position;
		Vector3 gravity = Physics.gravity;
		if (applyYGravity)
		{
			gravity.y = 0;

		}
		float time = timeInterval;
		for (int i = 0; i < dots.Length; ++i)
		{
			float x = (power.x * time) + (gravity.x / 2 * time * time);
			float y = (power.y * time) + (gravity.y / 2 * time * time);
			float z = (power.z * time) + (gravity.z / 2 * time * time);
			Vector3 point = new Vector3(x, y, z);
			dots[i].position = origin + point;
			time += timeInterval;
			//float dist = Vector3.Distance(dots[i].position, camTrm.position);
			//dots[i].localScale = Vector3.one * (camDist / dist);
		}
	}
}
