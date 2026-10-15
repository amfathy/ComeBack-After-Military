# LINQ Session 13: Tasks (`Concat`)

## How to use

1. Create a new Console project (.NET 5 or later).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helpers, so **no shared project is needed**.
3. Solve each task in its `// TODO` area and print the results.
4. Use **method syntax**. Tasks marked *(random)* give a different result on every run, so the expected result describes what must **always** be true.

## The data

- A **question bank of 8 questions** (`Q #1` to `Q #8`) with the helper methods `Randomize(count)`, `GetQuestionRange(indexes)` and `ToQuiz()`.
  - `QuestionBank.All` is built **once**, so questions taken from it are the **same objects** every time.
  - `GetQuestionRange` works with **zero-based indexes** (index `0` is `Q #1`).
- Small lists of numbers and names.
- A small class hierarchy: `Animal` with `Dog` and `Cat`.

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession13Tasks
{
    // ---------- Models ----------
    public class Choice
    {
        public int Order { get; set; }
        public string Description { get; set; }
    }

    public class Question
    {
        public string Title { get; set; }
        public List<Choice> Choices { get; set; } = new();
        public int CorrectAnswer { get; set; }

        public override string ToString()
        {
            var choices = "";
            foreach (var c in Choices)
                choices += $"\n\t{c.Order}) {c.Description}";
            return Title + choices;
        }
    }

    public class Animal
    {
        public string Name { get; set; }
        public override string ToString() => $"{GetType().Name} {Name}";
    }
    public class Dog : Animal { }
    public class Cat : Animal { }

    // ---------- Data (8 questions) ----------
    public static class QuestionBank
    {
        private static readonly Random random = new Random();

        private static Question Q(int n, string title, int correct, params string[] choices) =>
            new Question
            {
                Title = $"Q #{n}: {title}",
                Choices = choices.Select((text, i) => new Choice { Order = i + 1, Description = text }).ToList(),
                CorrectAnswer = correct
            };

        public static readonly List<Question> All = new()
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
            Q(7, "RTP port numbers must be", 1,
                 "An even number", "An odd number", "A prime number"),
            Q(8, "In the Kademlia protocol, the distance between nodes is determined by bitwise", 1,
                 "XOR", "XAND", "OR", "AND"),
        };

        // Returns "count" random questions (a different random order each call)
        public static IEnumerable<Question> Randomize(int count) =>
            All.OrderBy(x => random.Next()).ToList().Take(count);

        // Returns the questions whose ZERO-BASED index is in the given range
        public static List<Question> GetQuestionRange(IEnumerable<int> range) =>
            All.Where((x, i) => range.Contains(i)).ToList();
    }

    // ---------- Helpers ----------
    public static class Extensions
    {
        public static void ToQuiz(this IEnumerable<Question> questions)
        {
            foreach (var question in questions)
            {
                Console.WriteLine(question);
                Console.WriteLine();
            }
        }

        public static void Print<T>(this IEnumerable<T> source, string title)
        {
            Console.WriteLine();
            Console.WriteLine($"=== {title} ===");
            Console.WriteLine(string.Join(", ", source));
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // ---------- Sample data ----------
            var all = QuestionBank.All;
            var names1 = new List<string> { "Sara", "Ali" };
            var names2 = new List<string> { "Omar", "Sara" };
            var names3 = new List<string> { "Mona" };

            var dogs = new List<Dog> { new Dog { Name = "Rex" }, new Dog { Name = "Max" } };
            var cats = new List<Cat> { new Cat { Name = "Tom" }, new Cat { Name = "Kitty" } };

            // ===================== PART 1: Numbers and strings =====================
            // TODO: Task 1.1 ... 1.6


            // ===================== PART 2: Questions =====================
            // TODO: Task 2.1 ... 2.6


            // ===================== PART 3: Types must match =====================
            // TODO: Task 3.1 ... 3.4


            // ===================== PART 4: Concat vs Union / SelectMany / AddRange =====================
            // TODO: Task 4.1 ... 4.6


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.5

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: Numbers and strings

**Task 1.1.** Concat `{ 1, 2, 3 }` and `{ 4, 5 }`.
*Expected:* `1, 2, 3, 4, 5`

**Task 1.2.** Concat `{ 1, 2, 3 }` and `{ 3, 4 }`. How many items? Is the `3` repeated?
*Expected:* `1, 2, 3, 3, 4` (5 items, `3` appears twice).

**Task 1.3.** Does order matter? Print `a.Concat(b)` and `b.Concat(a)` for `a = { 1, 2 }` and `b = { 3, 4 }`.
*Expected:* `1, 2, 3, 4` and `3, 4, 1, 2`.

**Task 1.4.** Concat a list with an **empty** list (both orders). What do you get?
*Expected:* the same items as the non-empty list.

**Task 1.5.** Chain four sequences: `{ 1, 2 }`, `{ 3 }`, `{ 4, 5 }`, `{ 6 }`.
*Expected:* `1, 2, 3, 4, 5, 6`

**Task 1.6.** Call `numbers.Concat(null)` inside `try/catch`. Print the exception type.
*Expected:* `ArgumentNullException`.

---

## Part 2: Questions

**Task 2.1.** Join the **first 3** questions and the **last 2** questions of the bank.
*Expected:* `Q #1, Q #2, Q #3, Q #7, Q #8`.

**Task 2.2. (random)** Concat `Randomize(3)` and `Randomize(2)`. Print how many questions you get and use `ToQuiz()`.
*Expected:* always **5** items.
Then write code that checks whether the result contains a **duplicate question**, and run it **20 times**, counting how often a duplicate appeared.
*Expected:* duplicates appear often (roughly 2 runs out of 3). Explain why in a comment.

**Task 2.3. (random)** Do the same as Task 2.2 but concat only the **titles** (`Select(q => q.Title)` before `Concat`).
*Expected:* 5 strings. Write the type of the result in a comment.

**Task 2.4.** Create an `intro` question and an `outro` question (`Title = "Welcome!"` and `Title = "Thank you!"`). Build the quiz: **intro, the first 3 questions, outro**:
1. with `Prepend` / `Append`,
2. with `Concat` and one-item arrays.

*Expected:* 5 items; both ways give the same result; the original list of 8 is unchanged.

**Task 2.5.** Use `GetQuestionRange` to get indexes `1, 2, 3` and indexes `5, 6`, then concat them.
*Expected:* `Q #2, Q #3, Q #4, Q #6, Q #7`.

**Task 2.6. (the Range trap)** Concat `GetQuestionRange(Enumerable.Range(0, 2))` with `GetQuestionRange(Enumerable.Range(6, 10))`. How many questions do you get? Why?
*Expected:* `4` items (`Q #1, Q #2, Q #7, Q #8`): the range `6..15` only has two indexes that exist. `Range` takes a **count**, not an end value.

---

## Part 3: Types must match

**Task 3.1.** Try to concat `new[] { 1, 2, 3 }` and `new[] { 4.5, 5.5 }`. Does it compile? Fix it.
*Expected:* it does not compile as written; after converting the ints with `Select(i => (double)i)` you get `1, 2, 3, 4.5, 5.5`.

**Task 3.2.** Concat `{ 1, 2 }` and `{ "Sara" }` by converting the numbers to strings.
*Expected:* `"1", "2", "Sara"`.

**Task 3.3.** Concat `dogs` and `cats` into one `IEnumerable<Animal>` using `Concat<Animal>(...)`. Print all names.
*Expected:* `Dog Rex, Dog Max, Cat Tom, Cat Kitty`.
Also try `dogs.Concat(cats)` without the type argument and write in a comment what happens (it does not compile).

**Task 3.4. (the Cast trap)** Try `new[] { 1, 2 }.Cast<double>().ToList()` inside `try/catch`. What happens? Compare with `Select(i => (double)i)`.
*Expected:* `InvalidCastException` for `Cast`, and a working result for `Select`.

---

## Part 4: `Concat` vs `Union`, `SelectMany`, `AddRange`

**Task 4.1.** For `{ 1, 2, 3 }` and `{ 3, 4 }`, print the **count** of `Concat`, `Union` and `Concat(...).Distinct()`.
*Expected:* `5`, `4`, `4`.

**Task 4.2.** Take `q1 = all.Take(4)` (`Q #1` to `Q #4`) and `q2 = all.Skip(2).Take(4)` (`Q #3` to `Q #6`). Print the count of `Concat` and of `Union`.
*Expected:* `8` and `6`. Explain why `Union` can remove the duplicates here (same objects).

**Task 4.3.** Given:

```csharp
var groups = new List<List<int>> { new() { 1, 2 }, new() { 3 }, new() { 4, 5, 6 }, new() };
```

Flatten it with `SelectMany`, and again with a chain of `Concat` calls. Check that both are equal with `SequenceEqual`.
*Expected:* `1, 2, 3, 4, 5, 6` and `True`.

**Task 4.4.** Flatten `groups` with `Aggregate(Enumerable.Empty<int>(), (acc, g) => acc.Concat(g))`. When is this better or worse than `SelectMany`? (Answer in a comment.)
*Expected:* `1, 2, 3, 4, 5, 6`.

**Task 4.5. (deferred execution)**

```csharp
var a = new List<int> { 1, 2 };
var b = new List<int> { 3 };
```

1. Create `var c = a.Concat(b);` and `var snapshot = a.Concat(b).ToList();`.
2. Then `a.Add(99);`.
3. Print `c` and `snapshot`.

*Expected:* `c` is `1, 2, 99, 3`; `snapshot` is `1, 2, 3`.

**Task 4.6.** Compare `list1.Concat(list2)` with `list1.AddRange(list2)`: after each, print `list1.Count`.
*Expected:* `Concat` leaves `list1` unchanged; `AddRange` changes it.

---

## Part 5: Mixed

**Task 5.1. (random)** Build a quiz of **5 questions with no duplicates** from two draws: first `Randomize(3)`, then 2 random questions taken from the bank **excluding** the first three (`all.Except(first)`). Concat them.
*Expected:* always 5 questions, and `quiz.Distinct().Count()` is always `5`. Run it several times.

**Task 5.2.** Concat `names1`, `names2` and `names3`, then print them numbered: `1. Sara`, `2. Ali`, ...
*Expected:* 5 lines (`Sara` appears twice).
*Hint:* `Select((name, i) => ...)`.

**Task 5.3.** From the same three name lists, get the **unique** names sorted alphabetically.
*Expected:* `Ali, Mona, Omar, Sara`.

**Task 5.4. (report)** Build a text report from `names1`:

```
=== Report ===
1. Sara
2. Ali
Total: 2
```

Use a header line, the numbered lines and a footer line, joined with `Prepend`/`Append` or `Concat`, and print each line.

**Task 5.5.** In a comment, choose the right tool for each case (`Concat`, `Union`, `SelectMany`, `AddRange`, `Append`) and say why:
1. Two lists of orders must be shown together, duplicates included.
2. Two mailing lists must be merged without sending duplicate emails.
3. A report has 12 monthly lists that must become one list.
4. A single "Total" row must be added at the end of a table.
5. You really want to change an existing list by adding items to it.

---

## Challenge (optional)

**Interleave** two sequences: `a = { 1, 2, 3 }` and `b = { 10, 20, 30 }` become `1, 10, 2, 20, 3, 30`.
*Hint:* `Zip` (Session 3) to make pairs, then `SelectMany` to flatten them.

---

## Submission checklist

- [ ] Every task prints a clear result
- [ ] You can explain why `Concat` keeps duplicates, and when to use `Union` instead
- [ ] You can explain why order matters in `Concat`
- [ ] You can explain deferred execution with `Concat`
- [ ] You can join lists of different types
- [ ] You know the difference between `Concat` and `AddRange`
