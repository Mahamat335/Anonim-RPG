using System.Collections.Generic;
using UnityEngine;

namespace Anonim.CombatSystem.TileSelection.TileSelectionMethods
{
    [CreateAssetMenu(fileName = "SingleTileSelectionMethod", menuName = "Scriptable Objects/Tile Selection Methods/Single Tile Selection Method")]
    public class SingleTileSelectionMethod : TileSelectionMethod
    {
        public override List<Vector3Int> GetSelectedTiles(Vector3Int centerTilePosition, uint selectionRadius)
        {
            return new List<Vector3Int> { centerTilePosition };
        }
    }
}