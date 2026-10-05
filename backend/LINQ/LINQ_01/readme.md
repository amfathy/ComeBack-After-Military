# C# Delegates: `delegate`, `Action`, `Func`, `Predicate`

## 1. What is a delegate?

A **delegate** is a type that holds a reference to a method. Think of it as a *type-safe function pointer*: you can store a method in a variable, pass it to another method, and call it later.

```csharp
// 1. Declare the delegate type (defines the method signature)
public delegate int MathOperation(int a, int b);

// 2. Methods that match the signature
static int Add(int a, int b) => a + b;
static int Multiply(int a, int b) => a * b;

// 3. Use it
MathOperation op = Add;
Console.WriteLine(op(2, 3));   // 5

op = Multiply;
Console.WriteLine(op(2, 3));   // 6
```

**Why use delegates?**
- Pass behavior as a parameter (callbacks, strategies)
- Events (`event EventHandler`)
- LINQ (`Where`, `Select`, etc. take delegates)
- Decouple "what to do" from "when to do it"

### Multicast delegates

A delegate can hold multiple methods; they run in order.

```csharp
public delegate void Notify(string message);

static void LogToConsole(string m) => Console.WriteLine($"Console: {m}");
static void LogToFile(string m)    => Console.WriteLine($"File: {m}");

Notify notify = LogToConsole;
notify += LogToFile;      // add
notify("Hello");          // calls both

notify -= LogToConsole;   // remove
notify("Again");          // calls only LogToFile
```

---

## 2. Built-in generic delegates

Declaring a custom delegate every time is tedious, so .NET provides three ready-made families:

| Delegate | Returns | Parameters | Typical use |
|---|---|---|---|
| `Action<...>` | `void` | 0 to 16 | Do something (side effects) |
| `Func<..., TResult>` | a value (**last type** is the return type) | 0 to 16 | Compute/transform something |
| `Predicate<T>` | `bool` | exactly 1 | Test a condition |

---

## 3. `Action`: returns nothing

Use when the method performs work but returns no value.

```csharp
// No parameters
Action sayHi = () => Console.WriteLine("Hi!");
sayHi();

// One parameter
Action<string> greet = name => Console.WriteLine($"Hello, {name}");
greet("Sara");

// Two parameters
Action<string, int> repeat = (text, times) =>
{
    for (int i = 0; i < times; i++)
        Console.WriteLine(text);
};
repeat("Go!", 3);
```

**Real use:** `List<T>.ForEach` takes an `Action<T>`.

```csharp
var names = new List<string> { "Ali", "Mona", "Omar" };
names.ForEach(n => Console.WriteLine(n));
```

---

## 4. `Func`: returns a value

The **last** generic type is always the return type.

```csharp
// Func<TResult>: no params, returns int
Func<int> getFive = () => 5;

// Func<T, TResult>: one param, returns result
Func<int, int> square = x => x * x;
Console.WriteLine(square(4));        // 16

// Func<T1, T2, TResult>
Func<int, int, int> add = (a, b) => a + b;
Console.WriteLine(add(2, 3));        // 5

// Different types
Func<string, int> length = s => s.Length;
Console.WriteLine(length("hello"));  // 5
```

**Real use:** LINQ `Select`, `OrderBy`, `Sum`, etc.

```csharp
var numbers = new[] { 1, 2, 3, 4 };
var squares = numbers.Select(x => x * x);   // Func<int,int>
// 1, 4, 9, 16
```

---

## 5. `Predicate<T>`: returns bool

Equivalent to `Func<T, bool>`, but named for intent: "does this item match?"

```csharp
Predicate<int> isEven = n => n % 2 == 0;
Console.WriteLine(isEven(4));   // True
Console.WriteLine(isEven(7));   // False
```

**Real use:** `List<T>.Find`, `FindAll`, `RemoveAll`, `Exists`.

```csharp
var nums = new List<int> { 1, 2, 3, 4, 5, 6 };

int firstEven       = nums.Find(isEven);       // 2
List<int> allEvens  = nums.FindAll(isEven);    // 2, 4, 6
bool hasEven        = nums.Exists(isEven);     // True
nums.RemoveAll(isEven);                        // nums = 1, 3, 5
```

> **Note:** LINQ methods like `Where` use `Func<T, bool>`, not `Predicate<T>`. The two are *not* interchangeable as types, even though the shape is the same.

---

## 6. Passing delegates as parameters

This is where delegates shine: the caller decides the behavior.

```csharp
static List<int> Filter(List<int> items, Predicate<int> condition)
{
    var result = new List<int>();
    foreach (var item in items)
        if (condition(item))
            result.Add(item);
    return result;
}

static List<int> Transform(List<int> items, Func<int, int> transformer)
    => items.Select(transformer).ToList();

static void Process(List<int> items, Action<int> action)
{
    foreach (var item in items) action(item);
}

var data = new List<int> { 1, 2, 3, 4, 5 };

var big      = Filter(data, x => x > 3);        // 4, 5
var doubled  = Transform(data, x => x * 2);     // 2, 4, 6, 8, 10
Process(data, x => Console.WriteLine(x));       // prints each
```

---

## 7. Named methods vs. lambdas

All three can take either a named method or a lambda.

```csharp
static bool IsPositive(int n) => n > 0;

Predicate<int> p1 = IsPositive;        // method group
Predicate<int> p2 = n => n > 0;        // lambda
Predicate<int> p3 = delegate (int n) { return n > 0; };  // anonymous method (older style)
```

---

## 8. Quick comparison

```csharp
Action<int>        print   = x => Console.WriteLine(x);   // int -> void
Func<int, int>     doubler = x => x * 2;                  // int -> int
Predicate<int>     isBig   = x => x > 100;                // int -> bool
Func<int, bool>    isBig2  = x => x > 100;                // same shape as Predicate
```

| Question | Use |
|---|---|
| Do I need to return something? | No → `Action`, Yes → `Func` |
| Is the return always `bool` with one input? | `Predicate<T>` (or `Func<T,bool>` for LINQ) |
| Need a custom name or `ref`/`out` params? | Declare your own `delegate` |

---

## 9. Full runnable example

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        var people = new List<(string Name, int Age)>
        {
            ("Ali", 17), ("Mona", 25), ("Omar", 40), ("Sara", 15)
        };

        // Predicate: who is an adult?
        Predicate<(string Name, int Age)> isAdult = p => p.Age >= 18;
        var adults = people.FindAll(isAdult);

        // Func: build a display string
        Func<(string Name, int Age), string> describe =
            p => $"{p.Name} ({p.Age})";

        // Action: print it
        Action<string> print = Console.WriteLine;

        adults.Select(describe).ToList().ForEach(print);
        // Mona (25)
        // Omar (40)
    }
}
```

---

## 10. Summary

- **Delegate**: a type representing a method signature; holds references to methods.
- **`Action`**: no return value.
- **`Func`**: returns a value (last type parameter).
- **`Predicate<T>`**: takes one `T`, returns `bool`.
- Prefer `Action`/`Func`/`Predicate` over custom delegates unless you need a specific name or `ref`/`out`.
