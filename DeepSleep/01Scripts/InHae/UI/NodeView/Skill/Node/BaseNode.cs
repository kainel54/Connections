using System.Collections;
using System.Collections.Generic;
using IH.UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class BaseNode : MonoBehaviour
{
    [HideInInspector] public Skill currentSkill;
    [FormerlySerializedAs("_image")] public Image image;
    [SerializeField] private RectTransform _lineParent;
    
    protected UILineRenderer[] _uiLineRenderers;
    
    public List<BaseNode> connectedNodes = new List<BaseNode>();
    public NodeActiveFrame activeFrame;

    protected virtual void Awake()
    {
        _uiLineRenderers = _lineParent.GetComponentsInChildren<UILineRenderer>(true);
    }

    public virtual void LineConnect()
    {
        StartCoroutine(WaitLineConnect());
        DisableAllLines();
    }

    protected virtual IEnumerator WaitLineConnect()
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
        }
    }

    public virtual void NodeConnectCheck()
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            var node = connectedNodes[i] as PartNodeUI;
            
            if (node.isPartEmpty)
                continue;
            
            _uiLineRenderers[i].LineEnable();
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
        foreach (var line in _uiLineRenderers)
        {
            line.LineDisable();
        }
    }
}
