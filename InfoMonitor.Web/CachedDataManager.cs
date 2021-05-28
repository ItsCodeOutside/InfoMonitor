using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoMonitor.Web
{
    public class CachedDataManager
    {
        IMemoryCache _cache;
        Dictionary<string, CachedItem> _data = new Dictionary<string, CachedItem>();

        public CachedDataManager(IMemoryCache cache)
        {
            _cache = cache;
            _cache.Set("data", this);
        }

        public void Set(string sourceId, object data)
        {
            if (!_data.ContainsKey(sourceId))
            {
                _data.Add(sourceId, new CachedItem());
            }
            _data[sourceId].Data = data;
        }
        public List<object> GetAll()
        {
            var result = new List<object>(_data.Count);
            foreach (var key in _data.Keys)
            {
                result.Add(_data[key].Data);
            }
            return result;
        }
        public object GetOne(string sourceId)
        {
            object result = null;
            if (_data.ContainsKey(sourceId))
            {
                result = _data[sourceId];
            }
            return result;
        }
        public void ClearExpiredSources(TimeSpan timeout)
        {
            var toRemove = new List<string>();
            foreach (var key in _data.Keys)
            {
                if (DateTime.Now.Subtract(timeout).CompareTo(_data[key].LastUpdate) >= 0)
                {
                    toRemove.Add(key);
                }
            }
            foreach(var key in toRemove)
            {
                _data.Remove(key);
            }
        }
    }
}
