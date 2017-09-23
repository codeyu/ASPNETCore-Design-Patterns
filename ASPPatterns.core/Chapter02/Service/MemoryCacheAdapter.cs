using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace Service
{
    public class MemoryCacheAdapter : ICacheStorage
    {  
        private static readonly MemoryCache CacheStore = new MemoryCache(new MemoryCacheOptions());      
        public void Remove(string key)
        {
            CacheStore.Remove(key);   
        }

        public void Store(string key, object data)
        {
            CacheStore.Set(key, data);    
        }

        public T Retrieve<T>(string key)
        {
            T itemStored = (T)CacheStore.Get(key);
            if (itemStored == null)
                itemStored = default(T);

            return itemStored;       
        }       
    }
}
