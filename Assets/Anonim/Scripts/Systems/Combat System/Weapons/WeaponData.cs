using UnityEngine;
using Anonim.CombatSystem.TileSelection.TileSelectionMethods;

namespace Anonim.Systems.CombatSystem.Weapons
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/Weapons/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        public string WeaponName;
        public string WeaponDescription;
        public uint AttackRange = 1;
        public uint AttackRadius = 1;
        public bool IsProjectile = false;
        public TileSelectionMethod TileSelectionMethod;
    }
}