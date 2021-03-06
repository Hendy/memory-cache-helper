# MemoryCacheHelper

A singleton wrapper around System.Runtime.Caching.MemoryCache, providing thread-safe helper methods.

## Example

```csharp
@using MemoryCacheHelper

// a get or lazy set
var myObject = SharedMemoryCache.Instance.GetSet<MyObject>("key", () => { 

	// only the first thread on this key this will be executed
	// other GetSet threads to this key will be blocked until this is complete or aborted
	// this may be aborted if a direct set occurs on this this key before this is complete

	return new MyObject();

});

// a lazy set
SharedMemoryCache.Instance.Set<MyObject>("key", () => { 

	// the most recent thread on this key will abort any previous ones
	// this may be aborted if a direct set occurs on this this key before this is complete

	return new MyOjbect();

});

// a direct set
SharedMemoryCache.Instance.Set<MyObject>("key", new MyObject());

```

## API

```csharp
// The singleton instance
public static SharedMemoryCache Instance { get; }

// Unique name of wrapped memory cache
public string Name { get; }

// Set an optional default policy to use if one is not provided with each call
public CacheItemPolicy DefaultPolicy { set; }

// If key not found, sets a cache item by key, function and optional eviction
// Will be blocked if another valueFunction is already opertating on this key
public void Add(string key, Func<object> valueFunction, CacheItemPolicy policy = null) {}

// If key not found, sets a cache item by key, value and optional eviction
public void Add(string, object value, CacheItemPolicy = null) {}

// If key not found, sets a cache item by key, function and optional eviction
// Will be blocked if another valueFunction is already opertating on this key
public T AddOrGetExisting<T>(string key, Func<T> valueFunction, CacheItemPolicy policy = null) {}

// If key not found, sets a cache item by key, value and optional eviction
public T AddOrGetExisting<T>(string key, object value, CacheItemPolicy policy = null) {}

// Wrapper
public bool Contains(string key) {}

// Remove all cache keys, but without blocking any set operations 
public void Flush() {}

// Get the cache value as type T, else default(T)
public T Get<T>(string key) {}

// Found is true when the key exists and its value is of type T
public T Get<T>(string key, out bool found) {}

//  Wrapper
public object Get(string key) {}

// Attempts to return the approximate size via reflection, otherwize returns -1 for unknown
internal long GetApproximateSize() {}

public IEnumerable<string> GetKeys() {}

// Attempts to get cache value of type T, otherwise sets a cache item by key, function and optional eviction
// Will be blocked if another valueFunction is already opertating on this key
public T GetSet<T>(string Key, Func<T> valueFunction, CacheItemPolicy = null) {}

// Attempts to get cache value of type T, sets a cache item by key, value and optional eviction
public T GetSet<T>(seting key, object value, CacheItemPolicy = null) {}

// Check to see if the cache has the key
public bool HasKey(string key) {}

// Check to see if the memory cache is empty of all keys
public bool IsEmpty() {}

// State check to see if the wipe action is currently being executed
internal bool IsWiping() {}

// Remove cache entries where the function return true for the key
public void Remove(Func<string, bool> keyFunction) {}

// Remove specific cache item
public void Remove(string key) {}

// Set a cache item by key, function and optional eviction

public void Set(string key, Func<object> valueFunction, CacheItemPolicy policy = null) {}

// Set a cache item by key, value and optional eviction
public void Set(string key, object value, CacheItemPolicy policy = null) {}

// Wrapper - Removes a specified percentage of cache entries from the cache
public long Trim(int percent) {}

// Remove all cache entries, blocking any set operations until complete
public void Wipe() {}
```
