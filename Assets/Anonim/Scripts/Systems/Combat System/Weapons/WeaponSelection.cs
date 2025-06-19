using Anonim.Systems.CombatSystem.TileSelection;
using Anonim.Systems.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Anonim.Systems.CombatSystem.Weapons
{
    public class WeaponSelection : MonoBehaviour
    {
        [SerializeField] private WeaponData[] _weaponDataArray;
        [SerializeField] private PlayerTileSelector _playerTileSelector;

        private void OnEnable()
        {
            EventManager.Instance.EquipmentInput.AddListener(OnEquipmentChange);
        }

        private void OnDisable()
        {
            EventManager.Instance.EquipmentInput.RemoveListener(OnEquipmentChange);
        }

        private void OnEquipmentChange(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                string key = context.control.name; // örn: "1", "2", "3"

                if (int.TryParse(key, out int number))
                {
                    int index = number - 1;
                    if (index >= 0 && index < _weaponDataArray.Length)
                    {
                        _playerTileSelector.SetWeaponData(_weaponDataArray[index]);
                    }
                }
            }
        }
    }
}