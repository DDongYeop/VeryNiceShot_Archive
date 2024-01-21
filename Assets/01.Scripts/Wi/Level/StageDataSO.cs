using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StageData
{
    [TextArea]
    public string stageClearText;
    public int moveCount;
}

[CreateAssetMenu(menuName = "SO/Move Count")]
public class StageDataSO : ScriptableObject
{
    public List<StageData> datas;
}
