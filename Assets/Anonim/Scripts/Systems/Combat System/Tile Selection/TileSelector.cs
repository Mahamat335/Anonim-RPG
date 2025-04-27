using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    [System.Serializable]
    public class TileSelector
    {
        [SerializeField] private Tilemap tilemap;
        private List<Vector3Int> selectedTiles = new List<Vector3Int>();
        [SerializeField] private Color selectionTint;

        public void SetSelectedTiles(List<Vector3Int> tiles)
        {
            ClearSelection();
            selectedTiles = tiles;
            ApplySelection();
        }

        public void AddSelectedTile(Vector3Int tilePos)
        {
            if (!selectedTiles.Contains(tilePos))
            {
                ClearSelection();
                selectedTiles.Add(tilePos);
                ApplySelection();
            }
        }

        public void RemoveSelectedTile(Vector3Int tilePos)
        {
            if (selectedTiles.Contains(tilePos))
            {
                ClearSelection();
                selectedTiles.Remove(tilePos);
                ApplySelection();
            }
        }

        private void ApplySelection()
        {
            foreach (var tilePos in selectedTiles)
            {
                if (!tilemap.HasTile(tilePos))
                    continue;

                // Tile'ın rengi normalde Color.white olduğu için, selectionTint'ı alpha oranında uygula
                Color blendedColor = Color.Lerp(Color.white, selectionTint, selectionTint.a);
                blendedColor.a = 1.0f;
                tilemap.SetColor(tilePos, blendedColor);
            }
        }

        private void ClearSelection()
        {
            foreach (var tilePos in selectedTiles)
            {
                if (tilemap.HasTile(tilePos))
                {
                    tilemap.SetColor(tilePos, Color.white);
                }
            }
        }
    }
}