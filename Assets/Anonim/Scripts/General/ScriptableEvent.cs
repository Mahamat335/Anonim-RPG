using System;
using Sigtrap.Relays;
using UnityEngine;

namespace Anonim
{
    [CreateAssetMenu(fileName = "ScriptableEvent", menuName = "Scriptable Objects/Scriptable Events/Scriptable Event")]
    public class ScriptableEvent : ScriptableObject
    {
        private Relay _event = new();

        public void AddListener(Action listener)
        {
            _event.AddListener(listener);
        }

        public void AddOnce(Action listener)
        {
            _event.AddOnce(listener);
        }

        public void RemoveListener(Action listener)
        {
            _event.RemoveListener(listener);
        }

        public void RemoveOnce(Action listener)
        {
            _event.RemoveOnce(listener);
        }

        public void RemoveAll()
        {
            _event.RemoveAll();
        }

        public void Dispatch()
        {
            _event.Dispatch();
        }
    }

    public class ScriptableEvent<T> : ScriptableObject
    {
        private Relay<T> _event = new();

        public void AddListener(Action<T> listener)
        {
            _event.AddListener(listener);
        }

        public void AddOnce(Action<T> listener)
        {
            _event.AddOnce(listener);
        }

        public void RemoveListener(Action<T> listener)
        {
            _event.RemoveListener(listener);
        }

        public void RemoveOnce(Action<T> listener)
        {
            _event.RemoveOnce(listener);
        }

        public void RemoveAll()
        {
            _event.RemoveAll();
        }

        public void Dispatch(T t)
        {
            _event.Dispatch(t);
        }
    }

    public class ScriptableEvent<T, TK> : ScriptableObject
    {
        private Relay<T, TK> _event = new();

        public void AddListener(Action<T, TK> listener)
        {
            _event.AddListener(listener);
        }

        public void AddOnce(Action<T, TK> listener)
        {
            _event.AddOnce(listener);
        }

        public void RemoveListener(Action<T, TK> listener)
        {
            _event.RemoveListener(listener);
        }

        public void RemoveOnce(Action<T, TK> listener)
        {
            _event.RemoveOnce(listener);
        }

        public void RemoveAll()
        {
            _event.RemoveAll();
        }

        public void Dispatch(T t, TK k)
        {
            _event.Dispatch(t, k);
        }
    }

    public class ScriptableEvent<T, TK, TV> : ScriptableObject
    {
        private Relay<T, TK, TV> _event = new();

        public void AddListener(Action<T, TK, TV> listener)
        {
            _event.AddListener(listener);
        }

        public void AddOnce(Action<T, TK, TV> listener)
        {
            _event.AddOnce(listener);
        }

        public void RemoveListener(Action<T, TK, TV> listener)
        {
            _event.RemoveListener(listener);
        }

        public void RemoveOnce(Action<T, TK, TV> listener)
        {
            _event.RemoveOnce(listener);
        }

        public void RemoveAll()
        {
            _event.RemoveAll();
        }

        public void Dispatch(T t, TK k, TV v)
        {
            _event.Dispatch(t, k, v);
        }
    }
}
