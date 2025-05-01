using System.Collections.Generic;
using Anonim.Systems.DungeonSystem;
using Anonim.Systems.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Anonim.Systems.CombatSystem.TileSelection;

namespace Anonim.Systems.CombatSystem
{
    public class PlayerTileSelector : MonoBehaviour
    {
        [SerializeField] private TileSelector _tileSelector;
        //[SerializeField] private Attack _attack; TODO: I need do think about this
        public uint attackRadius = 1; // TODO: Remove this and use the attack radius from the attack scriptable object
        public uint attackRange = 3; // TODO: Remove this and use the attack range from the attack scriptable object
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

                Vector2Int gridPosition = DungeonGenerator.Instance.GetWorldToGridPosition(new Vector2(worldPosition.x, worldPosition.y));
                _playerTilePosition = DungeonGenerator.Instance.GetWorldToGridPosition(new Vector2(transform.position.x, transform.position.y));
                Vector3Int selectionCenter = UpdateSelectionCenter(gridPosition, attackRange);

                _tileSelector.UpdateSelectedTiles(selectionCenter, attackRadius);
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

            // Normalize yönü (x veya y düzleminde ilerle)
            Vector2 direction = delta;
            direction.Normalize();
            direction *= selectionRange;

            Vector3 targetPosition = new Vector3(_playerTilePosition.x + direction.x, _playerTilePosition.y + direction.y);
            Vector3Int newCenter = new Vector3Int(Mathf.RoundToInt(targetPosition.x), Mathf.RoundToInt(targetPosition.y));

            return newCenter;
        }
    }
}