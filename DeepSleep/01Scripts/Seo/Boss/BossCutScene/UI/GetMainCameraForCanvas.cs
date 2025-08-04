using UnityEngine;

public class GetMainCameraForCanvas : MonoBehaviour
{
    private Canvas _myCanvas;
    void Start()
    {
        _myCanvas = GetComponent<Canvas>();
        _myCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        _myCanvas.worldCamera = Camera.main;
    }
}
