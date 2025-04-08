using UnityEngine;

public class MiniMapDoorIcon : MonoBehaviour
{
    [SerializeField] private DoorDir _doorDir;
    public DoorDir DoorDir => _doorDir;

    public void Init()
    {
        RectTransform rectTransform = transform as RectTransform;

        float angle = 0f;        
        switch (_doorDir)
        {
            case DoorDir.Top:
                angle = 90f;
                break;
            case DoorDir.Left:
                angle = 180f;
                break;
            case DoorDir.Bottom:
                angle = -90f;
                break;
        }
        
        rectTransform.localEulerAngles = new Vector3(0, 0, angle);
    }

    public void ActiveChange(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    
    public void SetDir(DoorDir dir) => _doorDir = dir;
}
