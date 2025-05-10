using System.Collections.Generic;
using Anonim.Systems.DungeonSystem;
using UnityEngine;

namespace Anonim.CombatSystem.TileSelection.TileSelectionMethods
{
    [CreateAssetMenu(fileName = "SingleTileSelectionMethod", menuName = "Scriptable Objects/Tile Selection Methods/Single Tile Selection Method")]
    public class SingleTileSelectionMethod : TileSelectionMethod
    {
        public override List<Vector3Int> GetSelectedTiles(Vector3Int centerTilePosition, uint selectionRadius)
        {
            if (DungeonGenerator.Instance.IsFloor(centerTilePosition.x, centerTilePosition.y))
            {
                return new List<Vector3Int> { centerTilePosition };
            }
            return null;
        }
    }
}