using System;
using System.Collections;
using UnityEngine;

public class WhiteBlock : LevelObject
{
	[SerializeField] private float _dissolvedTime = 0.5f;
	private MeshRenderer meshRen;
	private Collider col;
	private readonly int _hashDissolved = Shader.PropertyToID("_Dissolved");

    public bool IsTouched { get; private set; }
	public event Action OnTouched;

	private void Awake()
	{
		meshRen = GetComponent<MeshRenderer>();
		col = GetComponent<Collider>();
		Active(true);
	}

	public override void Active(bool active)
	{
		IsTouched = !active;
		//meshRen.enabled = active;
		col.enabled = active;
		if (active)
		{
			StopAllCoroutines();
			meshRen.material.SetFloat(_hashDissolved, 0);
		}
		else
		{
			StartCoroutine(DissolvedCo());
		}
	}

	private IEnumerator DissolvedCo()
	{
		float currentTime = 0;

		while (currentTime < _dissolvedTime)
		{
			yield return null;
			currentTime += Time.deltaTime;
			float time = currentTime / _dissolvedTime;
			time = (float)(1 - Math.Pow(1 - time, 3));
			meshRen.material.SetFloat(_hashDissolved, Mathf.Lerp(0, 1, time));
		}
		meshRen.material.SetFloat(_hashDissolved, 1);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			Active(false);
			OnTouched?.Invoke();
		}
	}
}
