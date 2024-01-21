using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StageSelect : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject CurFlag;
    [SerializeField] private GameObject LockFlag;
    [SerializeField] private GameObject ClearFlag;
    [SerializeField] private ParticleSystem StageClearParticle;

    [Space]

    [SerializeField] private Sprite lockFlagIcon;
    [SerializeField] private Sprite unlockFlagIcon;
    [SerializeField] private Sprite curFlagIcon;

    [Space]

    [SerializeField] private Sprite CurStateIcon;
    [SerializeField] private Sprite LockStateIcon;
    [SerializeField] private Sprite ClearStateIcon;

    [Space]

    [SerializeField] private int StageIndex;
    [SerializeField] private StageInfo StageInfo;
    public StageInfo stageInfo => StageInfo;

    private void Start()
    {
        if (transform.childCount > 0)
            StageInfo.StageSpawnPos = transform.GetChild(0).position;

        HideUI();
        UpdateFlag();
    }

    public void SwapPos()
    {
        GameObject.Find("Player").transform.position = StageInfo.StageSpawnPos;
    }

    private void UpdateFlag()
    {
        if(transform.childCount >= 3)
            Destroy(transform.GetChild(2).gameObject);

        //Destroy(transform.GetChild(2));
        GameObject flagObj = LockFlag;
        Sprite icon = lockFlagIcon;

        switch (StageInfo.StageState)
        {
            case StageState.Current:
                flagObj = CurFlag;
                icon = curFlagIcon;
                break;
            case StageState.Locked:
                flagObj = LockFlag;
                icon = lockFlagIcon;
                break;
            case StageState.Clear:
                flagObj = ClearFlag; 
                icon = unlockFlagIcon;
                break;
        }

        GameObject newFlag = Instantiate(flagObj, Vector3.zero, Quaternion.identity);
        newFlag.transform.localScale = new Vector3(2.2f, 2.2f, 2.2f);
        newFlag.transform.SetParent(transform, false);

        GameObject newIcon = new GameObject("FlagIcon");
        newIcon.AddComponent<SpriteRenderer>().sprite = icon;
        newIcon.transform.SetParent(newFlag.transform);
        //GameObject newIcon = Instantiate("a", new Vector3(0.24f, 1.9f, -0.049f), Quaternion.Euler(new Vector3(11.25f, 0, 0)));
        newIcon.transform.localPosition = new Vector3(0.24f, 1.9f, -0.049f);
        newIcon.transform.localScale = new Vector3(0.09f, 0.18f, 0);
        newIcon.transform.rotation =  Quaternion.Euler(new Vector3(11.25f, 0, 0));
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enter");

        if (StageInfo.StageState == StageState.Locked)
            return;

        if (other.gameObject.name == "Player")
        {
            other.GetComponent<Movement>().StopMovement();
            other.GetComponentInChildren<TrailRenderer>().Clear();
            StageManager.Instance.SetStage(StageIndex, StageInfo);
        }
    }

    public void ShowUI()
    {
        Transform canvas = transform.Find("Canvas");
        canvas.gameObject.SetActive(true);


        Sprite newSprite = null;

        switch (StageInfo.StageState)
        {
            case StageState.Clear:
                newSprite = ClearStateIcon;
                break;
            case StageState.Locked:
                newSprite = LockStateIcon;
                break;
            case StageState.Current:
                newSprite = CurStateIcon;
                break;
        }

        canvas.Find("Button").GetComponent<Image>().sprite = newSprite;
    }

    public void HideUI()
    {
        transform.Find("Canvas").gameObject.SetActive(false);
    }
}
