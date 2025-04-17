using TMPro;
using UnityEngine;

namespace Anonim.Systems.StatSystem
{
    public class NpcStatInfo : MonoBehaviour
    {
        [SerializeField] private StatComponent _statComponent;
        [SerializeField] private TMP_Text _infoText;

        private void OnEnable()
        {
            _infoText.SetText(GetStatInfo());
        }
        private string GetStatInfo()
        {
            string info = "Stats:\n";
            info += $"Strength: {_statComponent.GetStat(StatType.Strength)}\n";
            info += $"Agility: {_statComponent.GetStat(StatType.Agility)}\n";
            info += $"Intelligence: {_statComponent.GetStat(StatType.Intelligence)}\n";
            info += $"Perception: {_statComponent.GetStat(StatType.Perception)}\n";
            info += $"Max Health: {_statComponent.GetStat(StatType.MaxHealth)}\n";
            info += $"Movement Speed: {_statComponent.GetStat(StatType.MovementSpeed)}\n";
            info += $"Attack Power: {_statComponent.GetStat(StatType.AttackSpeed)}\n";

            return info;
        }
    }
}