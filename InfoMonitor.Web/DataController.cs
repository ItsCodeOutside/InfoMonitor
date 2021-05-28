using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace InfoMonitor.Web
{
    [Produces("application/json")]
    public class DataController : ControllerBase
    {
        CachedDataManager _cachedData;
        TimeSpan _cacheExpirationSeconds = new TimeSpan(0, 10, 0);

        public DataController(IMemoryCache cache, IConfiguration config)
        {
            //Monitored data is stored only in memory. No persistent storage.
            if (!cache.TryGetValue("data", out CachedDataManager cachedData))
            {
                cachedData = new CachedDataManager(cache);
            }
            _cachedData = cachedData;
            _cacheExpirationSeconds = new TimeSpan(0, 0, config.GetValue<int>("AppSettings:CacheExpirationSeconds"));
        }

        /// <summary>
        /// Accepts and stores whatever data is given
        /// </summary>
        /// <param name="sourceId">ID supplied by sender, eg:/ Machine name</param>
        /// <param name="data">JSON of data that is being monitored</param>
        [HttpPost]
        public void Set(string sourceId, [FromBody] object data)
        {
            _cachedData.Set(sourceId, data);
        }

        [HttpGet]
        public object GetAll()
        {
            _cachedData.ClearExpiredSources(_cacheExpirationSeconds);
            return _cachedData.GetAll();
        }
    }
}
