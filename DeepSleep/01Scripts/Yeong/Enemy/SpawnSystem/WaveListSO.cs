using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveListSO", menuName = "SO/Wave/WaveListSO")]
public class WaveListSO : ScriptableObject
{
    public List<WaveSO> waveList;
}
