using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class StartScreen : MenuScreen
{
    [Header("Reference")]
    [SerializeField] private GameObject m_StartCam;
    [SerializeField] protected SelectStage_Screen SelectStage_Screen;
    [SerializeField] private AudioMixer m_audioMixer;
    [SerializeField] private GameObject m_Cover;

    //Audio Mixer
    private readonly string Mixer_Master = "MasterVolume";
    private readonly string Mixer_Music = "MusicVolume";
    private readonly string Mixer_SFX = "SFXVolume";

    //VisualElement Names
    private readonly string m_SettingContainerName = "Setting-Container";
    private readonly string m_SettingPanelName = "Setting-Panel";

    //Button Names
    private readonly string m_PlayBtnName = "Play-Button";
    private readonly string m_TutorialBtnName = "Tutorial-Button";
    private readonly string m_SettingBtnName = "Setting-Button";
    private readonly string m_ExitBtnName = "Exit-Button";
    private readonly string m_HoleInOneBtnName = "HoleInOne-Button";
    private readonly string m_SettingExitBtnName = "SettingExit-Button";

    //Slider Names
    private readonly string m_MusicSliderName = "Music-Slider";
    private readonly string m_SoundFxSliderName = "SoundFx-Slider";
    private readonly string m_MasterSliderName = "Master-Slider";

    //VisualElement
    private VisualElement m_SettingContainer;
    private VisualElement m_SettingPanel;

    //Button
    private Button m_PlayBtn;
    private Button m_TutorialBtn;
    private Button m_SettingBtn;
    private Button m_ExitBtn;
    private Button m_HoleInOneBtn;
    private Button m_SettingExitBtn;

    //Slider
    private Slider m_MusicSlider;
    private Slider m_SoundFxSlider;
    private Slider m_MasterSlider;

    private bool m_IsSettingOn = false;

    private void Start()
    {
        SelectStage_Screen.HideUI();
        GameObject.Find("Manager").GetComponentInChildren<InGameUI>().HidePauseButton();
        m_Cover.SetActive(true);
        FindObjectOfType<InGameUI>().HidePauseScreen();
    }

    protected override void SetVisualElements()
    {
        base.SetVisualElements();

        m_PlayBtn = m_Root.Q<Button>(m_PlayBtnName);
        m_TutorialBtn = m_Root.Q<Button>(m_TutorialBtnName);
        m_SettingBtn = m_Root.Q<Button>(m_SettingBtnName);
        m_ExitBtn = m_Root.Q<Button>(m_ExitBtnName);
        m_HoleInOneBtn = m_Root.Q<Button>(m_HoleInOneBtnName);


        m_SettingContainer = m_Root.Q<VisualElement>(m_SettingContainerName);
        m_SettingPanel = m_Root.Q<VisualElement>(m_SettingPanelName);

        m_MasterSlider = m_Root.Q<Slider>(m_MasterSliderName);
        m_MusicSlider = m_Root.Q<Slider>(m_MusicSliderName);
        m_SoundFxSlider = m_Root.Q<Slider>(m_SoundFxSliderName);

        m_SettingExitBtn = m_Root.Q<Button>(m_SettingExitBtnName);
    }

    protected override void RegisterButtonCallbacks()
    {
        base.RegisterButtonCallbacks();

        m_PlayBtn.RegisterCallback<ClickEvent>(ColseStartScreen);

        m_ExitBtn.RegisterCallback<ClickEvent>(evt =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
   Application.Quit();
#endif
        });

        m_TutorialBtn.RegisterCallback<ClickEvent>(OnTutorial);

        if(PlayerPrefs.GetInt("Tutorial") == 0 || !PlayerPrefs.HasKey("Tutorial"))
        {
            m_PlayBtn.style.display = DisplayStyle.None;
        }

        m_HoleInOneBtn.RegisterCallback<ClickEvent>(evt =>
        {

        });

        m_SettingBtn.RegisterCallback<ClickEvent>(evt => { ShowSettingPanel(); });
        m_SettingExitBtn.RegisterCallback<ClickEvent>(evt => { HideSettingPanel(); });


        if (!PlayerPrefs.HasKey(Mixer_Master))
            PlayerPrefs.SetFloat(Mixer_Master, 1);
        if (!PlayerPrefs.HasKey(Mixer_Music))
            PlayerPrefs.SetFloat(Mixer_Music, 1);
        if (!PlayerPrefs.HasKey(Mixer_SFX))
            PlayerPrefs.SetFloat(Mixer_SFX, 1);

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

        m_MasterSlider.value = PlayerPrefs.GetFloat(Mixer_Master);
        m_MusicSlider.value = PlayerPrefs.GetFloat(Mixer_Music);
        m_SoundFxSlider.value = PlayerPrefs.GetFloat(Mixer_SFX);
    }

    private void OnTutorial(ClickEvent evt)
    {
        GameObject.Find("Manager").GetComponentInChildren<InGameUI>().ShowPauseButton();
        GameManager.Instance.MoveToTutorial();
        PlayerPrefs.SetInt("Tutorial", 1);
        PlayerPrefs.Save();
    }

    private void ShowSettingPanel()
    {
        m_IsSettingOn = true;

        m_SettingContainer.style.display = DisplayStyle.Flex;
        m_SettingContainer.RemoveFromClassList("off");

        m_SettingPanel.RemoveFromClassList("off");
    }

    private void HideSettingPanel()
    {
        m_IsSettingOn = false;

        m_SettingPanel.AddToClassList("off");
        m_SettingContainer.AddToClassList("off");

        m_SettingContainer.RegisterCallback<TransitionEndEvent>(evt =>
        {
            if(!m_IsSettingOn)
                m_SettingContainer.style.display = DisplayStyle.None;

        });
    }

    private void ColseStartScreen(ClickEvent evt)
    {
        m_Cover.SetActive(false);
        m_StartCam.SetActive(false);

        GameObject.Find("Manager").GetComponentInChildren<InGameUI>().ShowPauseButton();
        GetComponent<UIDocument>().enabled = false;
        SelectStage_Screen.ShowUI();
    }

    [ContextMenu("ShowStartScreen")]
    public void ShowStartScreen()
    {
        m_Cover.SetActive(true);

        GameObject.Find("Manager").GetComponentInChildren<InGameUI>().HidePauseButton();
        SelectStage_Screen.HideUI();

        //GetComponent<MeshRenderer>().enabled = true;
        GetComponent<UIDocument>().enabled = true;
        m_StartCam.SetActive(true);

        SetVisualElements();
        RegisterButtonCallbacks();
    }

    [ContextMenu("Reset Tutorial")]
    public void ResetTutorial()
    {
        PlayerPrefs.SetInt("Tutorial", 0);
        PlayerPrefs.Save();
    }
}
