using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ChainDataList
{
    public List<PartItemSO> parts = new List<PartItemSO>();
    public string nodeReflectionName;
    
    private int _hash = 0;
    
    public override int GetHashCode()
    {
        if (_hash != 0)
            return _hash;
        
        foreach (var part in parts)
            _hash += part.GetHashCode();
        
        return _hash;
    }

    public override bool Equals(object obj)
    {
        if (obj is not ChainDataList other)
            return false;
        List<PartItemSO> list = other.parts;
        bool equal = list.OrderBy(x=>x).SequenceEqual(parts.OrderBy(x=>x));
        return equal; 
    }
}

[CreateAssetMenu(fileName = "ChainAbleListSO", menuName = "SO/ChainAbleListSO")]
public class ChainAbleListSO : ScriptableObject
{
    public List<ChainDataList> List;
    public Dictionary<ChainDataList, Type> nodeDictionary = new Dictionary<ChainDataList, Type>();

    private bool _isInitialized;
    
    private void OnEnable()
    {
        foreach (var chainData in List)
        {
            Type type = Type.GetType(chainData.nodeReflectionName);
            nodeDictionary.TryAdd(chainData, type);
        }
    }

    private void OnDestroy()
    {
        nodeDictionary.Clear();
    }
}
