﻿using System.Collections;
using System.Collections.Generic;

namespace DidacticalEnigma.Core.Utils
{
    class DualDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        private Dictionary<TKey, TValue> keyToValue = new Dictionary<TKey, TValue>();
        private Dictionary<TValue, TKey> valueToKey = new Dictionary<TValue, TKey>();

        TValue IReadOnlyDictionary<TKey, TValue>.this[TKey key] => keyToValue[key];

        public DualDictionary(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            foreach(var kvp in source)
            {
                keyToValue.Add(kvp.Key, kvp.Value);
                valueToKey.Add(kvp.Value, kvp.Key);
            }
        }

        public struct KeyProxy
        {
            internal readonly DualDictionary<TKey, TValue> dict;

            internal KeyProxy(DualDictionary<TKey, TValue> dict)
            {
                this.dict = dict;
            }

            public TValue this[TKey value] => dict.keyToValue[value];
        }

        public struct ValueProxy
        {
            internal readonly DualDictionary<TKey, TValue> dict;

            internal ValueProxy(DualDictionary<TKey, TValue> dict)
            {
                this.dict = dict;
            }

            public TKey this[TValue value] => dict.valueToKey[value];
        }

        public ValueProxy Value => new ValueProxy(this);

        public KeyProxy Key => new KeyProxy(this);

        public IEnumerable<TKey> Keys => keyToValue.Keys;

        public IEnumerable<TValue> Values => valueToKey.Keys;

        public int Count => keyToValue.Count;

        public bool ContainsKey(TKey key)
        {
            return keyToValue.ContainsKey(key);
        }

        public bool ContainsValue(TValue value)
        {
            return valueToKey.ContainsKey(value);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return keyToValue.GetEnumerator();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return keyToValue.TryGetValue(key, out value);
        }

        public bool TryGetKey(TValue value, out TKey key)
        {
            return valueToKey.TryGetValue(value, out key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
