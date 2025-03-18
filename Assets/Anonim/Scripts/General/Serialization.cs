using System.Collections.Generic;

namespace Anonim
{
    [System.Serializable]
    public class Serialization<TKey, TValue>
    {
        private List<TValue> _listTarget;
        private Dictionary<TKey, TValue> _dictionaryTarget;

        public Serialization(List<TValue> target)
        {
            _listTarget = target;
        }

        public Serialization(Dictionary<TKey, TValue> target)
        {
            _dictionaryTarget = target;
        }

        public List<TValue> ToList()
        {
            return _listTarget;
        }

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return _dictionaryTarget;
        }
    }

}