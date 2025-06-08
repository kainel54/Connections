using ObjectPooling;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveSO", menuName = "SO/Wave/WaveSO")]
public class WaveSO : ScriptableObject
{
    public WaveDifficult waveDifficult;
    [Header("Total Wave")]
    public List<WaveData> waveList;
}

public enum WaveDifficult
{
    Easy,Normal,Hard
}

public enum SpawnType
{
    Default, Circle, XShape, Cross,
}

[Serializable]
public struct WaveData
{
    [Header("Wave")]
    public List<EnemyPoolingType> enemyType;
    public SpawnType spawnType;
    public float spawnDelay;
    public bool isReverse;
}



