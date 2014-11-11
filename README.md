Unity-Parse-Helpers
===================

This library provides a number of helpers and utilities for dealing with Parse.com in Unity 3D

Type-Safe Queries
-----------------

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

Task Chaining
--------------

A series of extension methods have been provided that let you chain together calls in a Javascript-Promise like manner:

```
 // Signup
userService.Signup(usernameInp.text, passwordInp.text, playernameInp.text)

	// Then Login
	.Then(t => userService.Login(usernameInp.text, passwordInp.text))

	// Then we are done
	.Then(OnLoggedIn, OnError);             
```

Looming
-------

Using the extension ".OnMainThread()" for Task you can ensure that your Parse async calls always run in the main thread:

```
public Task<GameUser> Login(string email, string password)
{
	Debug.Log("Logging in..");

	return ParseUser.LogInAsync(email, password)
		.OnMainThread()
		.Then(t => Task.FromResult((GameUser)t.Result));
}
```

Installation
------------

Checkout this project as a sub-module in your project, and you should be good to go!
