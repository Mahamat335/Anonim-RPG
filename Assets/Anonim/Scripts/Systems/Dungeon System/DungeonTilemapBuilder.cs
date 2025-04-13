using UnityEngine;
using UnityEngine.Tilemaps;

namespace Anonim.Systems.DungeonSystem
{
    public class DungeonTilemapBuilder : MonoBehaviour
    {
        [Header("References")]
        private DungeonGenerator _generator;
        [SerializeField] private Tilemap _tilemap;

        [Header("Tiles")]
        [SerializeField] private TileBase _groundTile;
        [SerializeField] private TileBase _wallTile;
        [SerializeField] private TileBase _rockTile;

        private void Start()
        {
            _generator = DungeonGenerator.Instance;
            BuildFromGenerator();
        }

        private void BuildFromGenerator()
        {
            int width = _generator.Width;
            int height = _generator.Height;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3Int pos = new Vector3Int(x, y, 0);

                    if (_generator.Grid[x, y] == 0)
                    {
                        _tilemap.SetTile(pos, _groundTile);
                    }
                    else if (_generator.IsWall(x, y))
                    {
                        _tilemap.SetTile(pos, _wallTile);
                    }
                    else
                    {
                        _tilemap.SetTile(pos, _rockTile);
                    }
                }
            }
        }
    }
}
