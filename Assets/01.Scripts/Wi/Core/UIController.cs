using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
	[Header("Select")]
    [SerializeField] private Transform selectImage;
	[SerializeField] private CanvasGroup selectUI;
    [SerializeField] private Transform lowImage;
    [SerializeField] private Transform highImage;

	[Header("Screen Text")]
	[SerializeField] private TextMeshProUGUI screenText;
	[SerializeField] private TextMeshProUGUI screenBackText;

	private Coroutine shotModeCo;

	public void SetScreenText(bool active = true, string text = "")
	{
		screenText.transform.parent.gameObject.SetActive(active);
		screenText.SetText(text);
		screenBackText.SetText(text);
	}

	public void ActiveSelect(bool active)
	{
		selectImage.parent.gameObject.SetActive(active);
	}

	public void SetShotMode(bool isLow, bool immediately = false)
	{
		if (shotModeCo != null)
		{
			StopCoroutine(shotModeCo);
		}
		if (!immediately)
		{
			shotModeCo = StartCoroutine(ShotModeCo(isLow));
			//shotModeCo = StartCoroutine(ShotModeCo2(isLow));
		}
		else
		{
			selectImage.localPosition = isLow ? new Vector2(-50, 0) : new Vector2(50, 0);
		}
	}

	private IEnumerator ShotModeCo(bool isLow)
	{
		Vector2 original = selectImage.localPosition;
		Vector2 left = new Vector2(-50, 0);
		Vector2 right = new Vector2(50, 0);
		float timer = 0;
		while (timer <= 1f)
		{
			timer += Time.unscaledDeltaTime * 1.5f;
			selectImage.localPosition = isLow ? Vector2.Lerp(original, left, timer) : Vector2.Lerp(original, right, timer);
			yield return null;
		}
	}

	private IEnumerator ShotModeCo2(bool isLow)
	{
		selectUI.alpha = 1f;
		Vector3 originRot = selectUI.transform.localEulerAngles;
		Vector3 targetRot = new Vector3(0, 0, isLow ? 0 : 180);
		Vector3 originScale = new Vector3(0.9f, 0.9f, 1f);
		Vector3 targetScale = new Vector3(1f, 1f, 1f);

		lowImage.localScale = originScale;
		highImage.localScale = originScale;
		Transform targetImage = isLow ? lowImage : highImage;
		float timer = 0;
		while (timer <= 1f)
		{
			timer += Time.unscaledDeltaTime * 1.5f;
			selectUI.transform.localEulerAngles = Vector3.Lerp(originRot, targetRot, timer);
			yield return null;
		}

		timer = 0;
		while (timer <= 1f)
		{
			timer += Time.unscaledDeltaTime * 4f;
			targetImage.localScale = Vector3.Lerp(originScale, targetScale, timer);
			yield return null;
		}

		yield return new WaitForSeconds(0.1f);

		timer = 0;
		while (timer <= 1f)
		{
			timer += Time.unscaledDeltaTime * 0.5f;
			selectUI.alpha = Mathf.Lerp(1f, 0f, timer);
			yield return null;
		}
	}
}
