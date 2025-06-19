using Anonim.Systems.DungeonSystem;
using Anonim.Systems.EventSystem;
using UnityEngine;
using Anonim.Systems.CombatSystem.Weapons;
using Anonim.Systems.TimeSystem;
using Anonim.Systems.StatSystem;
using System.Collections.Generic;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    public class EnemyTileSelector : MonoBehaviour
    {
        [SerializeField] private TileSelector _tileSelector;
        public WeaponData _weaponData;
        private StatComponent _statComponent;
        private AnonimTimer _attackTimer;
        private AnonimTimer _attackDelay = new(1.0f);
        private Vector2Int _currentPosition => DungeonGenerator.Instance.GetWorldToGridPosition(transform.position);
        private List<List<Vector3Int>> _selectedTiles = new();

        private float _attackSpeed => _statComponent.GetStat(StatType.AttackSpeed);
        private float _detectionRange => _statComponent.GetStat(StatType.Perception) - GlobalPlayer.Instance.PlayerStatComponent.GetStat(StatType.Perception);

        private bool _canAttack = true;

        private void Awake()
        {
            _statComponent = GetComponent<StatComponent>();
        }

        private void OnEnable()
        {
            _attackDelay.OnCompleted += OnAttackDelayCompleted;
        }

        private void OnDisable()
        {
            if (_attackTimer != null)
            {
                _attackTimer.OnCompleted -= OnAttackTimerCompleted;
            }
            _attackDelay.OnCompleted -= OnAttackDelayCompleted;
        }

        private void Start()
        {
            _tileSelector.SetTilemap(SelectionTileMap.Instance.SelectionTilemap);
            if (_attackSpeed > 0)
            {
                _attackTimer = new AnonimTimer(1.0f / _attackSpeed);
                _attackTimer.OnCompleted += OnAttackTimerCompleted;
            }
            else
            {
                Debug.LogError($"{gameObject.name} AttackSpeed is not set or is zero.");
            }
        }

        private void Update()
        {
            _attackTimer?.Update(Time.deltaTime);
            _attackDelay.Update(Time.deltaTime);

            float distance = Vector2Int.Distance(_currentPosition, GlobalPlayer.Instance.PlayerPosition);
            if (distance > _detectionRange)
                return;


            if (_canAttack)
            {
                Vector3Int selectionCenter = UpdateSelectionCenter();
                _selectedTiles.Add(_tileSelector.AddSelectedTiles(selectionCenter, _weaponData.TileSelectionMethod, _weaponData.AttackRadius));
                Attack();
            }
        }

        private void OnAttackTimerCompleted()
        {
            _canAttack = true;
        }

        private void OnAttackDelayCompleted()
        {
            foreach (Vector3Int position in _tileSelector.SelectedTiles)
            {
                if (GlobalPlayer.Instance.PlayerPosition == new Vector2Int(position.x, position.y))
                {
                    GlobalPlayer.Instance.PlayerDamageableComponent.TakeDamage(1.0f);
                    break;
                }
            }
            _selectedTiles.RemoveAt(0);
            _tileSelector.SetSelectedTiles(_selectedTiles);
        }

        private void Attack()
        {
            _canAttack = false;
            _attackTimer.Start();
            _attackDelay.Start();
        }

        private Vector3Int UpdateSelectionCenter()
        {
            Vector2Int delta = GlobalPlayer.Instance.PlayerPosition - _currentPosition;

            if (delta.magnitude <= _weaponData.AttackRange)
            {
                return new Vector3Int(GlobalPlayer.Instance.PlayerPosition.x, GlobalPlayer.Instance.PlayerPosition.y);
            }

            Vector2 direction = ((Vector2)delta).normalized * _weaponData.AttackRange;

            return new Vector3Int(
                Mathf.RoundToInt(_currentPosition.x + direction.x),
                Mathf.RoundToInt(_currentPosition.y + direction.y)
            );
        }

        private void OnDestroy()
        {
            _tileSelector.ClearSelection();
        }
    }
}
