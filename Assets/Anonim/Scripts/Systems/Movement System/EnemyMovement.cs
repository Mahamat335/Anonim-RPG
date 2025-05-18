using UnityEngine;
using System.Collections.Generic;
using Anonim.Systems.StatSystem;
using Anonim.Systems.DungeonSystem;
using System.Collections;
using Anonim.Systems.TimeSystem;

namespace Anonim.Systems.MovementSystem
{
    public class EnemyMovement : MonoBehaviour
    {
        private StatComponent _statComponent;
        private AnonimTimer _movementTimer;
        private Vector2Int _currentTile => DungeonGenerator.Instance.GetWorldToGridPosition(transform.position);
        private float _detectionRange => _statComponent.GetStat(StatType.Perception) - GlobalPlayer.Instance.PlayerStatComponent.GetStat(StatType.Perception);
        private float _movementSpeed => _statComponent.GetStat(StatType.MovementSpeed);
        private bool _canMove = true;
        private void Awake()
        {
            _statComponent = GetComponent<StatComponent>();
            _movementTimer = new AnonimTimer(1.0f / _statComponent.GetStat(StatType.MovementSpeed));
        }

        private void OnDisable()
        {
            _movementTimer.OnCompleted -= OnMovementTimerCompleted;
        }

        private void Start()
        {
            if (_movementSpeed > 0)
            {
                _movementTimer = new AnonimTimer(1.0f / _movementSpeed);
            }
            else
            {
                Debug.LogError("MovementSpeed is not set or is zero."); // Handle this case as needed, maybe disable movement
                return;
            }

            _movementTimer.OnCompleted += OnMovementTimerCompleted;
        }

        private void Update()
        {
            if (_canMove)
            {
                TryMove();
            }

            _movementTimer.Update(Time.deltaTime);
        }

        private void OnMovementTimerCompleted()
        {
            _canMove = true;
        }

        private void TryMove()
        {
            _canMove = false;
            _movementTimer.Start();

            Vector2Int playerPos = GlobalPlayer.Instance.PlayerPosition;

            if (Vector2Int.Distance(_currentTile, playerPos) <= _detectionRange)
            {
                List<Vector2Int> path = AStarPathfinder.Instance.FindPath(_currentTile, playerPos);
                if (path != null && path.Count > 1)
                {
                    Vector2Int nextTile = path[1]; // [0] == current position
                    if (nextTile != GlobalPlayer.Instance.PlayerPosition)
                        StartCoroutine(MoveTo(nextTile));
                }
            }
        }

        private IEnumerator MoveTo(Vector2Int tilePos)
        {
            EnemySpawner.Instance.MoveEnemy(gameObject, tilePos);

            // Calculate world target position
            Vector3 targetWorldPos = DungeonGenerator.Instance.GetGridToWorldPosition(tilePos);

            // Smoothly move it to the target
            float elapsedTime = 0f;
            float moveDuration = 0.2f; // You can tweak this to adjust how fast it moves
            Vector3 startPos = transform.position;

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPos, targetWorldPos, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetWorldPos;
        }

        private void OnDestroy()
        {
            EnemySpawner.Instance.RemoveEnemy(_currentTile);
        }
    }
}
