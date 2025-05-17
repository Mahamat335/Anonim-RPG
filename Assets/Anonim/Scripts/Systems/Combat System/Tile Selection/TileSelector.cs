using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Anonim.CombatSystem.TileSelection.TileSelectionMethods;
using Anonim.Systems.DungeonSystem;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    [System.Serializable]
    public class TileSelector
    {
        [SerializeField] private Tilemap _tilemap;
        public List<Vector3Int> SelectedTiles = new List<Vector3Int>();
        [SerializeField] private Color _selectionTint;
        [SerializeField] private Tile _selectionTile;

        public void UpdateSelectedTiles(Vector3Int centerPosition, TileSelectionMethod tileSelectionMethod, uint selectionRadius = 1)
        {
            ClearSelection();
            SelectedTiles = tileSelectionMethod.GetSelectedTiles(centerPosition, selectionRadius);
            ApplySelection();
        }

        private void ApplySelection()
        {
            foreach (var tilePos in SelectedTiles)
            {
                if (DungeonGenerator.Instance.IsFloor(tilePos.x, tilePos.y) == false)
                {
                    continue;
                }


                if (_tilemap.HasTile(tilePos))
                {
                    Color currentColor = _tilemap.GetColor(tilePos);
                    Color blendedColor = Color.Lerp(currentColor, _selectionTint, 0.5f);
                    _tilemap.SetColor(tilePos, blendedColor);
                }
                else
                {
                    _tilemap.SetTile(tilePos, _selectionTile);
                    _tilemap.SetColor(tilePos, _selectionTint);
                }
            }
        }

        private void ClearSelection()
        {
            foreach (var tilePos in SelectedTiles)
            {
                _tilemap.SetTile(tilePos, null);
            }
        }
    }
}