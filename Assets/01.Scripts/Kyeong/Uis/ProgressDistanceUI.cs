using UnityEngine;
using UnityEngine.UIElements;

public class ProgressDistanceUI : MonoBehaviour
{
    private UIDocument _uiDocument;
    private VisualElement _playerPosElement;

    [SerializeField] private Vector2 _elementPos;
    private Transform _playerTrm;   //플레이어 위치
    private Transform _endTrm;      //홀(도착지점) 위치
    private float _startDistance;   //처음 시작했을 떄의 거리

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        //플레이어, 도착지점 가져오기 
        _startDistance = Vector3.Distance(_playerTrm.position, _endTrm.position); //처음 거리 가져오기 
    }

    private void OnEnable()
    {
        _playerPosElement = _uiDocument.rootVisualElement.Q<VisualElement>("player-pos");
    }

    private void Update()
    {
        float currentDistance = Vector3.Distance(_playerTrm.position, _endTrm.position); //현제 얼마나 멀리 있는지 
        float percent = currentDistance / _startDistance; //Lerp에서 사용할 수 있도록 Percent로 바꾸고
        percent = Mathf.Clamp(percent, 0, 1); // 0~1로 제한
        _playerPosElement.style.top = new StyleLength(Mathf.Lerp(_elementPos.x, _elementPos.y, percent)); //적용
    }
}
