using System.Collections;
using System.Collections.Generic;
using IH.UI;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseNode : MonoBehaviour
{
    [HideInInspector] public Skill currentSkill;
    [SerializeField] protected Image _image;
    [SerializeField] private List<UILineRenderer> uiLineRenderers = new List<UILineRenderer>();
    
    [HideInInspector] public List<PartNodeUI> connectedAbleNodes = new List<PartNodeUI>();
    private Dictionary<int, UILineRenderer> _nodeConnectLine = new Dictionary<int, UILineRenderer>();
    public NodeActiveFrame activeFrame;

    protected virtual void Awake()
    {
        
    }

    public void LineConnect()
    {
        StartCoroutine(WaitLineConnect());
        IndexNodeInit();
        DisableAllLines();
    }

    protected virtual IEnumerator WaitLineConnect()
    {
        for (int i = 0; i < connectedAbleNodes.Count; i++)
        {
            yield return new WaitForFixedUpdate();
            uiLineRenderers[i].gameObject.SetActive(true);

            RectTransform rectTrm = _image.transform as RectTransform;
            Vector3 startPos = new Vector3(0, 0);
            Vector3 relativePos = transform.InverseTransformPoint(connectedAbleNodes[i].transform.position);
            Vector3 delta = relativePos - new Vector3(0, rectTrm.sizeDelta.y);
            Vector3 endPos = startPos + delta;
                
            uiLineRenderers[i].points = new Vector2[3]
            {
                startPos,
                new Vector2(endPos.x - 100, relativePos.y),
                relativePos
            };
            yield return null;
        }
    }

    private void IndexNodeInit()
    {
        if(connectedAbleNodes.Count == _nodeConnectLine.Count)
            return;

        for (int i = 0; i < connectedAbleNodes.Count; i++)
            _nodeConnectLine.Add(i, uiLineRenderers[i]);
    }
    
    protected void NodeConnectCheck()
    {
        for (int i = 0; i < connectedAbleNodes.Count; i++)
        {
            var node = connectedAbleNodes[i];
            if (node.isEmpty)
                continue;
            
            transform.SetAsLastSibling();
            _nodeConnectLine[i].LineEnable();
            if (node.isSkillConnected)
                continue;

            node.activeFrame.ActiveFrameEnable();
            node.isSkillConnected = true;
            node.skillNode.ConnectNode(node);
            node.NodeConnectCheck();
        }
    }

    public void DisableAllLines()
    {
        activeFrame.ActiveFrameInit();
        foreach (var line in _nodeConnectLine.Values)
        {
            line.LineDisable();
        }
    }
}
