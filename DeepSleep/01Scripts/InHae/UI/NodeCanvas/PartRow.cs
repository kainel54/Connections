using System;
using System.Collections.Generic;
using System.Linq;
using IH.EventSystem;
using UnityEngine;
using YH.EventSystem;

public class PartRow : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _nodeEventChannel;
    [SerializeField] private float _offset;
    [HideInInspector] public List<PartNodeUI> partNodes;
    public int activeNodeCount => partNodes.FindAll(x=> x.gameObject.activeInHierarchy).Count;
    
    private void Awake()
    {
        partNodes = GetComponentsInChildren<PartNodeUI>(true).ToList();
        
        _nodeEventChannel.AddListener<ChainModeChangeEvent>(HandleChainModeChangeEvent);
    }

    private void OnDestroy()
    {
        _nodeEventChannel.RemoveListener<ChainModeChangeEvent>(HandleChainModeChangeEvent);
    }

    private void HandleChainModeChangeEvent(ChainModeChangeEvent evt)
    {
        List<PartNodeUI> activeNodes = partNodes.Where(x => x.gameObject.activeInHierarchy).ToList();
        foreach (PartNodeUI nodeUI in activeNodes)
            nodeUI.isChainMode = evt.isActive;
    }

    public void SetPosition()
    {
        List<PartNodeUI> activeNodes = partNodes.Where(x => x.gameObject.activeInHierarchy).ToList();
        int count = activeNodes.Count;
        List<float> positions = new List<float>();

        if (count % 2 == 1)
        {
            positions.Add(0);
            for (int i = 1; i <= count / 2; i++)
            {
                float offsetValue = i * _offset * 2;
                positions.Insert(0, offsetValue);
                positions.Add(-offsetValue);
            }
        }
        else
        {
            for (int i = 0; i < count / 2; i++)
            {
                float offsetValue = (i * 2 + 1) * _offset;
                positions.Insert(0, offsetValue);
                positions.Add(-offsetValue);
            }
        }

        for (int i = 0; i < positions.Count; i++)
        {
            activeNodes[i].transform.localPosition = new Vector2(0, positions[i]);
        }
    }
}
