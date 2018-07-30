# MemoryCacheHelper

A singleton wrapper around an System.Runtime.Caching.MemoryCache instance, providing thread-safe helper methods.

    @using MemoryCacheHelper

	// Set a typed value
	MemoryCache.Instance.Set("myKey", myObject);

	// Get a typed value
	var myObject = MemoryCache.Instance.Get<MyObject>("myKey"); 

	// Get or set if empty, a typed value by function
	var myObject = MemoryCache.Instance.AddOrGetExisting<MyObject>("myKey", () => {		
		// can be a long running function here - will be terminated if a direct Set occurs on this key from elsewhere
		return new MyObject();
	});

	// Does cache item exist
	bool hasKey = MemoryCache.HasKey("myKey");

	// Remove a cache item
	MemoryCache.Instance.Remove("myKey");
		
	// Remove cache items by function
	MemoryCache.Instance.Remove(x => x.StartsWith("myKeyPrefix"));