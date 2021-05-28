using System;

namespace InfoMonitor.Web
{
    public class CachedItem
    {
        object _data;
        public DateTime LastUpdate { get; private set; }
        public object Data
        {
            get { return _data; }
            set
            {
                LastUpdate = DateTime.Now;
                _data = value;
            }
        }
    }
}
