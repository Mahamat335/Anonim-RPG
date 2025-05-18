using System.Collections.Generic;
using Anonim.Systems.DungeonSystem;
using UnityEngine;

namespace Anonim.Systems.CombatSystem.TileSelection.TileSelectionMethods
{
    [CreateAssetMenu(fileName = "CrossTileSelectionMethod", menuName = "Scriptable Objects/Tile Selection Methods/Cross Tile Selection Method")]
    public class CrossTileSelectionMethod : TileSelectionMethod
    {
        public override List<Vector3Int> GetSelectedTiles(Vector3Int centerTilePosition, uint selectionRadius)
        {
            List<Vector3Int> selectedTiles = new List<Vector3Int>();

            if (DungeonGenerator.Instance.IsFloor(centerTilePosition.x, centerTilePosition.y))
            {
                selectedTiles.Add(centerTilePosition);
            }

            for (int i = -(int)selectionRadius; i <= selectionRadius; i++)
            {
                if (i == 0)
                {
                    continue;
                }

                if (DungeonGenerator.Instance.IsFloor(centerTilePosition.x + i, centerTilePosition.y))
                {
                    selectedTiles.Add(new Vector3Int(centerTilePosition.x + i, centerTilePosition.y));
                }

                if (DungeonGenerator.Instance.IsFloor(centerTilePosition.x, centerTilePosition.y + i))
                {
                    selectedTiles.Add(new Vector3Int(centerTilePosition.x, centerTilePosition.y + i));
                }
            }

            return selectedTiles;
        }
    }
}