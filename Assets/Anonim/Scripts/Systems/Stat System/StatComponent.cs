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
        }

        public float GetStat(StatType type)
        {
            return _statDict.TryGetValue(type, out var stat) ? stat.Value : 0f;
        }

        public void AddModifier(StatType type, StatModifier modifier)
        {
            if (_statDict.TryGetValue(type, out var stat))
            {
                stat.AddModifier(modifier);
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
        }
    }
}
