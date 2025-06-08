using System.Collections;
using UnityEngine;

public class DefaultUpgradeSkillNode : BaseNode
{
    public void SkillNodeInit(SkillInventoryItem item)
    {
        image.sprite = item.data.icon;
        image.color = Color.white;
    }

    protected override IEnumerator WaitLineConnect()
    {
        yield return base.WaitLineConnect();
        NodeConnectCheck();
    }
    
    public override void NodeConnectCheck()
    {
        for (int i = 0; i < connectedNodes.Count; i++)
        {
            var node = connectedNodes[i] as DefaultUpgradePartNode;
            if (node.isEmpty)
                continue;
            
            _uiLineRenderers[i].LineEnable();
            node.activeFrame.ActiveFrameEnable();
            node.NodeConnectCheck();
        }
    }
}
