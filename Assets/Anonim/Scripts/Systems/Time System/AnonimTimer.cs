using System;
using UnityEngine;

namespace Anonim.Systems.TimeSystem
{
    public class AnonimTimer
    {
        private float _duration;
        private float _elapsedTime;
        private bool _isRunning;
        private bool _isLooping;

        public bool IsRunning => _isRunning;
        public bool IsCompleted => !_isRunning && _elapsedTime >= _duration;

        public event Action OnCompleted;

        public AnonimTimer(float duration, bool isLooping = false)
        {
            _duration = duration;
            _elapsedTime = 0f;
            _isRunning = false;
            _isLooping = isLooping;
        }

        public void Start()
        {
            _elapsedTime = 0f;
            _isRunning = true;
        }

        public void Stop()
        {
            _isRunning = false;
        }

        public void Update(float deltaTime)
        {
            if (!_isRunning)
                return;

            _elapsedTime += deltaTime;

            if (_elapsedTime >= _duration)
            {
                _isRunning = false;
                OnCompleted?.Invoke();

                if (_isLooping)
                {
                    Reset(_duration);
                }
            }
        }

        public void Reset(float newDuration)
        {
            _duration = newDuration;
            _elapsedTime = 0f;
            _isRunning = true;
        }
    }
}