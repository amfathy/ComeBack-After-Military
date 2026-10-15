# LINQ Session 12: Tasks (`SequenceEqual` and Equality)

## How to use

1. Create a new Console project (.NET 5 or later).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helpers, so **no shared project is needed**.
3. Solve each task in its `// TODO` area and print a clear sentence such as `"quiz1 and quiz2 are equal: True"`.
4. In **Part 3** you will **edit the `Choice` and `Question` classes** in the starter code (look for the `// TODO` comments). The earlier parts must be run **before** you add the overrides, so keep the results of Part 2 in mind (or comment the overrides out to re-run them).

## The data

- **`Question` / `Choice` classes** with no equality code (on purpose).
- **`QuestionBank`** with 6 questions, in two flavors:
  - `QuestionBank.All`: one shared list. Reading it twice gives the **same objects**.
  - `QuestionBank.LoadQuestions()`: builds **new objects every call**. Two calls give two lists with the **same data** but **different objects**.
- **`Person`** (a `record` with simple properties) and **`PersonWithHobbies`** (a `record` that has a list).

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession12Tasks
{
    // ---------- Models ----------
    public class Choice
    {
        public int Order { get; set; }
        public string Description { get; set; }

        // TODO (Part 3): override Equals and GetHashCode here
    }

    public class Question
    {
        public string Title { get; set; }
        public List<Choice> Choices { get; set; } = new();
        public int CorrectAnswer { get; set; }

        // TODO (Part 3): override Equals and GetHashCode here

        public override string ToString()
        {
            var choices = "";
            foreach (var c in Choices)
                choices += $"\n\t{c.Order}) {c.Description}";
            return Title + choices;
        }
    }

    public record Person(string Name, int Age);
    public record PersonWithHobbies(string Name, List<string> Hobbies);

    // ---------- Data (6 questions) ----------
    public static class QuestionBank
    {
        private static Question Q(int n, string title, int correct, params string[] choices) =>
            new Question
            {
                Title = $"Q #{n}: {title}",
                Choices = choices.Select((text, i) => new Choice { Order = i + 1, Description = text }).ToList(),
                CorrectAnswer = correct
            };

        // Builds NEW objects every time it is called
        public static List<Question> LoadQuestions() => new()
        {
            Q(1, "Streaming stored audio/video is sometimes referred to as on-demand audio/video", 1,
                 "True", "False"),
            Q(2, "Quantization refers to", 1,
                 "Sampling in the voltage dimension", "Sampling in the frequency dimension",
                 "Sampling in the time dimension", "Sampling in the bitrate dimension"),
            Q(3, "Converting analog audio to compressed binary form is known as", 1,
                 "Pulse code modulation", "Polar quantization", "Pulse code quantization", "Polar code modulation"),
            Q(4, "___________ is an independent frame that is not related to any other frame", 1,
                 "I-frames", "P-frames", "B-frames"),
            Q(5, "Jitter is the variation of packet delay", 1,
                 "True", "False"),
            Q(6, "A peer in a swarm that has the complete content file is called a(n)", 2,
                 "Swarm", "Seed", "Leech", "Tracker"),
        };

        // Built ONCE: reading it twice gives the same objects
        public static readonly List<Question> All = LoadQuestions();
    }

    // ---------- Helper ----------
    public static class Extensions
    {
        public static void Print<T>(this IEnumerable<T> source, string title)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {title} ===");
            Console.WriteLine(string.Join(", ", source));
        }
    }

    // ---------- TODO (Part 4): write your IEqualityComparer<Question> classes here ----------


    internal class Program
    {
        static void Main(string[] args)
        {
            // ---------- Sample data ----------
            var numbers = new List<int> { 1, 2, 3 };
            var names1  = new List<string> { "Sara", "Ali", "Omar" };
            var names2  = new List<string> { "sara", "ali", "omar" };

            // ===================== PART 1: Value types and strings =====================
            // TODO: Task 1.1 ... 1.7


            // ===================== PART 2: Reference types (NO overrides yet) =====================
            // TODO: Task 2.1 ... 2.5


            // ===================== PART 3: Overriding Equals / GetHashCode =====================
            // TODO: Task 3.1 ... 3.6


            // ===================== PART 4: Custom comparers =====================
            // TODO: Task 4.1 ... 4.3


            // ===================== PART 5: Records and mixed =====================
            // TODO: Task 5.1 ... 5.5

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: Value types and strings

**Task 1.1.** Create two **separate** lists `{ 1, 2, 3 }` and compare them.
*Expected:* `True`.

**Task 1.2.** Compare `{ 1, 2, 3 }` with `{ 3, 2, 1 }`. Then sort both and compare again.
*Expected:* `False`, then `True`.

**Task 1.3.** Compare `{ 1, 2, 3 }` with `{ 1, 2, 3, 4 }`. Then compare two **empty** lists.
*Expected:* `False`, then `True`.

**Task 1.4.** Compare `names1` with `names2` (same names, different case). Then compare them with `StringComparer.OrdinalIgnoreCase`.
*Expected:* `False`, then `True`.

**Task 1.5.** Compare an **array** `{ 1, 2, 3 }` with a **`List<int>`** `{ 1, 2, 3 }`. Does the collection type matter?
*Expected:* `True`.

**Task 1.6. (palindromes)** A word is a palindrome if it reads the same backwards. Use `SequenceEqual` and `Reverse` to test `"level"`, `"linq"` and `"Level"` (the last one ignoring case).
*Expected:* `True`, `False`, `True`.
*Hint:* a `string` is a sequence of `char`.

**Task 1.7.** Call `numbers.SequenceEqual(null)` inside `try/catch`. Print the exception type.
*Expected:* `ArgumentNullException`.

---

## Part 2: Reference types (before any override)

Run these **before** adding any `Equals` code.

**Task 2.1.** Take the first three questions from `QuestionBank.All` (`q1`, `q2`, `q3`). Build **two different lists** that both contain `q1, q2, q3`. Compare them.
*Expected:* `True` (same objects, same order).

**Task 2.2.** Create one list and compare it **with itself**.
*Expected:* `True`.

**Task 2.3.** Call `QuestionBank.LoadQuestions().Take(4)` **twice** (two separate calls) and compare the results.
*Expected:* `False`. Explain why in a comment, even though the questions have the same text.

**Task 2.4.** Prove the explanation: for the two lists of Task 2.3 print
- `ReferenceEquals(a[0], b[0])`,
- `a[0].Title == b[0].Title`.

*Expected:* `False` and `True`.

**Task 2.5.** Now compare `QuestionBank.All.Take(4)` with `QuestionBank.All.Take(4)` (the shared list). What is the result and why?
*Expected:* `True`, because both sides hold the same objects.

---

## Part 3: Overriding `Equals` and `GetHashCode`

**Task 3.1.** In the `Question` class, override `Equals` so that two questions are equal when `Title`, `CorrectAnswer` **and** `Choices` (use `Choices.SequenceEqual(...)`) are equal. Also override `GetHashCode` (use `Title` and `CorrectAnswer` for now). Re-run Task 2.3.
*Expected:* **still `False`**. Why? (Answer in a comment.)

**Task 3.2.** Override `Equals` and `GetHashCode` in the `Choice` class as well (`Order` and `Description`). Re-run Task 2.3.
*Expected:* `True`.

**Task 3.3. (the hash code contract)** Change `Question.GetHashCode` so that it includes `Choices.GetHashCode()` (the hash of the list object). Then print:

```csharp
a[0].Equals(b[0]);                           // True
a[0].GetHashCode() == b[0].GetHashCode();    // ???
```

*Expected:* `True` and `False`. This **breaks** the rule that equal objects must have equal hash codes. Now fix it by looping over `Choices` and adding **each choice** to the hash (use the `HashCode` struct). The second line must now print `True`.

**Task 3.4.** Prove why the hash code matters. With the **broken** `GetHashCode` from Task 3.3, run:

```csharp
a.Concat(b).Distinct().Count();       // a and b each have 4 questions
new HashSet<Question>(a.Concat(b)).Count;
```

Then repeat with the **fixed** version.
*Expected:* broken version usually gives `8` (duplicates are not detected); fixed version gives `4`.

**Task 3.5.** Does order matter? Compare `a` with `b` **reversed**, then compare both after ordering them by `Title`.
*Expected:* `False`, then `True`.
*Careful:* `List<T>.Reverse()` reverses **in place** and returns `void`. Use `b.AsEnumerable().Reverse()`.

**Task 3.6.** Now that `Equals` works, check `QuestionBank.All.Contains(copy)` where `copy` is `LoadQuestions()[0]`. Compare with the result **before** the overrides (Session 7 trap).
*Expected:* `True` now, `False` before.

---

## Part 4: Custom comparers

**Task 4.1.** Write a class `QuestionTitleComparer : IEqualityComparer<Question>` that treats two questions as equal when they have the same `Title` (ignore case). Then:

```csharp
var original = QuestionBank.LoadQuestions()[0];
var modified = QuestionBank.LoadQuestions()[0];
modified.CorrectAnswer = 2;
```

Compare `new[] { original }` with `new[] { modified }`:
1. with the default comparison,
2. with your comparer.

*Expected:* `False` (the correct answers differ), then `True` (only titles are compared).

**Task 4.2.** Make a copy of a question with the title in **UPPER CASE** and show that your comparer still says "equal".
*Expected:* `True`.

**Task 4.3.** Use your comparer with `Distinct`: `a.Concat(b).Distinct(new QuestionTitleComparer()).Count()`.
*Expected:* `4`.
Then answer in a comment: **which of these operators need `GetHashCode`**: `SequenceEqual`, `Contains`, `Distinct`, `GroupBy`, `Union`?
*Expected answer:* `Distinct`, `GroupBy`, `Union` use it; `SequenceEqual` and `Contains` only use `Equals`.

---

## Part 5: Records and mixed

**Task 5.1.** Create two lists of `Person` records with the same values (`("Sara", 25)`, `("Ali", 30)`) and compare them.
*Expected:* `True`. No `Equals` code is needed.

**Task 5.2.** Create two `PersonWithHobbies` records with the same name and two separate lists `{ "Chess" }`. Call `Equals`.
*Expected:* `False`. Explain why in a comment (records use each property's `Equals`, and `List<T>` compares by reference).
*Then:* make it `True` by sharing the **same list object** in both records.

**Task 5.3. (sets and order)** Let `a = { 1, 2, 3, 3 }` and `b = { 3, 2, 1 }`. Print the results of:
1. `a.SequenceEqual(b)`,
2. `a.OrderBy(x => x).SequenceEqual(b.OrderBy(x => x))`,
3. `new HashSet<int>(a).SetEquals(b)`,
4. `!a.Except(b).Any() && !b.Except(a).Any()`.

*Expected:* `False`, `False`, `True`, `True`. Explain what each method treats as "equal".

**Task 5.4.** Write a method:

```csharp
static bool SameQuiz(IEnumerable<Question> a, IEnumerable<Question> b, bool ignoreOrder)
```

Test it with the lists from Part 3: same order, reversed with `ignoreOrder = false`, and reversed with `ignoreOrder = true`.
*Expected:* `True`, `False`, `True`.

**Task 5.5.** In a comment, decide: for each case, which would you use (`SequenceEqual`, ordering first, `SetEquals`, or a comparer)?
1. Two shopping carts must have the same items in the same order.
2. Two lists of tags must have the same tags, order does not matter.
3. Two lists of usernames must match, ignoring case.
4. Two quizzes are the same if they contain the same question **titles**.

---

## Challenge (optional)

1. Make `Question` implement `IEquatable<Question>` and use it from `Equals(object)`.
2. Write a generic method `ElementsEqualIgnoringOrder<T>(IEnumerable<T> a, IEnumerable<T> b)` that treats **duplicates** correctly, so `{1, 2, 3, 3}` and `{3, 2, 1}` are **not** equal, but `{1, 2, 3, 3}` and `{3, 3, 2, 1}` **are**.
   *Hint:* count each item with `GroupBy` and compare the counts.

---

## Submission checklist

- [ ] Every task prints a clear result
- [ ] You can explain why equal-looking objects are not equal by default
- [ ] You can explain why `Choice` also needs `Equals`, not only `Question`
- [ ] You can explain the rule between `Equals` and `GetHashCode`
- [ ] You can explain which operators need `GetHashCode`
- [ ] You can compare two sequences ignoring order and ignoring case
