using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public enum StageState { Clear = 0 , Current = 1 , Locked = 2 }
[CreateAssetMenu(menuName = "SO/Stage", fileName = "New StageInfo")]
public class StageInfo : ScriptableObject
{
    [SerializeField] private Sprite _mapImage;
    public Sprite MapImage => _mapImage;

    [SerializeField] private string _SceneName;
    public string SceneName => _SceneName;

    [SerializeField] private string _mapName;
    public string MapName => _mapName;

    [TextArea][SerializeField] private string _mapScript;
    public string MapScript => _mapScript;

    public Vector3 StageSpawnPos;

    [SerializeField]
    private StageState _stageState = StageState.Locked;
    public StageState StageState => _stageState;

    public void SetState(StageState stageState)
    {
        _stageState = stageState;
        PlayerPrefs.SetInt(_SceneName, (int)_stageState);
    }

    public void LoadState()
    {
        if (!PlayerPrefs.HasKey(_SceneName))
        {
            PlayerPrefs.SetInt(_SceneName, (int)StageState.Locked);
        }

        _stageState = (StageState)PlayerPrefs.GetInt(_SceneName);
    }
}
