using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Anonim.Systems.CombatSystem.TileSelection.TileSelectionMethods;
using Anonim.Systems.DungeonSystem;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    [System.Serializable]
    public class TileSelector
    {
        private Tilemap _tilemap;
        public List<Vector3Int> SelectedTiles = new List<Vector3Int>();
        private Color _selectionTint { get { return _isUsingPrimaryTint ? _primarySelectionTint : _secondarySelectionTint; } }
        [SerializeField] private Color _primarySelectionTint;
        [SerializeField] private Color _secondarySelectionTint;
        [SerializeField] private Tile _selectionTile;
        private bool _isUsingPrimaryTint = true;

        public void SetTilemap(Tilemap tilemap)
        {
            _tilemap = tilemap;
        }

        public void UpdateSelectedTiles(Vector3Int centerPosition, TileSelectionMethod tileSelectionMethod, uint selectionRadius = 1)
        {
            ClearSelection();
            SelectedTiles = tileSelectionMethod.GetSelectedTiles(centerPosition, selectionRadius);
            ApplySelection();
        }

        public List<Vector3Int> AddSelectedTiles(Vector3Int centerPosition, TileSelectionMethod tileSelectionMethod, uint selectionRadius = 1)
        {
            ClearSelection();
            List<Vector3Int> currentSelectedTiles = tileSelectionMethod.GetSelectedTiles(centerPosition, selectionRadius);
            SelectedTiles.AddRange(currentSelectedTiles);
            ApplySelection();
            return currentSelectedTiles;
        }

        public void SetSelectedTiles(List<List<Vector3Int>> newSelectedTiles)
        {
            ClearSelection();
            SelectedTiles.Clear();
            foreach (List<Vector3Int> currentList in newSelectedTiles)
            {
                SelectedTiles.AddRange(currentList);
            }
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

        public void ClearSelection()
        {
            foreach (var tilePos in SelectedTiles)
            {
                _tilemap.SetTile(tilePos, null);
            }
        }

        public void ToggleActiveSelectionTint()
        {
            _isUsingPrimaryTint = !_isUsingPrimaryTint;
        }
    }
}