using System;
using System.Collections.Generic;
using UnityEngine;

namespace YH.StatSystem
{
    public enum EModifyMode
    {
        Add,
        Percnet
    }

    //StatElementSO�� �����ͼ� �ش� ������ [��ȭ]�� ǥ������
    [Serializable]
    public class StatElement : ISerializationCallbackReceiver
    {
        public StatElementSO elementSO;
        [SerializeField] private float _baseValue;
        private Dictionary<string, float> _addModifiers;
        private Dictionary<string, float> _percentModifiers;
        private Dictionary<EModifyMode, Dictionary<string, float>> _modifiers;

        private Dictionary<string, int> _addModifiersOverlapCount;
        private Dictionary<string, int> _percentModifiersOverlapCount;
        private Dictionary<EModifyMode, Dictionary<string, int>> _modifierOverlaps;

        public event Action<float, float> OnValueChanged;
        public event Action<int, int> OnIntValueChanged;

        public float Value { get; private set; }
        public int IntValue { get; private set; }

        public StatElement(float baseValue)
        {
            SetDictionary();
            SetValue();
        }

        public void OnBeforeSerialize() { }

        //Serialize���� ����� ���� ȣ��
        public void OnAfterDeserialize()
        {
            SetDictionary();
            SetValue();
        }

        private void SetDictionary()
        {
            _addModifiers ??= new Dictionary<string, float>();
            _percentModifiers ??= new Dictionary<string, float>();
            _modifiers ??= new Dictionary<EModifyMode, Dictionary<string, float>>()
            {
                { EModifyMode.Add, _addModifiers },
                { EModifyMode.Percnet, _percentModifiers }
            };
            _addModifiersOverlapCount ??= new Dictionary<string, int>();
            _percentModifiersOverlapCount ??= new Dictionary<string, int>();
            _modifierOverlaps ??= new Dictionary<EModifyMode, Dictionary<string, int>>()
            {
                { EModifyMode.Add, _addModifiersOverlapCount },
                { EModifyMode.Percnet, _percentModifiersOverlapCount }
            };
        }

        private void SetValue()
        {
            //���� ������� ����
            float totalAddModifier = 0;
            foreach (float addModifier in _addModifiers.Values)
            {
                totalAddModifier += addModifier;
            }

            //�ۼ�Ʈ ������� ����
            float totalPercentModifier = 0;
            foreach (float percentModifier in _percentModifiers.Values)
            {
                totalPercentModifier += percentModifier;
            }

            //���� �� ���
            float value = (_baseValue + totalAddModifier) * (1 + totalPercentModifier / 100);
            ////�ִ�, �ּ� ����
            //if (elementSO != null)
            //    value = Mathf.Clamp(value, elementSO.minMaxValue.x, elementSO.minMaxValue.y);
            
            int intValue = Mathf.CeilToInt(value);
            
            float previewValue = Value;
            int previewIntValue = IntValue;
            
            Value = value;
            IntValue = intValue;
            
            if (previewValue != Value)
                OnValueChanged?.Invoke(previewValue, Value);
            if (previewIntValue != IntValue) 
                OnIntValueChanged?.Invoke(previewIntValue, IntValue);
        }

        public void AddModify(string key, float modify, EModifyMode eModifyMode)
        {
            if (!_modifierOverlaps[eModifyMode].TryAdd(key, 1))
                _modifierOverlaps[eModifyMode][key]++;

            if (!_modifiers[eModifyMode].ContainsKey(key))
                _modifiers[eModifyMode][key] = modify;

            SetValue();
        }
        public void RemoveModify(string key, EModifyMode eModifyMode)
        {
            if (_modifierOverlaps[eModifyMode].ContainsKey(key))
            {
                _modifierOverlaps[eModifyMode][key]--;
                if (_modifierOverlaps[eModifyMode][key] < 0)
                    _modifierOverlaps[eModifyMode][key] = 0;
            }
            else
                Debug.LogWarning($"[{key}]Key not found for statModifierOverlap");

            if (_modifiers[eModifyMode].ContainsKey(key))
            {
                if (_modifierOverlaps[eModifyMode][key] == 0)
                    _modifiers[eModifyMode].Remove(key);
            }
            else
                Debug.LogWarning($"[{key}]Key not found for statModifier");
            SetValue();
        }
    }
}