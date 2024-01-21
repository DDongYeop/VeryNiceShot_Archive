using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ShotModeUI : MonoBehaviour
{
    private UIDocument _uiDocument;

    private VisualElement _lowShotElement;
    private VisualElement _highShotElement;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        VisualElement root = _uiDocument.rootVisualElement;

        _lowShotElement = root.Q<VisualElement>("low-shot");
        _highShotElement = root.Q<VisualElement>("high-shot");
    }

    public void ShotModeChange(bool value)
    {
        if (value)
        {
            _lowShotElement.RemoveFromClassList("choose");
            _highShotElement.AddToClassList("choose");
        }
        else
        {
            _lowShotElement.AddToClassList("choose");
            _highShotElement.RemoveFromClassList("choose");
        }
    }
}
