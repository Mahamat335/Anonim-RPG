using System.Collections.Generic;
using Anonim.Systems.StatSystem;
using UnityEngine;

namespace Anonim.Systems.DungeonSystem
{
    public class EnemySpawner : Singleton<EnemySpawner>
    {
        [SerializeField] private SerializableDictionary<GameObject, int> _enemiesSerializableDictionary;
        private Dictionary<GameObject, int> _enemiesDictionary;
        private Dictionary<Vector2Int, GameObject> _enemies = new();
        private Transform _enemyParentTransform;

        protected override void Awake()
        {
            base.Awake();
            _enemiesDictionary = _enemiesSerializableDictionary.ToDictionary();
        }

        private void Start()
        {
            SpawnEnemies(); // You might wanna use OnDungeonGenerated callback
        }

        private void SpawnEnemies()
        {
            _enemyParentTransform = Instantiate(new GameObject("Enemies").transform);

            foreach (KeyValuePair<GameObject, int> kvp in _enemiesDictionary)
            {
                for (int i = 0; i < kvp.Value; i++)
                {
                    Vector2Int spawnGridPos = GetRandomPosition();
                    Vector2 worldPos = DungeonGenerator.Instance.GetGridToWorldPosition(spawnGridPos);
                    _enemies.Add(spawnGridPos, Instantiate(kvp.Key, worldPos, Quaternion.identity, _enemyParentTransform));
                }
            }
        }

        private Vector2Int GetRandomPosition()
        {
            int width = DungeonGenerator.Instance.Width;
            int height = DungeonGenerator.Instance.Height;
            Vector2Int position;

            do
            {
                position = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
            } while (_enemies.ContainsKey(position) || DungeonGenerator.Instance.IsFloor(position.x, position.y) == false);

            return position;
        }

        public GameObject GetEnemyInTile(Vector2Int targetPosition)
        {
            return _enemies.ContainsKey(targetPosition) ? _enemies[targetPosition] : null;
        }

        public GameObject GetEnemyInTile(int x, int y)
        {
            Vector2Int targetPosition = new(x, y);
            return _enemies.ContainsKey(targetPosition) ? _enemies[targetPosition] : null;
        }

        public void MoveEnemy(GameObject gameObject, Vector2Int newPosition)
        {
            Vector2Int? currentKey = null;

            foreach (var kvp in _enemies)
            {
                if (kvp.Value == gameObject)
                {
                    currentKey = kvp.Key;
                    break;
                }
            }

            if (currentKey.HasValue)
            {
                _enemies.Remove(currentKey.Value);
                _enemies[newPosition] = gameObject;
            }
            else
            {
                Debug.LogWarning("Enemy not found in dictionary.");
            }
        }

        public void RemoveEnemy(Vector2Int position)
        {
            _enemies.Remove(position);
        }
    }
}
