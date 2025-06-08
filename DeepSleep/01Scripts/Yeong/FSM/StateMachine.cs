using System;
using System.Collections.Generic;
using YH.Entities;
using UnityEngine;

namespace YH.FSM
{
    public class StateMachine
    {
        public EntityState currentState { get; private set; }

        private Dictionary<FSMState, EntityState> _states;
        public StateMachine(EntityStateListSO fsmStates, Entity entity)
        {
            _states = new Dictionary<FSMState, EntityState>();

            foreach (EntityStateSO state in fsmStates.states)
            {
                try
                {
                    Type t = Type.GetType(state.className);
                    var entityState = Activator.CreateInstance(t, entity, state.animParam) as EntityState;
                    _states.Add(state.stateName, entityState);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"{state.className} loading Error, Message : {ex.Message}");
                }
            }
        }

        public void Initialize(FSMState startState)
        {
            currentState = GetState(startState);
            Debug.Assert(currentState != null, $"{startState} state not found");
            currentState.Enter();
        }

        public void ChangeState(FSMState newState)
        {
            currentState.Exit();
            currentState = GetState(newState);
            Debug.Assert(currentState != null, $"{newState} state not found");
            currentState.Enter();
        }

        public void UpdateStateMachine()
        {
            currentState.Update();
        }

        public EntityState GetState(FSMState state) => _states.GetValueOrDefault(state);
    }
}
