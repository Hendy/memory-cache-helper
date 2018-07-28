# MemoryCacheHelper

This library exposes helper methods on a memory cache via a singleton, notably the .GetSet&lt;T&gt;(string key, Func&lt;T&gt; expensiveFunction) which is thread safe in that it locks (on a per key basis) ensuring any expensive functions to populate a cache key would only executed once.

	@using MemoryCacheHelper

	// set some typed value with (optional) timeout in seconds
	MemoryCache.Instance.Set("key", "value", 60);

	// get some typed value
	var value = MemoryCache.Instance.Get<string>("key"); 

	// cache some expensive function that returns MyObject with (optional) timeout
	var myObject = MemoryCache.Instance.GetSet<MyObject>("myKey", () => {	  
	  return new MyObject();
	}, 60);

	// use a function to remove cache items
	MemoryCache.Instance.Remove(x => x.StartsWith("myKeyPrefix"));
