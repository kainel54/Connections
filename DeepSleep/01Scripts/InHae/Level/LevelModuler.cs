using System.Collections.Generic;
using UnityEngine;

public static class LevelModuler
{
    public static DoorDir GetConverseDir(DoorDir currentDir)
    {
        DoorDir converseDir = DoorDir.Right;
            
        if (currentDir == DoorDir.Left)
            converseDir = DoorDir.Right;
        if (currentDir == DoorDir.Right)
            converseDir = DoorDir.Left;
        if (currentDir == DoorDir.Top)
            converseDir = DoorDir.Bottom;
        if (currentDir == DoorDir.Bottom)
            converseDir = DoorDir.Top;

        return converseDir;
    }
    
    public static Vector2Int GetNextGrid(DoorDir dir)
    {
        Vector2Int gridPos = new Vector2Int();

        if (dir == DoorDir.Left)
            gridPos.x = -1;
        if (dir == DoorDir.Right)
            gridPos.x = 1;
        if (dir == DoorDir.Top)
            gridPos.y = 1;
        if (dir == DoorDir.Bottom)
            gridPos.y = -1;

        return gridPos;
    }

    public static Dictionary<Vector2Int, int> BFS(Dictionary<Vector2Int, LevelRoom> levelGridDictionary)
    {
        Dictionary<Vector2Int, int> distanceDictionary = new Dictionary<Vector2Int, int> { { Vector2Int.zero, 0 } };
        
        Queue<Vector2Int> q = new Queue<Vector2Int>();
        q.Enqueue(Vector2Int.zero);
        
        while (q.Count != 0)
        {
            Vector2Int front = q.Peek();
            q.Dequeue();
            
            foreach (var connectGrid in levelGridDictionary[front].connectGrid)
            {
                if (!distanceDictionary.ContainsKey(connectGrid))
                {
                    distanceDictionary.Add(connectGrid, distanceDictionary[front] + 1);
                    q.Enqueue(connectGrid);
                }
            }
        }
        return distanceDictionary;
    }
}
