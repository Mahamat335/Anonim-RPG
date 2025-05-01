using System.Collections.Generic;
using Anonim.Systems.EventSystem;

namespace Anonim.Systems.StatSystem
{
    [System.Serializable]
    public class Stat
    {
        public StatType Type;
        public float BaseValue;
        private List<StatModifier> _modifiers = new();

        public float Value => CalculateValue();

        public void AddModifier(StatModifier modifier, bool isNotifying)
        {
            _modifiers.Add(modifier);

            if (isNotifying)
            {
                EventManager.Instance.StatModifierAdded.Dispatch(this);
            }
        }

        public void RemoveModifier(StatModifier modifier)
        {
            _modifiers.Remove(modifier);
        }

        private float CalculateValue()
        {
            float finalValue = BaseValue;
            foreach (var mod in _modifiers)
            {
                finalValue += mod.Value;
            }
            return finalValue;
        }
    }

    public class StatModifier
    {
        public float Value => _valueProvider != null ? _valueProvider() : _value;
        public object Source;

        private float _value;
        private System.Func<float> _valueProvider;

        public StatModifier(float value, object source)
        {
            _value = value;
            Source = source;
        }

        public StatModifier(System.Func<float> valueProvider, object source)
        {
            _valueProvider = valueProvider;
            Source = source;
        }
    }

}
