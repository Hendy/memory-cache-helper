# MemoryCacheHelper

A singleton wrapper around an System.Runtime.Caching.MemoryCache instance, providing thread-safe helper methods.


	namespace MemoryCacheHelper
	{
		public sealed class MemoryCache
		{	
			public static MemoryCache Instance { get; } // singleton instance			
			
			public string Name { get; } // unique name of wrapped memory cache

			public CacheItemPolicy DefaultCacheItemPolicy { set; } // set an optional default

			public void Set(string key, object value, CacheItemPolicy policy = null) { }

			TODO: public void Set(string key, Func<object> valueFunction, CacheItemPolicy policy = null) { }

			public T Get<T>(string key) { }

			public T Get<T>(string key, bool found) { }

			public object Get(string key) { } // wrapper

			pulbic void Add(string key, Func<T> valueFunction, CacheItemPolicy policy = null) { }

			public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null) { }

			TODO: public T GetSet<T>(string key, Funct<T> valueFunction, CacheItemPolicy policy = null) { } // alias for AddOrGetExisting

			public bool Contains(string key) { } // wrapper

			public bool HasKey(string key) { } // alias for Contains

			public IEnumerable<string> GetKeys() { }

			public bool IsEmpty() { }

			public long Trim(int percent) { } // wrapper

			public void Wipe() { }
		}
	}
