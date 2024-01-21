using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    public StageSelect[] StageSelects;
    private List<StageInfo> stageInfos = new List<StageInfo>();

    public int CurStageIndex { private set; get; }

    private void Awake()
    {
        if(Instance == null)            
            Instance = this;
        else
            Destroy(gameObject);

        LoadStageInfo();
        UpdateStageState();


        if(SceneManager.GetActiveScene().name == "StageSelectScene" || SceneManager.GetActiveScene().name == "StartScene")
        {
            GameObject.Find("Player").transform.position = stageInfos[CurStageIndex - 1].StageSpawnPos;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        HideStageUI();
    }

    void Update()
    {
        
    }
    
    public void LoadStageInfo()
    {
        stageInfos.Clear();

        for (int i = 1; i <= 10; i++)
        {
            StageInfo stageInfo = Resources.Load($"Stages/Stage_{i}") as StageInfo;
            stageInfo.LoadState();
            stageInfos.Add(stageInfo);   
        }
    }

    private bool IsnullReference()
    {
        if (stageInfos.Count <= 0)
            return true;

        foreach(var item in stageInfos)
            if(item == null) 
                return true;

        return false;
    }

    private void UpdateStageState()
    {
        if (IsnullReference())
            LoadStageInfo();

        //if (IsnullReference())
        //    return;

        for(int i = 0; i < stageInfos.Count; i++)
        {
            if(stageInfos[i].StageState == StageState.Current)
            {
                CurStageIndex = i + 1;
                break;
            }
            else if (stageInfos[i].StageState == StageState.Locked)
            {
                CurStageIndex = i + 1;
                stageInfos[i].SetState(StageState.Current);
                break;
            }
        }
        

        //if (CurStageIndex > 1)
        //    GameObject.Find("Player").transform.position = StageSelects[CurStageIndex - 2].stageInfo.StageSpawnPos;

        //for (int i = 0; i < StageSelects.Length; i++)
        //{
        //    if (i == CurStageIndex - 1)
        //    {
        //        StageSelects[i].SetCurrentStage();
        //    }
        //    else if (i < CurStageIndex - 1)
        //    {
        //        StageSelects[i].SetStageClear();
        //    }
        //    else
        //    {
        //        StageSelects[i].SetStageLock();
        //    }
        //}
    }

    public void SetStage(int stageIndex, StageInfo stageInfo)
    {
        //if (stageIndex != CurStageIndex)
        //    return;

        GameManager.Instance.MoveToGameStage(stageIndex);
        //GameManager.Instance.MoveToStage(stageInfo.SceneName);
    }

    public void OnStageClear(int stageIndex)
    {
        if (IsnullReference())
            LoadStageInfo();

        for (int i = 0; i < stageIndex; i++)
        {
            stageInfos[i].SetState(StageState.Clear);
        }

        //JsonManager.Instance.SavePlayerDataToJson();
        //CurStageIndex = stageIndex;
    }

    [ContextMenu("Current Stage Clear")]
    public void CurStageClear()
    {
        OnStageClear(CurStageIndex);
    }

    public void ShowStageUI()
    {
        StageSelects = GameObject.Find("Stages").GetComponentsInChildren<StageSelect>();

        if (IsnullReference())
            LoadStageInfo();

        foreach (var stage in StageSelects)
        {
            stage.ShowUI();
        }
    }

    public void HideStageUI()
    {
        StageSelects = GameObject.Find("Stages")?.GetComponentsInChildren<StageSelect>();

        if (IsnullReference())
            LoadStageInfo();

        if (StageSelects != null)
		{
            foreach (var stage in StageSelects)
            {
                stage.HideUI();
            }
		}
    }

    [ContextMenu("ResterStage")]
    public void ResetStage()
    {
        Debug.Log("Stage Reset!");
        PlayerPrefs.DeleteKey("Tutorial");
        foreach(var stage in stageInfos)
        {
            stage.SetState(StageState.Locked);
        }

        UpdateStageState();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
