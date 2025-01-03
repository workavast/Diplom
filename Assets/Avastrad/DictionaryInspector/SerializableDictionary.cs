using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Avastrad.DictionaryInspector
{
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IDictionary<TKey, TValue>, ISerializationCallbackReceiver, IEnumerable
    {
        [SerializeField] private List<DictionaryCell> cells = new List<DictionaryCell>();
    
        private Dictionary<TKey, TValue> _dictionary = new Dictionary<TKey, TValue>();
    
        public TValue this[TKey key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }
    
        public int Count => _dictionary.Count;
        public bool IsReadOnly => false;
    
        public IEnumerable<TKey> Keys => _dictionary.Keys;
        public IEnumerable<TValue> Values => _dictionary.Values;
    
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => _dictionary.Keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => _dictionary.Values;
    
        public bool ContainsKey(TKey key) => _dictionary.ContainsKey(key);
        public bool ContainsValue(TValue value) => _dictionary.ContainsValue(value);
        public bool TryGetValue(TKey key, out TValue value) => _dictionary.TryGetValue(key, out value);

        public bool TryAdd(TKey key, TValue value)
        {
            if (_dictionary.TryAdd(key, value))
            {
                cells.Add(new DictionaryCell(key, value));
                return true;
            }

            return false;
        }
    
        public void Add(TKey key, TValue value)
        {
            _dictionary.Add(key, value);
            cells.Add(new DictionaryCell(key, value));
        }

        public bool Remove(TKey key)
        {
            if (_dictionary.Remove(key))
            {
                var removeIndex = cells.FindIndex(v => v.key.Equals(key));
                cells.RemoveAt(removeIndex);
                return true;
            }

            return false;
        }

        public bool Remove(TKey key, out TValue value)
        {
            if (_dictionary.Remove(key, out value))
            {
                var removeIndex = cells.FindIndex(c => c.key.Equals(key));
                cells.RemoveAt(removeIndex);
                return true;
            }

            return false;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            if (_dictionary.ContainsKey(item.Key)) 
                return;
        
            _dictionary.Add(item.Key, item.Value);
            cells.Add(new DictionaryCell(item.Key, item.Value));
        }
    
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (_dictionary.Remove(item.Key))
            {
                cells.RemoveAt(cells.FindIndex(c => c.key.Equals(item.Key)));
            
                return true;
            }

            return false;
        }

        public void Clear()
        {
            _dictionary.Clear();
            cells.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item) 
            => _dictionary.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) 
            => throw new NotImplementedException();

        void ISerializationCallbackReceiver.OnBeforeSerialize() { }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _dictionary.Clear();
            foreach (var cell in cells)
                _dictionary.Add(cell.key, cell.value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() 
            => GetEnumerator();
        
        [Serializable]
        private struct DictionaryCell
        {
            public TKey key;
            public TValue value;

            public DictionaryCell(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}