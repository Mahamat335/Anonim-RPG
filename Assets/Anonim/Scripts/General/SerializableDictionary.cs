using System;
using System.Collections.Generic;
using UnityEngine;

namespace Anonim
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue>
    {
        [SerializeField]
        SerializableDictionaryItem<TKey, TValue>[] serializableDictionaryItems;

        public Dictionary<TKey, TValue> ToDictionary()
        {
            Dictionary<TKey, TValue> resultDictionary = new Dictionary<TKey, TValue>();
            foreach (KeyValuePair<TKey, TValue> keyValuePair in resultDictionary)
            {
                resultDictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
            return resultDictionary;
        }
    }

    [Serializable]
    public class SerializableDictionaryItem<TKey, TValue>
    {
        [SerializeField]
        public TKey key;
        [SerializeField]
        public TValue value;
    }
}
