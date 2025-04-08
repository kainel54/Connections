using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "StatusData", menuName = "SO/StatusData")]
public class StatusDataSO : ScriptableObject
{
    public StatusEnum status;
    
    public Sprite icon;
    public Sprite frame;

    // 스테이터스 아이콘에 마우스 오버랩 시 표시될 이름 및 설명
    public string statusName;
    public string description;
}
