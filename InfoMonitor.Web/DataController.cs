using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace InfoMonitor.Web
{
    [Produces("application/json")]
    public class DataController : ControllerBase
    {
        IMemoryCache _cache;

        public DataController(IMemoryCache cache)
        {
            //Monitored data is stored only in memory. No persistent storage.
            _cache = cache;
        }

        /// <summary>
        /// Accepts and stores whatever data is given
        /// </summary>
        /// <param name="sourceId">ID supplied by sender, eg:/ Machine name</param>
        /// <param name="data">JSON of data that is being monitored</param>
        [HttpPost]
        public void Set(string sourceId, [FromBody] object data)
        {
            //ignore empties
            if (string.IsNullOrEmpty(sourceId) || data == null)
                return;

            List<string> sources = _cache.Get("sources") as List<string>;
            if (sources == null)
            {
                sources = new List<string>();
                sources.Add(sourceId);
                _cache.Set("sources", sources);
            }
            //Overwrite anything that currently exists for this sourceId
            _cache.Set(sourceId, data);
        }

        [HttpGet]
        public object Get()
        {
            List<string> sources = _cache.Get("sources") as List<string>;
            List<object> dataList = new List<object>();
            if (sources != null)
            {
                foreach (var sourceId in sources)
                {
                    var result = _cache.Get(sourceId);
                    if (result != null)
                        dataList.Add(result);
                }
            }
            return dataList;
        }
    }
}
