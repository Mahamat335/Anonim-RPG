using UnityEngine;

namespace Anonim.Systems.DungeonSystem
{
    public class DungeonGenerator : Singleton<DungeonGenerator>
    {
        public int Width = 150;
        public int Height = 150;
        public float InitialRockPercentage = 0.5f;
        public int IterationCount = 3;
        public int RockThreshold = 13;
        public int MooreNeighborhoodSize = 2;
        public int[,] Grid => _grid;

        // This is the sprite renderer that will display the dungeon
        public SpriteRenderer DungeonSprite;

        private int[,] _grid;
        private Texture2D _texture;
        [SerializeField] private Vector2 _gridSize = new Vector2(2.56f, 2.56f);

        private void OnEnable()
        {
            GenerateDungeon();
        }

        private void GenerateDungeon()
        {
            _grid = new int[Width, Height];
            _texture = new Texture2D(Width, Height, TextureFormat.RGBA32, false);
            _texture.filterMode = FilterMode.Point;
            _texture.wrapMode = TextureWrapMode.Clamp;
            DungeonSprite.sprite = Sprite.Create(_texture, new Rect(0, 0, Width, Height), new Vector2(0.5f, 0.5f), 1);

            RandomFillGrid();

            for (int i = 0; i < IterationCount; i++)
            {
                ApplyCellularAutomata();
            }

            DrawGrid();
            _texture.Apply();
        }

        private void RandomFillGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _grid[x, y] = Random.value < InitialRockPercentage ? 1 : 0;
                }
            }
        }

        private void ApplyCellularAutomata()
        {
            int[,] newGrid = new int[Width, Height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int rocks = CountRockNeighbors(x, y);
                    newGrid[x, y] = rocks >= RockThreshold ? 1 : 0;
                }
            }

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _grid[x, y] = newGrid[x, y];
                }
            }
        }

        private int CountRockNeighbors(int x, int y)
        {
            int count = 0;
            for (int dx = -MooreNeighborhoodSize; dx <= MooreNeighborhoodSize; dx++)
            {
                for (int dy = -MooreNeighborhoodSize; dy <= MooreNeighborhoodSize; dy++)
                {
                    if (dx == 0 && dy == 0)
                    {
                        continue;
                    }

                    int nx = x + dx;
                    int ny = y + dy;

                    if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && _grid[nx, ny] == 1)
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private void DrawGrid()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Color color = Color.black;

                    if (_grid[x, y] == 0)
                    {
                        color = Color.white;
                    }
                    else if (IsWall(x, y))
                    {
                        color = Color.gray;
                    }

                    _texture.SetPixel(x, y, color);
                }
            }
        }

        public bool IsWall(int x, int y)
        {
            if (IsWithinBounds(x, y) == false)
            {
                return false;
            }

            if (_grid[x, y] == 0)
            {
                return false;
            }

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    if (nx >= 0 && nx < Width && ny >= 0 && ny < Height && _grid[nx, ny] == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsFloor(int x, int y)
        {
            if (IsWithinBounds(x, y))
            {
                return _grid[x, y] == 0;
            }
            return false;
        }

        public bool IsWithinBounds(int x, int y)
        {
            if (x < 0 || x >= Width || y < 0 || y >= Height)
            {
                return false;
            }
            return true;
        }

        public Vector2 GetGridToWorldPosition(Vector2Int gridPosition)
        {
            return new Vector2((gridPosition.x + 0.5f) * _gridSize.x, (gridPosition.y + 0.5f) * _gridSize.y);
        }

        public Vector2Int GetWorldToGridPosition(Vector2 worldPosition)
        {
            return new Vector2Int(Mathf.FloorToInt(worldPosition.x / _gridSize.x), Mathf.FloorToInt(worldPosition.y / _gridSize.y));
        }
    }

}

