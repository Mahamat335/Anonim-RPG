using System.Collections.Generic;
using Anonim.Systems.DungeonSystem;
using Anonim.Systems.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    public class PlayerTileSelector : MonoBehaviour
    {
        [SerializeField] private TileSelector _tileSelector;
        private Vector3Int _currentTilePosition;

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

        private void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Attack logic here
            }
        }

        private void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 mousePosition = context.ReadValue<Vector2>();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                Vector2Int gridPosition = DungeonGenerator.Instance.GetWorldToGridPosition(new Vector2(worldPosition.x, worldPosition.y));

                UpdateSingleTileSelection(new Vector3Int(gridPosition.x, gridPosition.y, 0));
            }
        }

        public void UpdateSingleTileSelection(Vector3Int newTilePosition)
        {
            _tileSelector.RemoveSelectedTile(_currentTilePosition);
            _tileSelector.AddSelectedTile(newTilePosition);
            _currentTilePosition = newTilePosition;
        }

        public void ClearSelection()
        {
            _tileSelector.SetSelectedTiles(new List<Vector3Int>());
            _currentTilePosition = Vector3Int.zero;
        }
    }
}