using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Anonim.CombatSystem.TileSelection.TileSelectionMethods;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    [System.Serializable]
    public class TileSelector
    {
        [SerializeField] private Tilemap tilemap;
        public List<Vector3Int> SelectedTiles = new List<Vector3Int>();
        [SerializeField] private Color selectionTint;

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
                if (!tilemap.HasTile(tilePos))
                {
                    continue;
                }

                // Blend the color with the selection tint
                Color blendedColor = Color.Lerp(Color.white, selectionTint, selectionTint.a);
                blendedColor.a = 1.0f;
                tilemap.SetColor(tilePos, blendedColor);
            }
        }

        private void ClearSelection()
        {
            foreach (var tilePos in SelectedTiles)
            {
                if (tilemap.HasTile(tilePos))
                {
                    tilemap.SetColor(tilePos, Color.white);
                }
            }
        }
    }
}