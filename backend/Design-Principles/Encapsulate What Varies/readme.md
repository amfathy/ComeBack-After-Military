# Encapsulate What Varies

> Identify the part of your code that is likely to change, and separate it from the code that is stable.

- **Things that change** → put them behind an abstraction
- **Things that don't change** → keep them independent

## The Idea

When a piece of logic is likely to grow, shrink, or change shape over time (like the rules for deciding *which* pizza to create), it shouldn't be mixed together with the logic that never changes (like the steps to *prepare, cook, and cut* a pizza).

By pulling the volatile part into its own method (or class), we get two benefits:

1. **Maintainability** — when the changing part needs an update, we only touch that one isolated piece, not the whole workflow.
2. **Readability** — the stable method now reads like a clean, high-level recipe, free of clutter.

---

## The Code Before

The pizza-selection logic (the part that changes whenever we add/remove pizza types) is tangled directly inside the `Order` method, which is supposed to represent a **stable** workflow (prepare → cook → cut).

```csharp
using System;
using System.Threading;

namespace DesignPrinciples.EncapsulateWhatVarient
{
    class Pizza
    {
        public virtual string Title => $"{nameof(Pizza)}";
        public virtual decimal Price => 10m;

        public static Pizza Order(string type)
        {

        ////////////////////////////////////////////////////////////////////////
            Pizza pizza;
            if (type.Equals(PizzaConstants.CheesePizza))
                pizza = new Cheese();
            else if (type.Equals(PizzaConstants.VegeterianPizza))
                pizza = new Vegeterian();
            else
                pizza = new Chicken();
        /////////////////////////////////////////////////////////////////////////
        
            Prepare();
            Cook();
            Cut();

            return pizza;
        }

        private static void Prepare()
        {
            Console.Write("preparing...");
            Thread.Sleep(500);
            Console.WriteLine(" completed");
        }

        private static void Cook()
        {
            Console.Write("cooking...");
            Thread.Sleep(500);
            Console.WriteLine(" completed");
        }

        private static void Cut()
        {
            Console.Write("cutting and boxing...");
            Thread.Sleep(500);
            Console.WriteLine(" completed");
        }

        public override string ToString()
        {
            return $"\n{Title}" +
                   $"\n\tPrice: {Price.ToString("C")}";
        }
    }

    class Cheese : Pizza
    {
        public override string Title => $"{base.Title} {nameof(Cheese)}";
        public override decimal Price => base.Price + 3m;
    }

    class Chicken : Pizza
    {
        public override string Title => $"{base.Title} {nameof(Chicken)}";
        public override decimal Price => base.Price + 6m;
    }

    class Vegeterian : Pizza
    {
        public override string Title => $"{base.Title} {nameof(Vegeterian)}";
        public override decimal Price => base.Price + 4m;
    }

    internal class PizzaConstants
    {
        public const string CheesePizza = "cheese";
        public const string VegeterianPizza = "veggie";
        public const string ChickenPizza = "chicken";
    }

    class Program
    {
        static void Main(string[] args)
        {
            Pizza pizza = Pizza.Order(PizzaConstants.VegeterianPizza);
            Console.WriteLine(pizza);
            Console.ReadKey();
        }
    }
}
```

### The Problem

The `Order` method mixes two very different kinds of code:

| Code | Nature |
|---|---|
| `if / else if / else` block that picks a pizza type | **Volatile** — changes every time a new pizza type is added |
| `Prepare()`, `Cook()`, `Cut()` sequence | **Stable** — the workflow steps rarely change |

Because they live in the same method, any change to the pizza-selection rules risks touching (and possibly breaking) the stable workflow around it.

---

## The Code After

The volatile selection logic is extracted into its own private method, `Create`. The `Order` method now only orchestrates stable, high-level steps — it delegates the "what varies" part instead of containing it.

```csharp
using System;
using System.Threading;

namespace DesignPrinciples.EncapsulateWhatVarient
{
    class Pizza
    {
        public virtual string Title => $"{nameof(Pizza)}";
        public virtual decimal Price => 10m;
/////////////////volatile section///////////////////////////////
        private static Pizza Create(string type)
        {
            Pizza pizza;
            if (type.Equals(PizzaConstants.CheesePizza))
                pizza = new Cheese();
            else if (type.Equals(PizzaConstants.VegeterianPizza))
                pizza = new Vegeterian();
            else
                pizza = new Chicken();
            return pizza;
        }
/////////////////volatile section///////////////////////////////


////////////////Stable section//////////////////////////////////
        public static Pizza Order(string type)
        {
            Pizza pizza = Create(type);

            Prepare();
            Cook();
            Cut();

            return pizza;
        }
////////////////Stable section//////////////////////////////////
//
//          the End of the code still same
//        
       
```

---

## What Changed & Why It Matters

- The `if / else if / else` pizza-selection block moved into a dedicated `Create(string type)` method.
- `Order(string type)` now simply calls `Create(type)` and runs the fixed workflow: `Prepare → Cook → Cut`.
- `Order` no longer needs to know **how** a pizza is chosen — only **that** a pizza is produced.

### Benefits

- ✅ **Isolation** — adding a new pizza type (or changing the selection rule entirely, e.g. replacing the `if/else` chain with a dictionary or a factory) only requires editing `Create`.
- ✅ **Stability preserved** — `Order`, `Prepare`, `Cook`, and `Cut` are never touched again for this kind of change.
- ✅ **Readability** — `Order` now reads as a clear, linear recipe:
  ```csharp
  Pizza pizza = Create(type);
  Prepare();
  Cook();
  Cut();
  ```
- ✅ **Lower risk of bugs** — stable, well-tested code paths aren't disturbed by changes to volatile logic.

---

## Key Takeaway

> Don't let *what changes* and *what stays the same* live in the same place.
> Extract the changing part into its own method/class, and let the stable code simply **use** it — without knowing (or caring) about its internal details.
