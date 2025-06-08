using UnityEngine;

public class MiniMapTypeIcon : MonoBehaviour
{
    public LevelTypeEnum levelType;

    public void Init()
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
    }
}
