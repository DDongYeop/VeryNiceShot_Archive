using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingManager : MonoBehaviour
{
	[SerializeField] private RectTransform transitionImage;
	[SerializeField] private CutOffMaskImage image;

	private bool isLoading = false;

	private int sceneIndex;
	private string sceneName;

	public float Delay { get; set; } = 0f;

    public void LoadScene(int index)
	{
		if (isLoading) return;

		isLoading = true;
		transitionImage.gameObject.SetActive(true);
		sceneIndex = index;
		SceneManager.sceneLoaded += OnSceneLoaded;
		StartCoroutine(LoadSceneCo());
	}
    public void LoadScene(string name)
	{
		if (isLoading) return;

		isLoading = true;
		transitionImage.gameObject.SetActive(true);
		sceneName = name;
		SceneManager.sceneLoaded += OnSceneLoaded;
		StartCoroutine(LoadSceneCo(sceneName));
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		image.SetAllDirty();
		isLoading = false;
		Delay = 0f;
	}

	private IEnumerator LoadSceneCo()
	{
		yield return new WaitForSeconds(1f);

		yield return StartCoroutine(FadeCircleCo(true));

		SceneManager.LoadScene(sceneIndex);
		GameManager.Instance.UIController.SetScreenText(active:false);

		yield return new WaitForSeconds(0.7f);

		StartCoroutine(FadeCircleCo(false));
	}

	private IEnumerator LoadSceneCo(string name)
	{
		yield return new WaitForSeconds(Delay);

		yield return StartCoroutine(FadeCircleCo(true));

		SceneManager.LoadScene(name);
		GameManager.Instance.UIController.SetScreenText(active:false);

		yield return new WaitForSeconds(0.7f);

		StartCoroutine(FadeCircleCo(false));
	}

	private IEnumerator FadeCircleCo(bool fadeIn)
	{
		float timer = 0f;
		while (timer <= 1f)
		{
			yield return null;
			timer += Time.unscaledDeltaTime;
			transitionImage.sizeDelta = fadeIn ? Vector2.Lerp(new Vector2(3000f, 3000f), Vector2.zero, timer) : Vector2.Lerp(Vector2.zero, new Vector2(3000f, 3000f), timer);
		}

		if (!fadeIn)
		{
			transitionImage.gameObject.SetActive(false);
		}
	}
}
