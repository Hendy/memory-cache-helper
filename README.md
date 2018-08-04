# MemoryCacheHelper

A singleton wrapper around an System.Runtime.Caching.MemoryCache instance, providing thread-safe helper methods.

	// The singleton instance
	public static MemoryCache Instance { get; }

	// Unique name of wrapped memory cache
	public string Name { get; }

	// Optional default cache item policy to use if one is not provided with each call
	public CacheItemPolicy DefaultCacheItemPolicy { set; }


	public void Add(string key, Func<object> valueFunction, CacheItemPolicy policy = null) {}

	TODO: public void Add(string, object value, CacheItemPolicy = null) {}

	// Alias to GetSet<T>(key, valueFunction, policy)
	public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null) {}

	TODO: public T AddOrGetExisting<T>(string key, object value, CacheItemPolicy policy = null) {}

	// Wrapper
	public bool Contains(string key) {}

	public T Get<T>(string key) {}

	public T Get<T>(string key, out bool found) {}

	// Wrapper
	public object Get(string key) {}

	// Attempts to return the approximate size, otherwize returns -1 for unknown
	internal long GetApproximateSize() {}

	public IEnumerable<string> GetKeys() {}

	public T GetSet<T>(string Key, Func<T> valueFunction, CacheItemPolicy = null) {}

	TODO: public T GetSet<T>(seting key, object value, CacheItemPolicy = null) {}

	public bool HasKey(string key) {}

	public bool IsEmpty() {}

	internal bool IsSetting() {}

	internal bool IsWiping() {}

	// Remove cache entries where the function return true for the key
	public void Remove(Func<string, bool> keyFunction) {}

	public void Remove(string key) {}

	public void Set(string key, Func<object> valueFunction, CacheItemPolicy policy = null) {}

	public void Set(string key, object value, CacheItemPolicy policy = null) {}

	// Wrapper - Removes a specified percentage of cache entries from the cache
	public long Trim(int percent) {}

	// Remove all cache entries, blocking any set operations until the wipe is complete
	public void Wipe() {}
