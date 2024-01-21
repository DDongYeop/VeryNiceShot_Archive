using UnityEngine;

public class LevelManager : MonoBehaviour
{
	private Movement player;
	public Movement Player => player;
	private Hole hole;

	private WhiteBlock[] whiteBlocks;

	private ObstacleCntUI _obstacleCntUI;

    public void SetUpGame()
	{
		_obstacleCntUI = FindObjectOfType<ObstacleCntUI>();
		player = FindObjectOfType<Movement>();
		hole = FindObjectOfType<Hole>();

		if (player != null)
		{
			GameManager gameManager = GameManager.Instance;
			if (gameManager.currentGameState != GameState.Menu && gameManager.CurrentStage > 0 && gameManager.CurrentStage <= gameManager.StageData.datas.Count)
				player.MoveCount = gameManager.StageData.datas[gameManager.CurrentStage - 1].moveCount;
			else
				player.MoveCount = -1;
			GameManager.Instance.UIController.ActiveSelect(player.canHighShot);
		}

		whiteBlocks = FindObjectsByType<WhiteBlock>(FindObjectsSortMode.None);
		foreach (WhiteBlock wb in whiteBlocks)
		{
			wb.OnTouched += IsAllWhiteBlockTouched;
		}
		_obstacleCntUI?.Restart();
		IsAllWhiteBlockTouched();
	}

	public void IsAllWhiteBlockTouched()
	{
		_obstacleCntUI?.Collision();
		foreach (WhiteBlock wb in whiteBlocks)
		{
			if (wb.IsTouched == false)
			{
				return;
			}
		}
		hole?.Active(true);
	}

	public void ClearGame()
	{
		foreach (WhiteBlock wb in whiteBlocks)
		{
			wb.OnTouched -= IsAllWhiteBlockTouched;
		}
	}

	public void ResetGame()
	{
		player.ResetPlayer();
		hole.Active(false);
		foreach (WhiteBlock wb in whiteBlocks)
		{
			wb.Active(true);
		}
		_obstacleCntUI.Restart();
		IsAllWhiteBlockTouched();
	}

	public void SetShotMode(bool isHighShot)
	{
		if (player.canHighShot)
		{
			player.isHighShot = isHighShot;
			GameManager.Instance.UIController.SetShotMode(!isHighShot);
		}
	}
}
