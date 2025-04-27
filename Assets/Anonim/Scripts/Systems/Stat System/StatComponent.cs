using UnityEngine;
using System.Collections.Generic;

namespace Anonim.Systems.StatSystem
{
    public class StatComponent : MonoBehaviour
    {
        [SerializeField] private List<Stat> _stats;

        private Dictionary<StatType, Stat> _statDict;

        private void Awake()
        {
            _statDict = new();
            foreach (var stat in _stats)
            {
                _statDict[stat.Type] = stat;
            }

            LinkDerivedStats();
        }

        public float GetStat(StatType type)
        {
            return _statDict.TryGetValue(type, out var stat) ? stat.Value : 0f;
        }

        public void AddModifier(StatType type, StatModifier modifier, bool isNotifying = true)
        {
            if (_statDict.TryGetValue(type, out var stat))
            {
                stat.AddModifier(modifier, isNotifying);
            }
        }

        public void RemoveModifier(StatType type, StatModifier modifier)
        {
            if (_statDict.TryGetValue(type, out var stat))
            {
                stat.RemoveModifier(modifier);
            }
        }

        public void LevelUp()
        {
            _statDict[StatType.Strength].BaseValue += 1;
            _statDict[StatType.Agility].BaseValue += 1;
            _statDict[StatType.Intelligence].BaseValue += 1;
            _statDict[StatType.Perception].BaseValue += 1;
        }

        private void LinkDerivedStats()
        {
            //TODO : Seperatly written functions for each derived stat instead of lambdas
            // Strength → MaxHealth
            AddModifier(StatType.MaxHealth, new StatModifier(() => _statDict[StatType.Strength].Value * 10f, this), false);

            // Agility → MovementSpeed
            AddModifier(StatType.MovementSpeed, new StatModifier(() => _statDict[StatType.Agility].Value * 0.1f, this), false);

            // Agility → AttackSpeed
            AddModifier(StatType.AttackSpeed, new StatModifier(() => _statDict[StatType.Agility].Value * 0.05f, this), false);
        }

    }
}
