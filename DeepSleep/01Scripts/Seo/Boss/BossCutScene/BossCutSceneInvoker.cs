using UnityEngine;

public class BossCutSceneInvoker : MonoBehaviour
{
    private BossRoomEnterCutScene _enterCutScene;
    [SerializeField] private CutSceneUISlider _cutSceneUi;
    void Start()
    {
        _enterCutScene = transform.parent.GetComponent<BossRoomEnterCutScene>();
    }

    public void EndCutScene()
    {
        _enterCutScene.CutSceneEnded();
    }


    public void ShowCutSceneUI()
    {
        _cutSceneUi.StartSlide();
    }
}
