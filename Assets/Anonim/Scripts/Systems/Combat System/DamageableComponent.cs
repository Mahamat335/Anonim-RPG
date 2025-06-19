using TMPro;
using UnityEngine;
using Anonim.Systems.StatSystem;
using UnityEngine.SocialPlatforms.Impl;

namespace Anonim.Systems.CombatSystem
{
    public class DamageableComponent : MonoBehaviour
    {
        [SerializeField] private TMP_Text _healthText;

        private StatComponent _statComponent;
        private float _currentHealth;
        private float _maxHealth;
        [SerializeField] private bool _isPlayer = false;

        private void Start()
        {
            _statComponent = GetComponent<StatComponent>();
            if (_statComponent == null)
            {
                Debug.LogError("StatComponent not found on the GameObject.", this);
                enabled = false;
                return;
            }

            _maxHealth = _statComponent.GetStat(StatType.MaxHealth);
            _currentHealth = _maxHealth;

            UpdateHealthText();
        }

        public void TakeDamage(float amount)
        {
            _currentHealth -= amount;
            _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
            UpdateHealthText();

            // Optional: Destroy or disable when health reaches zero
            if (_currentHealth <= 0f)
            {
                OnDeath();
            }
        }

        private void UpdateHealthText()
        {
            if (_healthText != null)
            {
                _healthText.text = $"{Mathf.CeilToInt(_currentHealth)} / {Mathf.CeilToInt(_maxHealth)}";
            }
        }

        private void OnDeath()
        {
            if (_isPlayer)
            {
                Time.timeScale = 0f;
                gameObject.SetActive(false);
                ScoreBoard.Instance.OpenCanvas();
            }
            else
            {
                Destroy(gameObject);
                ScoreBoard.Instance.AddScore(75);
            }
        }
    }
}
