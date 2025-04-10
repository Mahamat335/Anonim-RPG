using System.Collections.Generic;

namespace Anonim.Systems.StatSystem
{
    [System.Serializable]
    public class Stat
    {
        public StatType Type;
        public float BaseValue;
        private List<StatModifier> _modifiers = new();

        public float Value => CalculateValue();

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
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
        public float Value;
        public object Source;

        public StatModifier(float value, object source)
        {
            Value = value;
            Source = source;
        }
    }
}
