using System;
using System.Collections.Generic;
using UnityEngine;

namespace YH.FSM
{
    public enum FSMState
    {
        Idle, Move, Attack,
        Dash,Hit,Chase,FireIdle
    }
    
    [CreateAssetMenu(fileName = "EntityStateListSO", menuName = "SO/FSM/EntityStateList")]
    public class EntityStateListSO : ScriptableObject
    {
        public List<StateSO> states;
    }
}
