using System.Collections.Generic;
using UnityEngine;

namespace Anonim.Systems.DungeonSystem
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private int _enemyCount = 10;

        private void Start()
        {
            SpawnEnemies(); // You might wanna use OnDungeonGenerated callback
        }

        private void SpawnEnemies()
        {
            DungeonGenerator dungeon = DungeonGenerator.Instance;
            List<Vector2Int> floorPositions = new List<Vector2Int>();

            // Tüm floor tile'ları bul
            for (int x = 0; x < dungeon.Width; x++)
            {
                for (int y = 0; y < dungeon.Height; y++)
                {
                    if (dungeon.IsFloor(x, y))
                    {
                        floorPositions.Add(new Vector2Int(x, y));
                    }
                }
            }

            // Eğer floor tile yoksa çık
            if (floorPositions.Count == 0)
            {
                Debug.LogWarning("No floor tiles found to spawn enemies.");
                return;
            }

            // Enemyleri rastgele floor tile'lara yerleştir
            for (int i = 0; i < _enemyCount; i++)
            {
                Vector2Int spawnGridPos = floorPositions[Random.Range(0, floorPositions.Count)];
                Vector2 worldPos = dungeon.GetGridToWorldPosition(spawnGridPos);
                Instantiate(_enemyPrefab, worldPos, Quaternion.identity);
            }
        }
    }
}
