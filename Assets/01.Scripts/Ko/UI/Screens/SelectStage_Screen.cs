using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum CameraType { Iso = -1 , TopDown = 1 }
public class SelectStage_Screen : MenuScreen
{
    [Header("Reference")]
    [SerializeField] private GameObject MapCamObj;
    [SerializeField] private Sprite IsoIcon;
    [SerializeField] private Sprite TopDownIcon;

    //Button Name
    private readonly string m_MapBtnName;
    
    //Buttons
    private Button m_MapBtn;

    private CameraType m_CameraType = CameraType.Iso;
    private bool IsHide = false;

    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        m_MapBtn = m_Root.Q<Button>(m_MapBtnName);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        m_MapBtn.RegisterCallback<ClickEvent>(UpdateMap);
    }

    private void UpdateMap(ClickEvent evt)
    {
        if (IsHide)
            return;

        Debug.Log("Map Active");
        if(m_CameraType == CameraType.Iso)
        {
            m_MapBtn.style.backgroundImage = new StyleBackground(TopDownIcon);
            StageManager.Instance.ShowStageUI();
            MapCamObj.SetActive(true);
        }
        else
        {
            m_MapBtn.style.backgroundImage = new StyleBackground(IsoIcon);
            StageManager.Instance.HideStageUI();
            MapCamObj.SetActive(false);
        }

        m_CameraType = (CameraType)((int)m_CameraType * -1);
    }

    public void HideUI()
    {
        IsHide = true;
        m_MapBtn.style.display = DisplayStyle.None;
    }

    public void ShowUI()
    {
        IsHide = false;
        m_MapBtn.style.display = DisplayStyle.Flex;
    }
}
