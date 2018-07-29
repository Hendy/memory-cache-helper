# MemoryCacheHelper

    @using MemoryCacheHelper

	// Set a typed value in cache (any function currently setting this key is cancelled)
	MemoryCache.Instance.Set("myKey", myObject);

	// Get a typed value from cache
	var myObject = MemoryCache.Instance.Get<MyObject>("myKey"); 

	// Ensure result of a function is in cache and return it
	// This handles thread blocking (on a per key basis) such that only the first function is executed
	// If another thread directly sets this key, then this function is cancelled and the result returned from cache
	var myObject = MemoryCache.Instance.GetAdd<MyObject>("myKey", () => {
		// some (potentially) long running function
		return new MyObject();
	});

	// Check to see if a cache key is in use
	bool hasKey = MemoryCache.HasKey("myKey");

	// Remove a cache item
	MemoryCache.Instance.Remove("myKey");
	or
	MemoryCache.Instance.Set("myKey", null);
		
	// Use a function, Func<string, bool> to remove cache items
	// The function is evaluated against all keys and those returning true are removed
	MemoryCache.Instance.Remove(x => x.StartsWith("myKeyPrefix"));