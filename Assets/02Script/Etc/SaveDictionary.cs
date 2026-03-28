using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using Newtonsoft.Json;

namespace _02Script.Etc
{
    [Serializable]
    public class SaveDictionary<K, V> : ISerializationCallbackReceiver
    {
        [SerializeField, JsonProperty("ks")] 
        public List<K> ks = new List<K>();
        [SerializeField, JsonProperty("vs")] 
        public List<V> vs = new List<V>();
        [JsonIgnore]
        private Dictionary<K, V> _dictionary = new Dictionary<K, V>();
        [JsonIgnore]
        public Dictionary<K, V> Dict => _dictionary;

        public bool ContainsKey(K key) => _dictionary.ContainsKey(key);

        public Dictionary<K,V> ToDictionary() => _dictionary;

        public void Add(K key, V value)
        {
            if (_dictionary.ContainsKey(key)) _dictionary[key] = value;
            else _dictionary.Add(key, value);
        }
        
        public void Remove(K key)
        {
            vs.Remove(_dictionary[key]);
            ks.Remove(key);
            _dictionary.Remove(key);
        }
        
        public void Clear()
        {
            _dictionary.Clear();
            ks.Clear();
            vs.Clear();
        }

        public void OnBeforeSerialize() //직렬화 전
        {
            if (_dictionary != null && _dictionary.Count > 0)
                SyncListFromDict();
        }

        public void OnAfterDeserialize() //역 직렬화 후
        {
            SyncDictFromList();
        }
        
        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context) //객체 생성 후 자동 실행
        {
            SyncDictFromList();
        }
    
        [OnSerializing]
        internal void OnSerializingMethod(StreamingContext context)
        {
            SyncListFromDict();
        }

        public void SyncListFromDict()
        {
            ks.Clear();
            vs.Clear();
            foreach (var kvp in _dictionary)
            {
                ks.Add(kvp.Key);
                vs.Add(kvp.Value);
            }
        }
        public void SyncDictFromList()
        {
            if (ks == null || vs == null || ks.Count == 0) return;
    
            _dictionary ??= new Dictionary<K, V>();
            _dictionary.Clear();
            for (int i = 0; i < ks.Count && i < vs.Count; i++)
            {
                _dictionary[ks[i]] = vs[i];
            }
        }

        [JsonIgnore]
        public V this[K key]
        {
            get => _dictionary[key];
            set => _dictionary[key] = value;
        }
    }
    
    [Serializable]
    public struct SaveVector2
    {
        public float x;
        public float y;

        public SaveVector2(Vector2 v) { x = v.x; y = v.y; }
        public Vector2 ToVector2() => new Vector2(x, y);
    
        public static implicit operator Vector2(SaveVector2 v) => new Vector2(v.x, v.y);
        public static implicit operator SaveVector2(Vector2 v) => new SaveVector2(v);
    }
}