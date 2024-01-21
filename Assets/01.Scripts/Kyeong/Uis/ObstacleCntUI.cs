using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ObstacleCntUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Label _cntLabel;

    WhiteBlock[] _whiteBlocks;
    private int _maxCnt;
    private int _currentCnt;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _whiteBlocks = FindObjectsOfType<WhiteBlock>();
    }

    private void OnEnable()
    {
        _maxCnt = _whiteBlocks.Length;
        _cntLabel = _uiDocument.rootVisualElement.Q<Label>("cnt-label");
    }

    public void Restart()
    {
        return;
        _currentCnt = 0;
        _cntLabel.RemoveFromClassList("unshow");
        _cntLabel.text = $"0 / {_maxCnt}";
        
        if (_currentCnt == _maxCnt)
            _cntLabel.text = "";
    }

    public void Collision()
    {
        return;
        _currentCnt = 0;
        foreach (var whiteBlock in _whiteBlocks)
        {
            if (whiteBlock.IsTouched)
                _currentCnt++;
        }
        _cntLabel.text = $"{_currentCnt} / {_maxCnt}";
        
        if (_currentCnt == _maxCnt)
            _cntLabel.AddToClassList("unshow");
    }
}
