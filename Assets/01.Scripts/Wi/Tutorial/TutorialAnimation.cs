using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TutorialAnimation : MonoBehaviour
{
	[SerializeField] private bool textAnimation = false;
	[SerializeField] private string[] texts;
	private int textIndex = 0;
	[SerializeField] private TextMeshProUGUI textmesh;
	[SerializeField] private float timerPerCharacter;
	public UnityEvent OnTextEnd;
	private float timer;
	private string writingText;
	private bool isWriting = false;

	private Coroutine writingCo;

	private void Start()
	{
		GameManager.Instance.LevelManager.Player.enabled = false;
		NextText();
	}

	public void AnimationTrigger()
	{
		GameManager.Instance.LevelManager.Player.enabled = true;
	}

	public void StopAnimation()
	{
		GameManager.Instance.LevelManager.Player.enabled = true;
		gameObject.SetActive(false);
	}

	public void NextText()
	{
		if (!textAnimation) return;
		if (isWriting)
		{
			StopCoroutine(writingCo);
			textmesh.text = texts[textIndex - 1];
			isWriting = false;
		}
		else
		{
			if (textIndex >= texts.Length)
			{
				OnTextEnd?.Invoke();
			}
			else
			{
				TextAnimation(texts[textIndex++]);
			}
		}
	}

	private void TextAnimation(string text)
	{
		isWriting = true;
		if (writingCo != null)
		{
			StopCoroutine(writingCo);
		}
		textmesh.text = string.Empty;
		writingText = string.Empty;
		writingCo = StartCoroutine(WritingCo(text));
	}

	private IEnumerator WritingCo(string text)
	{
		int t = 0;
		timer = timerPerCharacter;
		while (textmesh.text.Length < text.Length)
		{
			timer -= Time.deltaTime;
			if (timer <= 0f)
			{
				timer = timerPerCharacter;
				writingText = text.Substring(0, t);
				textmesh.text = writingText;
				t++;
			}
			yield return null;
		}
		isWriting = false;
	}
}
