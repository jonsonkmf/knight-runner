using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ScriptableSystem.GameCollection
{
    public abstract class GameCollection<TValue> : SerializedScriptableObject
    {
        [SerializeField] [ReadOnly] private List<TValue> _collection = new List<TValue>();
        private readonly BaseEvent _onChanged = new BaseEvent();

        public void Add(TValue value)
        {
            if (_collection.Contains(value)) return;
            _collection.Add(value);
            _onChanged.Invoke();
        }

        public void Remove(TValue value)
        {
            if (!_collection.Contains(value)) return;
            _collection.Remove(value);
            _onChanged.Invoke();
        }

        public IEventListener OnChanged => _onChanged;
        public List<TValue> Collection => _collection;
        
    }
}