using Anonim.Systems.DungeonSystem;
using Anonim.Systems.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Anonim.Systems.CombatSystem.Weapons;
using Anonim.Systems.TimeSystem;
using Anonim.Systems.StatSystem;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    public class EnemyTileSelector : MonoBehaviour
    {
        [SerializeField] private TileSelector _tileSelector;
        public WeaponData _weaponData;
        private Vector2Int _lastPlayerGridPosition;
        private Vector2Int _lastGridPosition;
        private StatComponent _statComponent;
        private AnonimTimer _attackTimer;
        private float _attackSpeed => _statComponent.GetStat(StatType.AttackSpeed);
        private float _detectionRange => _statComponent.GetStat(StatType.Perception) - GlobalPlayer.Instance.PlayerStatComponent.GetStat(StatType.Perception);
        private bool _canAttack = true;

        private void Awake()
        {
            _statComponent = GetComponent<StatComponent>();
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {
            _attackTimer.OnCompleted -= OnAttackTimerCompleted;
        }

        private void Start()
        {
            if (_attackSpeed > 0)
            {
                _attackTimer = new AnonimTimer(1.0f / _attackSpeed);
            }
            else
            {
                Debug.LogError(gameObject.name + "AttackSpeed is not set or is zero."); // Handle this case as needed, maybe disable movement
                return;
            }

            _attackTimer.OnCompleted += OnAttackTimerCompleted;
        }

        private void Update()
        {
            _attackTimer.Update(Time.deltaTime);
        }


        private void OnAttackTimerCompleted()
        {
            _canAttack = true;
            _tileSelector.ToggleActiveSelectionTint();
        }

        private void Attack()
        {
            if (_canAttack == false)
            {
                return;
            }
            _canAttack = false;
            _tileSelector.ToggleActiveSelectionTint();
            _attackTimer.Start();
            // Attack logic here
            foreach (Vector3Int position in _tileSelector.SelectedTiles)
            {
                GameObject enemyObject = EnemySpawner.Instance.GetEnemyInTile(position.x, position.y);
                if (enemyObject == null)
                {
                    continue;
                }
                Destroy(enemyObject);
            }
        }

        private Vector3Int UpdateSelectionCenter()
        {
            Vector2Int delta = _lastGridPosition - _lastPlayerGridPosition;

            if (delta.magnitude <= _weaponData.AttackRange)
            {
                return new Vector3Int(_lastGridPosition.x, _lastGridPosition.y);
            }

            Vector2 direction = delta;
            direction.Normalize();
            direction *= _weaponData.AttackRange;

            Vector3Int newCenter = new Vector3Int(Mathf.RoundToInt(_lastPlayerGridPosition.x + direction.x), Mathf.RoundToInt(_lastPlayerGridPosition.y + direction.y));

            return newCenter;
        }
    }
}