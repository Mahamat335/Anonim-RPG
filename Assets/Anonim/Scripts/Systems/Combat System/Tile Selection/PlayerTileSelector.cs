using Anonim.Systems.DungeonSystem;
using Anonim.Systems.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Anonim.Systems.CombatSystem.Weapons;
using Anonim.Systems.TimeSystem;
using Anonim.Systems.StatSystem;

namespace Anonim.Systems.CombatSystem.TileSelection
{
    public class PlayerTileSelector : MonoBehaviour
    {
        [SerializeField] private TileSelector _tileSelector;
        public WeaponData _weaponData;
        private Vector2Int _lastPlayerGridPosition;
        private Vector2Int _lastGridPosition;
        private StatComponent _statComponent;
        private AnonimTimer _attackTimer;
        private float _playerAttackSpeed => _statComponent.GetStat(StatType.AttackSpeed);
        private bool _canAttack = true;

        private void Awake()
        {
            _statComponent = GetComponent<StatComponent>();
        }

        private void OnEnable()
        {
            EventManager.Instance.PlayerMouseLeftClickInput.AddListener(OnMouseLeftClick);
            EventManager.Instance.PlayerMousePositionInput.AddListener(OnMouseMove);
            EventManager.Instance.PlayerMovementCompleted.AddListener(OnPlayerMovementCompleted);
            EventManager.Instance.StatModifierAdded.AddListener(OnStatModifierChanged); // If you want to disable player somehow, you might wanna check this line
        }

        private void OnDisable()
        {
            EventManager.Instance.PlayerMouseLeftClickInput.RemoveListener(OnMouseLeftClick);
            EventManager.Instance.PlayerMousePositionInput.RemoveListener(OnMouseMove);
            EventManager.Instance.PlayerMovementCompleted.RemoveListener(OnPlayerMovementCompleted);
            EventManager.Instance.StatModifierAdded.RemoveListener(OnStatModifierChanged); // If you want to disable player somehow, you might wanna check this line
            _attackTimer.OnCompleted -= OnAttackTimerCompleted;
        }

        private void Start()
        {
            if (_playerAttackSpeed > 0)
            {
                _attackTimer = new AnonimTimer(1.0f / _playerAttackSpeed);
            }
            else
            {
                Debug.LogError("PlayerMovementSpeed is not set or is zero."); // Handle this case as needed, maybe disable movement
                return;
            }

            _attackTimer.OnCompleted += OnAttackTimerCompleted;
        }

        private void Update()
        {
            _attackTimer.Update(Time.deltaTime);
        }


        #region Input Actions
        private void OnMouseLeftClick(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Attack();
            }
        }

        private void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                Vector2 mousePosition = context.ReadValue<Vector2>();
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

                Vector2Int gridPosition = DungeonGenerator.Instance.GetWorldToGridPosition(new Vector2(worldPosition.x, worldPosition.y));

                if (gridPosition == _lastGridPosition)
                {
                    return;
                }

                _lastGridPosition = gridPosition;

                Vector3Int selectionCenter = UpdateSelectionCenter();

                _tileSelector.UpdateSelectedTiles(selectionCenter, _weaponData.TileSelectionMethod, _weaponData.AttackRadius);
            }
        }

        private void OnPlayerMovementCompleted(Vector2Int newPlayerGridPosition)
        {
            if (newPlayerGridPosition != _lastPlayerGridPosition)
            {
                _lastPlayerGridPosition = newPlayerGridPosition;
                Vector3Int selectionCenter = UpdateSelectionCenter();
                _tileSelector.UpdateSelectedTiles(selectionCenter, _weaponData.TileSelectionMethod, _weaponData.AttackRadius);
            }
        }
        #endregion

        private void OnStatModifierChanged(Stat stat)
        {
            if (stat.Type == StatType.MovementSpeed)
            {
                _attackTimer.OnCompleted -= OnAttackTimerCompleted; // Unsubscribe from the old timer, I'm not sure if this is necessary but it's a good practice
                if (_playerAttackSpeed > 0)
                {
                    _attackTimer = new AnonimTimer(1.0f / _playerAttackSpeed);
                }
                else
                {
                    Debug.LogError("PlayerMovementSpeed is not set or is zero."); // Handle this case as needed, maybe disable movement
                    return;
                }
                _attackTimer.OnCompleted += OnAttackTimerCompleted;
            }
        }

        private void OnAttackTimerCompleted()
        {
            _canAttack = true;
        }

        private void Attack()
        {
            if (_canAttack == false)
            {
                return;
            }
            _canAttack = false;
            _attackTimer.Start();
            // Attack logic here
            foreach (Vector3Int position in _tileSelector.SelectedTiles)
            {
                Debug.Log(position);
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