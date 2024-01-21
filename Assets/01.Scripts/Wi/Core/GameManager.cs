using UnityEngine.SceneManagement;
using UnityEngine;

public enum GameState
{
	Menu,
	Game,
	Clear,
}

public class GameManager : MonoBehaviour
{
	[SerializeField] private StageDataSO stageData;
	public StageDataSO StageData => stageData;
	[SerializeField] private PoolingListSO _poolingListSo;
	
    public static GameManager Instance;
	private LevelManager levelManager;
	public LevelManager LevelManager => levelManager;
	private SceneLoadingManager sceneLoader;
	private UIController uiController;
	public UIController UIController => uiController;

	[HideInInspector] public GameState currentGameState;
	private int currentStage = 0;
	public int CurrentStage => currentStage;

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			levelManager = GetComponent<LevelManager>();
			sceneLoader = GetComponent<SceneLoadingManager>();
			uiController = GetComponent<UIController>();
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}

		CreatePool();
		SceneManager.sceneLoaded += OnSceneLoaded;

		if (PlayerPrefs.GetInt("Tutorial", 0) == 0)
		{
			PlayerPrefs.SetInt("Tutorial", 1);
			SceneManager.LoadScene("Tutorial_1");
			currentGameState = GameState.Game;
		}
		else
		{
			SceneManager.LoadScene(1);
			currentGameState = GameState.Menu;
		}
	}

	public void StartGame()
	{
		levelManager.SetUpGame();
		UIController.SetShotMode(true, true);
	}

	public void ClearGame()
	{
		currentGameState = GameState.Clear;
		//int stage = PlayerPrefs.GetInt("stage", 0);
		//if (currentStage >= stage)
		//{
		//}
		Debug.Log($"Stage Clear! {currentStage}");
		StageManager.Instance.OnStageClear(currentStage);

		levelManager.ClearGame();
		if (levelManager.Player.CurrentMoveCount == 1)
		{
			uiController.SetScreenText(text: "Hole In One");
		}
		else if (currentStage != 0 && stageData.datas.Count >= currentStage)
		{
			uiController.SetScreenText(text: stageData.datas[currentStage - 1].stageClearText);
		}
		else
		{
			uiController.SetScreenText(text: "Very Nice\nShot");
		}
	}

	public void MoveToNextStage()
	{
		currentStage++;
		int sceneIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/00.Scenes/Wi/Stage_{currentStage}.unity");
		if (sceneIndex > 0)
		{
			currentGameState = GameState.Game;
			sceneLoader.LoadScene(sceneIndex);
		}
		else
		{
			Debug.Log("NoMore Scene");
			currentStage--;
		}
	}

	public void MoveToGameStage(int stageNum)
	{
		//int sceneIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/00.Scenes/Wi/Stage_{stageNum}.unity");
		int sceneIndex = SceneUtility.GetBuildIndexByScenePath($"Assets/00.Scenes/Yoon/Com/ComStage{stageNum}.unity");

		if (sceneIndex > 0)
		{
			currentGameState = GameState.Game;
			currentStage = stageNum;
			sceneLoader.LoadScene(sceneIndex);
		}
		else
		{
			Debug.Log("No Scene");
		}
	}

	public void MoveToStage(string stageName, float delay = 0)
	{
		currentGameState = GameState.Game;
		sceneLoader.Delay = delay;
		sceneLoader.LoadScene(stageName);
	}

	public void MoveToSelect(float delay = 0)
	{
        currentGameState = GameState.Menu;
        sceneLoader.Delay = delay;
        sceneLoader.LoadScene("StageSelectScene");
    }

	public void MoveToHome(float delay = 0)
	{
        currentGameState = GameState.Menu;
        sceneLoader.Delay = delay;
        sceneLoader.LoadScene("StartScene");
    }

	public void MoveToTutorial()
	{
		currentGameState = GameState.Menu;
		sceneLoader.LoadScene("Tutorial_1");
	}

	private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		StartGame();
		UIController.SetShotMode(true, true);
	}

	private void CreatePool()
	{
		PoolManager.Instance = new PoolManager(transform);
		_poolingListSo.PoolList.ForEach(p =>
		{
			PoolManager.Instance.CreatePool(p.Prefab, p.Count);
		});
	}
}
