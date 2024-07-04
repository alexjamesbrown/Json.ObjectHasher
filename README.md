# JSON Object Hasher

This takes an object, or objects, and generates an md5 hash.

Since these aren't meant to be 'secure' hashes, md5 is fine.
(I may enable algorithm specification in the future)

## Usage

```
var hasher = new ObjectHasher();

var obj = new MyThing() { Name = "Alex" };

var hash = hasher.GenerateHash(obj);
```


