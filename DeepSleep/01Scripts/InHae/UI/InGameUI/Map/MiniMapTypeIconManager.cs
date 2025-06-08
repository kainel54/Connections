using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniMapTypeIconManager : MonoBehaviour
{
    private Dictionary<LevelTypeEnum, MiniMapTypeIcon> _icons
        = new Dictionary<LevelTypeEnum, MiniMapTypeIcon>();
    
    private void Awake()
    {
        GetComponentsInChildren<MiniMapTypeIcon>(true)
            .ToList().ForEach(x=>_icons.Add(x.levelType, x));  
    }
    
    public void Init(LevelTypeEnum levelType)
    {
        if(!_icons.ContainsKey(levelType))
            return;
        _icons[levelType].Init();
    }
}
