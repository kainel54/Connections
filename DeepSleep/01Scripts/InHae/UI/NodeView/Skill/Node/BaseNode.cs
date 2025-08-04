using System;
using System.Collections;
using System.Collections.Generic;
using IH.UI;
using ObjectPooling;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public abstract class BaseNode : MonoBehaviour, IPoolable
{
    [SerializeField] protected NodeUIPoolingType _poolingType;
    
    [HideInInspector] public Skill currentSkill;
    [FormerlySerializedAs("_image")] 
    public Image image;
    [SerializeField] private RectTransform _lineParent;
    
    protected UILineRenderer[] _uiLineRenderers;
    
    public List<BaseNode> connectedNodes = new ();
    public NodeActiveFrame activeFrame;
    public bool isNewEnableNode;
    
    public int index;

    protected virtual void Awake()
    {
        _uiLineRenderers = _lineParent.GetComponentsInChildren<UILineRenderer>(true);
    }

    public virtual void LineConnect()
    {
        StartCoroutine(WaitLineConnect());
    }

    protected virtual IEnumerator WaitLineConnect()
    {
        yield return null;
        
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            if (connectedNodes[i].transform.GetSiblingIndex() < transform.GetSiblingIndex())
                continue;

            _uiLineRenderers[i].gameObject.SetActive(true);

            Vector3 startPos = new Vector3(0, 0);
            Vector3 relativePos = transform.InverseTransformPoint(connectedNodes[i].transform.position);
                
            _uiLineRenderers[i].points = new Vector2[2]
            {
                startPos,
                relativePos
            };
            
            yield return null;
            _uiLineRenderers[i].InitMaterialIfNeeded();
            _uiLineRenderers[i].SetVerticesDirty();
            _uiLineRenderers[i].SetMaterialDirty();
        }
        DisableAllLines();
    }

    public virtual void NodeConnectCheckAndEnable()
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            var node = connectedNodes[i] as PartNodeUI;
            
            if ( connectedNodes[i] is SkillNodeUI || node.isPartEmpty)
                continue;

            if (node.isNewEnableNode)
            {
                node.isNewEnableNode = false;
                _uiLineRenderers[i].LineLerpEnable();
            }
            else
            {
                _uiLineRenderers[i].LineEnable();
            }
            
            if (node.isSkillConnected)
                continue;

            node.activeFrame.ActiveFrameEnable();
            node.isSkillConnected = true;
            node.skillNode.ConnectNode(node);
            node.NodeConnectCheckAndEnable();
        }
    }

    public void DisableAllLines()
    {
        activeFrame.ActiveFrameInit();
        foreach (var line in _uiLineRenderers)
            line.LineDisable();
    }

    public GameObject GameObject => gameObject;
    public Enum PoolEnum => _poolingType;
    public virtual void OnPop()
    {
    }

    public virtual void OnPush()
    {
        foreach (var uiLine in _uiLineRenderers)
            uiLine.Init();
        
        isNewEnableNode = false;
        connectedNodes.Clear();
    }
}
