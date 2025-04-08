using System;
using System.Collections.Generic;
using UnityEngine;

namespace YH.StatSystem
{
    [CreateAssetMenu(fileName = "BaseStat", menuName = "SO/Stat/BaseStat")]
    public class StatBaseSO : ScriptableObject
    {
        private Dictionary<string, StatElement> _statDictionary;
        [SerializeField] private List<StatElement> _statElements = new List<StatElement>();

        private void Awake()
        {
            SetUpDictionary();
        }

        private void OnValidate()
        {
            SetUpDictionary();
        }

        private void SetUpDictionary()
        {
            _statDictionary = new Dictionary<string, StatElement>();
            foreach (StatElement statElement in _statElements)
            {
                if (statElement.elementSO == null) continue;

                _statDictionary.Add(statElement.elementSO.statName, statElement);
            }
        }

        public StatElement GetStatElement(string statName)
        {
            if (_statDictionary.ContainsKey(statName))
                return _statDictionary[statName];
            else
                return null;
        }
    }
}
