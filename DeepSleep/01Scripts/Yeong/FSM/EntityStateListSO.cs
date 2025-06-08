using System;
using System.Collections.Generic;
using UnityEngine;

namespace YH.FSM
{
    public enum FSMState
    {
        Idle, Move, Attack,
        Dash,Dead
    }

    [CreateAssetMenu(fileName = "EntityStateListSO", menuName = "SO/FSM/EntityStateList")]
    public class EntityStateListSO : ScriptableObject
    {
        public List<EntityStateSO> states;
    }
}
