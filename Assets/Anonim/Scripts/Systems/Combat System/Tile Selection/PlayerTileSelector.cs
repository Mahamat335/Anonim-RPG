using Anonim.Systems.DungeonSystem;
using Anonim.Systems.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Anonim.Systems.CombatSystem.Weapons;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    public class PlayerTileSelector : MonoBehaviour
    {
        [SerializeField] private TileSelector _tileSelector;
        public WeaponData _weaponData; // TODO: Use attackData.AttackType instead of AttackType
        private Vector2Int _playerTilePosition; // TODO: Remove this and use the player position from another script

        private void OnEnable()
        {
            EventManager.Instance.PlayerMouseLeftClickInput.AddListener(OnMouseLeftClick);
            EventManager.Instance.PlayerMousePositionInput.AddListener(OnMouseMove);
        }

        private void OnDisable()
        {
            EventManager.Instance.PlayerMouseLeftClickInput.RemoveListener(OnMouseLeftClick);
            EventManager.Instance.PlayerMousePositionInput.RemoveListener(OnMouseMove);
        }

        #region Input Actions
        private void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Attack();
            }
        }

        private void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 mousePosition = context.ReadValue<Vector2>();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                // TODO: cache last grid position and player position and only update if they change. EG: CheckForChanges()

                Vector2Int gridPosition = DungeonGenerator.Instance.GetWorldToGridPosition(new Vector2(worldPosition.x, worldPosition.y));
                _playerTilePosition = DungeonGenerator.Instance.GetWorldToGridPosition(new Vector2(transform.position.x, transform.position.y));
                Vector3Int selectionCenter = UpdateSelectionCenter(gridPosition, _weaponData.AttackRange);

                _tileSelector.UpdateSelectedTiles(selectionCenter, _weaponData.TileSelectionMethod, _weaponData.AttackRadius);
            }
        }
        #endregion

        protected virtual void Attack()
        {
            // Attack logic here
        }

        public Vector3Int UpdateSelectionCenter(Vector2Int centerPosition, uint selectionRange)
        {
            Vector2Int delta = centerPosition - _playerTilePosition;

            if (delta.magnitude <= selectionRange)
            {
                return new Vector3Int(centerPosition.x, centerPosition.y);
            }

            Vector2 direction = delta;
            direction.Normalize();
            direction *= selectionRange;

            Vector3 targetPosition = new Vector3(_playerTilePosition.x + direction.x, _playerTilePosition.y + direction.y);
            Vector3Int newCenter = new Vector3Int(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.y));

            return newCenter;
        }
    }
}