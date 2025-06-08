using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DefaultUpgradePreviewNode : BaseNode
{
    [SerializeField] private Image _edgeImage;

    private bool _beforeActive;
    
    public void Init(DefaultUpgradePartNode selectNode)
    {
        connectedNodes.Clear();
        connectedNodes.Add(selectNode);
    }
    
    public override void LineConnect()
    {
        StartCoroutine(WaitLineConnect());
        _uiLineRenderers[0].LineDisable();
    }

    public void NewNodeInit(bool isActive)
    {
        LineConnect();
        if(isActive == _beforeActive)
            return;
        
        _beforeActive = isActive;
        _edgeImage.gameObject.SetActive(isActive);
        image.gameObject.SetActive(isActive);
    }

    public void Disable()
    {
        if(!gameObject.activeInHierarchy)
            return;
        gameObject.SetActive(true);
    }

    protected override IEnumerator WaitLineConnect()
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            if (connectedNodes[i].transform.GetSiblingIndex() < transform.GetSiblingIndex())
                continue;

            yield return new WaitForSecondsRealtime(0.02f);
            _uiLineRenderers[i].gameObject.SetActive(true);

            Vector3 startPos = new Vector3(0, 0);
            Vector3 relativePos = transform.InverseTransformPoint(connectedNodes[i].transform.position);
                
            _uiLineRenderers[i].points = new Vector2[2]
            {
                startPos,
                relativePos
            };
            
            _uiLineRenderers[i].SetVerticesDirty();
            yield return null;
        }    }
}