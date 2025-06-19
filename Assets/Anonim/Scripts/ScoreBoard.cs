using System;
using UnityEngine;
using TMPro;

namespace Anonim
{
    public class ScoreBoard : Singleton<ScoreBoard>
    {
        private int _score = 0;
        [SerializeField] private GameObject _scoreBoardCanvas;
        [SerializeField] private TMP_Text _scoreText;

        public void AddScore(int amount)
        {
            _score += amount;
        }

        public void OpenCanvas()
        {
            _scoreBoardCanvas.SetActive(true);
            UpdateScoreText();
        }

        private void UpdateScoreText()
        {
            _scoreText.text = $"Score: {_score}";
        }
    }
}