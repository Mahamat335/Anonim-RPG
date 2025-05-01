using Anonim.CombatSystem.TileSelection.TileSelectionMethods;
using UnityEngine;

namespace Anonim.Systems.CombatSystem.Attacks
{
    [CreateAssetMenu(fileName = "AttackType", menuName = "Scriptable Objects/Attacks/Attack Type")]
    public class AttackType : ScriptableObject
    {
        public uint AttackRange = 1;
        public uint AttackRadius = 1;
        public TileSelectionMethod TileSelectionMethod;
    }
}