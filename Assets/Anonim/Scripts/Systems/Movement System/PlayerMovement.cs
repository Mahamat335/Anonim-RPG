using UnityEngine;
using Anonim.Systems.EventSystem;
using Anonim.Systems.StatSystem;
using UnityEngine.InputSystem;
using Anonim.Systems.TimeSystem;
using System.Collections;
using Anonim.Systems.DungeonSystem;

namespace Anonim.Systems.MovementSystem
{
    public class PlayerMovementInput : MonoBehaviour
    {
        private StatComponent _statComponent;
        private AnonimTimer _movementTimer;
        private float _playerMovementSpeed => _statComponent.GetStat(StatType.MovementSpeed);
        private bool _canMove = true;
        private bool _isMoving = false;
        private Vector2 _movementInput;
        [SerializeField] private Transform _rendererTransform;

        private void Awake()
        {
            _statComponent = GetComponent<StatComponent>();
        }

        private void OnEnable()
        {
            EventManager.Instance.PlayerMovementInput.AddListener(OnPlayerMovementInput);
            EventManager.Instance.StatModifierAdded.AddListener(OnStatModifierChanged); // If you want to disable player somehow, you might wanna check this line
        }

        private void OnDisable()
        {
            EventManager.Instance.PlayerMovementInput.RemoveListener(OnPlayerMovementInput);
            _movementTimer.OnCompleted -= OnMovementTimerCompleted; // If you want to disable player somehow, you might wanna check this line
        }

        private void Start()
        {
            if (_playerMovementSpeed > 0)
            {
                _movementTimer = new AnonimTimer(1.0f / _playerMovementSpeed);
            }
            else
            {
                Debug.LogError("PlayerMovementSpeed is not set or is zero."); // Handle this case as needed, maybe disable movement
                return;
            }

            _movementTimer.OnCompleted += OnMovementTimerCompleted;
            // TODO: Set this in another script, dungeon generation maybe
            transform.position = DungeonGenerator.Instance.GetGridToWorldPosition(new Vector2Int(0, 0)); // Set initial position to (0,0) grid position
        }

        private void Update()
        {
            if (_isMoving && _canMove)
            {
                MovePlayer();
            }

            _movementTimer.Update(Time.deltaTime);
        }

        private void OnStatModifierChanged(Stat stat)
        {
            if (stat.Type == StatType.MovementSpeed)
            {
                _movementTimer.OnCompleted -= OnMovementTimerCompleted; // Unsubscribe from the old timer, I'm not sure if this is necessary but it's a good practice
                if (_playerMovementSpeed > 0)
                {
                    _movementTimer = new AnonimTimer(1.0f / _playerMovementSpeed);
                }
                else
                {
                    Debug.LogError("PlayerMovementSpeed is not set or is zero."); // Handle this case as needed, maybe disable movement
                    return;
                }
                _movementTimer.OnCompleted += OnMovementTimerCompleted;
            }
        }

        private void MovePlayer()
        {
            _canMove = false;
            _movementTimer.Start();
            StartCoroutine(PlayerMovementAnimation());
        }

        private void OnPlayerMovementInput(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                _movementInput = context.ReadValue<Vector2>();
                _isMoving = true;
            }
            else if (context.canceled)
            {
                _movementInput = Vector2.zero;
                _isMoving = false;
            }
            else if (context.performed)
            {
                _movementInput = context.ReadValue<Vector2>();
            }
        }

        private IEnumerator PlayerMovementAnimation()
        {
            Vector2 movementInput = new Vector2(
                Mathf.Round(_movementInput.x),
                Mathf.Round(_movementInput.y)
            );

            // Allow only horizontal or vertical moves (no diagonal)
            if (Mathf.Abs(movementInput.x) > 0 && Mathf.Abs(movementInput.y) > 0)
            {
                yield break;
            }

            // Calculate current and target grid positions
            Vector3 currentPosition = transform.position;
            Vector2Int currentGridPos = DungeonGenerator.Instance.GetWorldToGridPosition(currentPosition);
            Vector2Int targetGridPos = currentGridPos + Vector2Int.RoundToInt(movementInput);

            // Check bounds
            if (targetGridPos.x < 0 || targetGridPos.x >= DungeonGenerator.Instance.Width ||
                targetGridPos.y < 0 || targetGridPos.y >= DungeonGenerator.Instance.Height)
            {
                yield break;
            }

            // Check if target tile is walkable (white tiles only)
            if (DungeonGenerator.Instance.Grid[targetGridPos.x, targetGridPos.y] != 0)
            {
                yield break;
            }

            // Play movement animation
            PlayMovementAnimation(movementInput);

            // Calculate world target position
            Vector3 targetWorldPos = DungeonGenerator.Instance.GetGridToWorldPosition(targetGridPos);

            // Smoothly move player to the target
            float elapsedTime = 0f;
            float moveDuration = 0.2f; // You can tweak this to adjust how fast player moves
            Vector3 startPos = transform.position;

            while (elapsedTime < moveDuration)
            {
                transform.position = Vector3.Lerp(startPos, targetWorldPos, elapsedTime / moveDuration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetWorldPos;
            EventManager.Instance.PlayerMovementCompleted.Dispatch(targetGridPos); // Notify other systems about the new position
        }

        private void PlayMovementAnimation(Vector2 direction)
        {
            if (direction.x * _rendererTransform.localScale.x < 0)
            {
                _rendererTransform.localScale = new Vector3(-_rendererTransform.localScale.x, _rendererTransform.localScale.y, _rendererTransform.localScale.z);
            }

            // Burada kendi Animator parametrelerini kullanacaksın
            // Örnek: Animator'da "MoveX" ve "MoveY" float parametrelerin olsun.

            /*  Animator animator = GetComponent<Animator>();
             if (animator == null)
             {
                 return;
             }

             animator.SetFloat("MoveX", direction.x);
             animator.SetFloat("MoveY", direction.y);
             animator.SetTrigger("Walk"); // Diyelim ki 'Walk' trigger parametren var
              */
        }



        private void OnMovementTimerCompleted()
        {
            _canMove = true;
        }
    }
}