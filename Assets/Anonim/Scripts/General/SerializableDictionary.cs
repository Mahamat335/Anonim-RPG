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
            foreach (SerializableDictionaryItem<TKey, TValue> keyValuePair in serializableDictionaryItems)
            {
                resultDictionary.Add(keyValuePair.key, keyValuePair.value);
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
