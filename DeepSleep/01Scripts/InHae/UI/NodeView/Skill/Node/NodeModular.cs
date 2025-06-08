using System.Collections.Generic;
using UnityEngine;

namespace IH.UI
{
    public static class NodeModular
    {
        public static float NodeOffset = 150f;
        
        private static readonly Dictionary<NodeDir, Vector2Int> nodeDirToGrid = new Dictionary<NodeDir, Vector2Int>
        {
            { NodeDir.Left,        new Vector2Int(-2, 0) },
            { NodeDir.LeftTop,     new Vector2Int(-1, 1) },
            { NodeDir.LeftBottom,  new Vector2Int(-1, -1) },
            { NodeDir.Right,       new Vector2Int(2, 0) },
            { NodeDir.RightTop,    new Vector2Int(1, 1) },
            { NodeDir.RightBottom, new Vector2Int(1, -1) }, 
        };
        
        public static Vector2Int GetNodeDirGrid(NodeDir nodeDir)
        {
            return nodeDirToGrid.TryGetValue(nodeDir, out var dir) ? dir : Vector2Int.zero;
        }
    }
}