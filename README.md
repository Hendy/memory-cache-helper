# MemoryCacheHelper

    @using MemoryCacheHelper

	// cache some expensive function that returns MyObject
	var myObject = MemoryCache.Instance.GetSet<MyObject>("key", () => {	  
	  return new MyObject();
	});

	// use a lambda to remove cache items
	MemoryCache.Instance.Remove(x => x.StartsWith("myKeyPrefix"));