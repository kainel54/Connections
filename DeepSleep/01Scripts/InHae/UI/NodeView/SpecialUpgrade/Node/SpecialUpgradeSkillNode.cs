using System.Collections;
using UnityEngine;

public class SpecialUpgradeSkillNode : BaseNode
{
    public void SkillNodeInit(SkillInventoryItem item)
    {
        image.sprite = item.data.icon;
        image.color = Color.white;
    }

    protected override IEnumerator WaitLineConnect()
    {
        yield return base.WaitLineConnect();
        NodeConnectCheckAndEnable();
    }
    
    public override void NodeConnectCheckAndEnable()
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            var node = connectedNodes[i] as SpecialUpgradePartNode;
            if (node.isEmpty)
                continue;
            
            _uiLineRenderers[i].LineEnable();
            node.activeFrame.ActiveFrameEnable();
            node.NodeConnectCheckAndEnable();
        }
    }
}
