using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JsonManager : MonoBehaviour
{
    public static JsonManager Instance;

    private List<StageInfo> stageInfos = new List<StageInfo>();
    private string _savePath;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        if (Application.platform == RuntimePlatform.Android)
        {
            _savePath = Application.dataPath + "/SaveData/";
        }
        else
        {
            _savePath = Application.persistentDataPath + "/SaveData/";
        }

        if (!Directory.Exists(_savePath))
        {
            Directory.CreateDirectory(_savePath);
        }

        LoadStageInfo();
        LoadPlayerDataToJson();
        SavePlayerDataToJson();
    }

    public void LoadStageInfo()
    {
        stageInfos.Clear();

        for (int i = 1; i <= 10; i++)
            stageInfos.Add(Resources.Load($"Stages/Stage_{i}") as StageInfo);
    }

    [ContextMenu("To Json Data")]
    public void SavePlayerDataToJson()
    {
        string mapState = "";
        foreach(var item  in stageInfos)
        {
            mapState += ((int)item.StageState).ToString();
        }

        string finalJson = JsonUtility.ToJson(mapState);
        File.WriteAllText(_savePath + "StageData.json", finalJson);
    }

    [ContextMenu("Load Json Data")]
    public void LoadPlayerDataToJson()
    {
        if (File.Exists(_savePath + "StageData.json"))
        {
            string json = File.ReadAllText(_savePath + "StageData.json");

            string mapState = JsonUtility.FromJson<string>(json);

            for(int i = 0; i < stageInfos.Count; i++)
            {
                int state = (int)Char.GetNumericValue(mapState[i]);
                stageInfos[i].SetState((StageState)state);
            }
        }
    }
}
