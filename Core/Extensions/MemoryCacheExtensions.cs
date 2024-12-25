using Microsoft.Extensions.Caching.Memory;

namespace Core.Extensions
{
    public static class MemoryCacheExtensions
    {
        public static List<string> GetKeys(this IMemoryCache memoryCache)
        {
            var cacheEntries = new List<string>();

            // Cache içeriğini almak için `private` üyeleri yasal bir yolla keşfedin
            if (memoryCache is MemoryCache memCache)
            {
                var coherentState = typeof(MemoryCache)
                    .GetProperty("EntriesCollection", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
                    .GetValue(memCache) as dynamic;

                if (coherentState != null)
                {
                    foreach (var entry in coherentState)
                    {
                        cacheEntries.Add(entry.Key.ToString());
                    }
                }
            }

            return cacheEntries;
        }
    }

}
