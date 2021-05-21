using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
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

            List<SourceItem> sources = _cache.Get("sources") as List<SourceItem>;
            if (sources == null)
            {
                sources = new List<SourceItem>();
            }


            //Overwrite anything that currently exists for this sourceId
            sources.Add(new SourceItem() { SourceId = sourceId });
            _cache.Set("sources", sources);
            _cache.Set(sourceId, data);
        }

        [HttpGet]
        public object Get()
        {
            List<SourceItem> sources = _cache.Get("sources") as List<SourceItem>;
            List<object> dataList = new List<object>();
            if (sources != null)
            {
                List<SourceItem> expiredSources = new List<SourceItem>();
                foreach (var sourceItem in sources)
                {
                    var result = _cache.Get(sourceItem.SourceId);
                    if (result != null)
                    {
                        //Make a list of anything that has 'expired'
                        if (DateTime.Now.Subtract(sourceItem.LastUpdate).TotalMinutes > 5)
                        {
                            expiredSources.Add(sourceItem);
                        }
                        else
                            dataList.Add(result);
                    }
                }
                //Remove the expired sources, if there are any
                if (expiredSources.Count > 0)
                {
                    foreach (var sourceItem in expiredSources)
                    { 
                        sources.Remove(sourceItem);
                        _cache.Remove(sourceItem.SourceId);
                    }
                    //Possible a just-added item could be hidden because of this
                    _cache.Set("sources", sources);
                }
            }
            return dataList;
        }
    }
}
