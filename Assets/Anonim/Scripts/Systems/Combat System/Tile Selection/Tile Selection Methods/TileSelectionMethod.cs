using System.Collections.Generic;
using UnityEngine;

namespace Anonim.Systems.CombatSystem.TileSelection.TileSelectionMethods
{
    public abstract class TileSelectionMethod : ScriptableObject
    {
        public abstract List<Vector3Int> GetSelectedTiles(Vector3Int centerTilePosition, uint selectionRadius);
    }
}