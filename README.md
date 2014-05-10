Unity-Parse-Helpers
===================

This library provides type-safe extension methods for using Parse.com

If you like using Parse.com in Unity but hate the fact you have to make non-type-safe calls such as:

```
new ParseQuery<Armor>().WhereLessThanOrEqualTo("cost", 13);
```

Then disappoint no more, using this library the above becomes:

```
new ParseQuery<Armor>().WhereLessThanOrEqualTo(a => a.Cost, 13);
```

The library can also handle chains such as

```
new ParseQuery<Player>().Include(p => p.Stats.Heath.Remaining); // becomes "stats.health.remaining"
```

The library also handles interfaces by introducing a new attribute "ParseFieldType".

```
[ParseClassName("Father")]
public class Father : ParseObject, IFather
{
	[ParseFieldName("daughter")]
	[ParseFieldType(typeof(Child))]
	public IChild Daughter { get; set; }
}

new ParseQuery<Father>().Include(f => f.Daughter.Name); // becomes "daughter.name" and works because ParseFieldType redirects the chain to Child rather than IChild
```

It can even handle lists

```
[ParseClassName("Father")]
public class Father : ParseObject, IFather
{
	[ParseFieldName("children")]
	[ParseFieldType(typeof(Child))]
	public List<IChild> Children { get; set; }
}

new ParseQuery<Father>().Include(f => f.Children[0].Name); // becomes "children.name"
```

Installation
------------

Download the DLL (https://github.com/mikecann/Unity-Parse-Helpers/releases) and include in your unity project. 

Make sure you also have the Parse.com SDK in your project.

Then simply add:

```
using UnityParseHelpers;
```

