using System;
using System.Collections.Generic;
using UnityEngine;
using YH.Entities;

namespace YH.StatSystem
{
    public class EntityStat : MonoBehaviour, IEntityComponent
    {
        [SerializeField] private List<StatElement> _overrideStatElements = new List<StatElement>();
        [SerializeField] private StatBaseSO _baseStat;
        private Entity _entity;
        public void Initialize(Entity entity)
        {
            _entity = entity;
            Initialize();
        }
        private Dictionary<string, StatElement> _overrideStatDictionary;

        public void Initialize()
        {
            _overrideStatDictionary = new Dictionary<string, StatElement>();
            foreach (StatElement statElement in _overrideStatElements)
            {
                if (statElement.elementSO == null) continue;

                _overrideStatDictionary.Add(statElement.elementSO.statName, statElement);
            }
        }

        public StatElement GetElement(StatElementSO statType)
        {
            if (_overrideStatDictionary.TryGetValue(statType.statName, out StatElement statElement))
                return statElement;

            statElement = _baseStat.GetStatElement(statType.statName);
            if (statElement != null)
                return statElement;
            else
                return null;
        }
        public bool TryGetElement(StatElementSO statType, out StatElement statElement)
        {
            if (_overrideStatDictionary.TryGetValue(statType.statName, out statElement)) return true;

            statElement = _baseStat.GetStatElement(statType.statName);
            return statElement != null;
        }
        public StatElement GetElement(string statName)
        {
            if (_overrideStatDictionary.TryGetValue(statName, out StatElement statElement))
                return statElement;

            statElement = _baseStat.GetStatElement(statName);
            if (statElement != null)
                return statElement;
            else
                return null;
        }
        public bool TryGetElement(string statName, out StatElement statElement)
        {
            if (_overrideStatDictionary.TryGetValue(statName, out statElement)) return true;

            statElement = _baseStat.GetStatElement(statName);
            return statElement != null;
        }

        
    }
}