# MemoryCacheHelper

The benefit of this library

    @using MemoryCacheHelper

	// set some typed value with (optional) timeout
	MemoryCache.Instance.Set("key", "value", 60);

	// get some typed value
	var value = MemoryCache.Instance.Get<string>("key"); 

	// cache some expensive function that returns MyObject with (optional) timeout
	// this is 'thread safe' in that cache locks (on per key basis) ensuring the expensive function only called once
	var myObject = MemoryCache.Instance.GetSet<MyObject>("myKey", () => {	  
	  return new MyObject();
	}, 60);

	// use a function to remove cache items
	MemoryCache.Instance.Remove(x => x.StartsWith("myKeyPrefix"));
