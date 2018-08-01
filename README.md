# MemoryCacheHelper

A singleton wrapper around an System.Runtime.Caching.MemoryCache instance, providing thread-safe helper methods.


	namespace MemoryCacheHelper
	{
		public sealed class MemoryCache
		{	
			public static MemoryCache Instance { get; } // Singleton instance			
			
			public string Name { get; } // unique name of wrapped memory cache

			TODO: public CacheItemPolicy DefaultCacheItemPolicy { set; }

			public void Set(string key, object value, CacheItemPolicy policy = null) { ... }

			TODO: public void Set(string key, Func<object> valueFunction, CacheItemPolicy policy = null) { ... }

			public T Get<T>(string key) { }

			public T Get<T>(string key, bool found) { }

			public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null) { }

			public bool HasKey(string key) { }

			TODO: public IEnumerable<string> GetKeys() { }

			public bool IsEmpty() { }

			TODO: public long Trim(int percent)
		}
	}
