# MemoryCacheHelper

A singleton wrapper around an System.Runtime.Caching.MemoryCache instance, providing thread-safe helper methods.

	// Properties

	public static MemoryCache Instance { get; } // the singleton instance

	public string Name { get; } // unique name of wrapped memory cache

	public CacheItemPolicy DefaultCacheItemPolicy { set; } // set an optional default

	// Methods

	public void Add(string key, Func<T> valueFunction, CacheItemPolicy policy = null) {}

	public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null) {}

	public bool Contains(string key) {} // wrapper

	public T Get<T>(string key) {}

	public T Get<T>(string key, out bool found) {}

	public object Get(string key) {} // wrapper

	internal long GetApproximateSize() {}

	public IEnumerable<string> GetKeys() {}

	public T GetSet<T>(string Key, Func<T> valueFunction, CacheItemPolicy = null) {} // same as AddOrGetExisting

	public bool HasKey(string key) {}

	public bool IsEmpty() {}

	internal bool IsSetting() {}

	internal bool IsWiping() {}

	public void Remove(Func<string, bool> keyFunction) {}

	public void Remove(string key) {}

	public void Set(string key, Func<object> valueFunction, CacheItemPolicy policy = null) {}

	public void Set(string key, object value, CacheItemPolicy policy = null) {}

	public long Trim(int percent) {} // wrapper

	public void Wipe() {}
