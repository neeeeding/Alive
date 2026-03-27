using System;
using System.Collections.Generic;
using UnityEngine;


namespace _02Script.Etc
{
//저장 가능 딕셔너리
    [Serializable]
    public class SaveDictionary<K, V> : ISerializationCallbackReceiver
    {
        [SerializeField] private List<K> ks = new List<K>(); //키들
        [SerializeField] private List<V> vs = new List<V>(); //값들

        private Dictionary<K, V> _dictionary = new Dictionary<K, V>(); //사용될 딕셔너리

        public void Add(K key, V value) //값 추가
        {
            _dictionary.Add(key, value);
            ks.Add(key);
            vs.Add(value);
        }
        public void Remove(K key)
        {
            vs.Remove(_dictionary[key]);
            ks.Remove(key);
            _dictionary.Remove(key);
        }

        public void Clear() //지우기
        {
            _dictionary.Clear();
            ks.Clear();
            vs.Clear();
        }
        
        public void Dictionary(Dictionary<K, V> dictionary)
        {
            _dictionary = dictionary;
        }
        
        public Dictionary<K, V> ToDictionary()
        {
            if (_dictionary.Count == 0 && ks.Count > 0)
            {
                OnAfterDeserialize();
            }
            return new Dictionary<K, V>(_dictionary);
        }

        public void FromDictionary(Dictionary<K, V> source) //로드할 때 사용
        {
            _dictionary = new Dictionary<K, V>(source);
            ks.Clear();
            vs.Clear();
            foreach (var kv in source)
            {
                ks.Add(kv.Key);
                vs.Add(kv.Value);
            }
        }
        public void OnBeforeSerialize() //직렬화
        {
            ks.Clear(); //값 초기화
            vs.Clear();
            foreach (var kv in _dictionary)
            {
                ks.Add(kv.Key);
                vs.Add(kv.Value);
            }
        }

        public void OnAfterDeserialize() //역직렬화
        {
            _dictionary = new Dictionary<K, V>();
            for (int i = 0; i < ks.Count && i < vs.Count; i++)
            {
                _dictionary[ks[i]] = vs[i];
            }
        }

        public V this[K key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }
    }
}