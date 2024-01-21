using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class InGameUI : MenuScreen
{

    #region Reference

    [Header("Reference")]
    [SerializeField] private AudioMixer m_audioMixer;
    [SerializeField] private GameObject m_UiCover;

    //Audio Mixer
    private readonly string Mixer_Master = "MasterVolume";
    private readonly string Mixer_Music = "MusicVolume";
    private readonly string Mixer_SFX = "SFXVolume";

    //VisualElement Names
    private readonly string m_PausePopupName = "Pause-Popup";
    private readonly string m_PausBtnPanelName = "PausBtn-Panel";
    private readonly string m_SettingPanelName = "Setting-Panel";

    //Button names
    private readonly string m_PausBtnName = "Pause-Button";
    private readonly string m_HomeBtnName = "Home-Button";
    private readonly string m_ResumeBtnName = "Resume-Button";
    private readonly string m_SettingBtnName = "Setting-Button";
    private readonly string m_SettingExitBtnName = "SettingExit-Button";

    //Slider Names
    private readonly string m_MusicSliderName = "Music-Slider";
    private readonly string m_SoundFxSliderName = "SoundFx-Slider";
    private readonly string m_MasterSliderName = "Master-Slider";

    //Buttons
    private Button m_PauseBtn;
    private Button m_HomeBtn;
    private Button m_ResumeBtn;
    private Button m_SettingBtn;
    private Button m_SettingExitBtn;

    //VisualElements
    private VisualElement m_PausePopup;
    private VisualElement m_PausBtnPanel;
    private VisualElement m_SettingPanel;

    //Slider
    private Slider m_MusicSlider;
    private Slider m_SoundFxSlider;
    private Slider m_MasterSlider;

    #endregion

    private bool IsPausePopupOn = false;
    private bool IsSettingPanelOn = false;

    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        m_PauseBtn = m_Root.Q<Button>(m_PausBtnName);
        m_PausePopup = m_Root.Q<VisualElement>(m_PausePopupName);

        m_HomeBtn = m_Root.Q<Button>(m_HomeBtnName);
        m_ResumeBtn = m_Root.Q<Button>(m_ResumeBtnName);
        m_SettingBtn = m_Root.Q<Button>(m_SettingBtnName);


        m_PausBtnPanel = m_Root.Q<VisualElement>(m_PausBtnName);

        m_SettingPanel = m_Root.Q<VisualElement>(m_SettingPanelName);
        m_SettingExitBtn = m_Root.Q<Button>(m_SettingExitBtnName);


        m_MasterSlider = m_Root.Q<Slider>(m_MasterSliderName);
        m_MusicSlider = m_Root.Q<Slider>(m_MusicSliderName);
        m_SoundFxSlider = m_Root.Q<Slider>(m_SoundFxSliderName);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        m_SettingBtn.RegisterCallback<ClickEvent>(evt => { ShowSettingPanel(); });
        m_SettingExitBtn.RegisterCallback<ClickEvent>(evt => { HideSettingPanel(); });

        m_PauseBtn.RegisterCallback<ClickEvent>(ShowPauseScreen);
        m_ResumeBtn.RegisterCallback<ClickEvent>(evt => { HidePauseScreen(); });

        m_HomeBtn.RegisterCallback<ClickEvent>(evt =>
        {
            HidePauseScreen();
            GameManager.Instance.MoveToHome();
        });


        m_MasterSlider.RegisterValueChangedCallback(evt =>
        {
            float volume = evt.newValue;
            m_audioMixer.SetFloat(Mixer_Master, MathF.Log10(volume) * 20);
            PlayerPrefs.SetFloat(Mixer_Master, volume);
        });

        m_MusicSlider.RegisterValueChangedCallback(evt =>
        {
            float volume = evt.newValue;
            m_audioMixer.SetFloat(Mixer_Music, MathF.Log10(volume) * 20);
            PlayerPrefs.SetFloat(Mixer_Music, volume);
        });

        m_SoundFxSlider.RegisterValueChangedCallback(evt =>
        {
            float volume = evt.newValue;
            m_audioMixer.SetFloat(Mixer_SFX, MathF.Log10(volume) * 20);
            PlayerPrefs.SetFloat(Mixer_SFX, volume);
        });
    }

    private void ShowSettingPanel()
    {
        IsSettingPanelOn = true;
        m_SettingPanel.style.display = DisplayStyle.Flex;

        m_MasterSlider.value = PlayerPrefs.GetFloat(Mixer_Master);
        m_MusicSlider.value = PlayerPrefs.GetFloat(Mixer_Music);
        m_SoundFxSlider.value = PlayerPrefs.GetFloat(Mixer_SFX);

        m_SettingPanel.RemoveFromClassList("off");
    }
    
    private void HideSettingPanel()
    {
        IsSettingPanelOn = false;

        m_SettingPanel.AddToClassList("off");

        m_SettingPanel.RegisterCallback<TransitionEndEvent>(evt =>
        {
            if(!IsSettingPanelOn)
                m_SettingPanel.style.display = DisplayStyle.None;
        });
    }

    public void HidePauseScreen()
    {
        IsPausePopupOn = false;
        m_UiCover.SetActive(false);

        m_PausePopup.AddToClassList("off");

        m_PausePopup.RegisterCallback<TransitionEndEvent>(evt =>
        {
            if (!IsPausePopupOn)
                m_PausePopup.style.display = DisplayStyle.None;    
        });

    }

    private void ShowPauseScreen(ClickEvent evt)
    {
        IsPausePopupOn = true;
        m_UiCover.SetActive(true);

        m_PausePopup.style.display = DisplayStyle.Flex;
        m_PausePopup.RemoveFromClassList("off");

    }

    public void HidePauseButton()
    {
        m_PausBtnPanel.style.display = DisplayStyle.None;
    }

    public void ShowPauseButton()
    {
        m_PausBtnPanel.style.display = DisplayStyle.Flex;
    }
}
