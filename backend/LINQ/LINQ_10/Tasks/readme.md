# LINQ Session 11: Tasks (`ElementAt`, `First`, `Last`, `Single`)

## How to use

1. Create a new Console project (.NET 6 or later, because a couple of tasks use `^1` and the default-value overload).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helpers, so **no shared project is needed**.
3. Solve each task in its `// TODO` area and print the results.
4. For tasks that should **throw**, wrap the call in `try/catch` and print the exception type and message. Do not let the program crash.

## The data

A bank of **14 multiple-choice questions** (networking and multimedia), plus a few small lists.

| Question | Choices | Notes |
|---|---|---|
| Q #1, Q #7 | 2 | True / False |
| Q #6, Q #8 | 3 | |
| Q #2 to #5, #9 to #12 | 4 | |
| **Q #13** (SSL certificate) | 4 | **Duplicate number!** |
| **Q #13** (What is PAN) | 4 | **Duplicate number!** |

> The two questions numbered `#13` are **on purpose**. They copy a real problem from the original question bank (two questions with the same number), and they are what makes `Single` fail in some tasks.
> `CorrectAnswer` is the number of the correct choice (starting at 1). In `Q #9` and `Q #12` it is `2`; in all others it is `1`.

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession11Tasks
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

    // ---------- Data (14 questions) ----------
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
            Q(1,  "Streaming stored audio/video is sometimes referred to as on-demand audio/video", 1,
                  "True", "False"),
            Q(2,  "Quantization refers to", 1,
                  "Sampling in the voltage dimension", "Sampling in the frequency dimension",
                  "Sampling in the time dimension", "Sampling in the bitrate dimension"),
            Q(3,  "Nyquist theorem states", 1,
                  "If the highest frequency is f, sample 2f times per second",
                  "If the highest frequency is f, sample f times per second",
                  "If the highest frequency is f, sample 3f times per second",
                  "If the highest frequency is f, sample 4f times per second"),
            Q(4,  "Converting analog audio to compressed binary form (sampling, quantization, encoding) is known as", 1,
                  "Pulse code modulation", "Polar quantization", "Pulse code quantization", "Polar code modulation"),
            Q(5,  "The 3 categories that MPEG divides its frames into are", 1,
                  "I-frames, P-frames, and B-frames", "A-frames, B-frames, and C-frames",
                  "B-frames, P-frames, and R-frames", "T-frames, I-frames, and B-frames"),
            Q(6,  "___________ is an independent frame that is not related to any other frame", 1,
                  "I-frames", "P-frames", "B-frames"),
            Q(7,  "Jitter is the variation of packet delay", 1,
                  "True", "False"),
            Q(8,  "RTP port numbers must be", 1,
                  "An even number", "An odd number", "A prime number"),
            Q(9,  "A peer in a swarm that has the complete content file is called a(n)", 2,
                  "Swarm", "Seed", "Leech", "Tracker"),
            Q(10, "In the Kademlia protocol, the distance between nodes is determined by bitwise", 1,
                  "XOR", "XAND", "OR", "AND"),
            Q(11, "___________ is a standard security technology for establishing an encrypted link between a server and a client", 1,
                  "SSL", "RSA", "SHA-2", "TCP"),
            Q(12, "Which unstructured overlay algorithm increases the TTL value by a small amount if the query fails?", 2,
                  "Flooding", "Expanding ring", "Dijkstra", "Pastry"),
            Q(13, "A ___________ is a small data file that digitally binds a cryptographic key to an organization's details", 1,
                  "SSL certificate", "Private key", "Public key", "RSA certificate"),
            Q(13, "What is PAN", 1,
                  "A network that lets devices connect over the range of a person (e.g. Bluetooth)",
                  "A privately owned network inside a single building",
                  "A network that spans a city",
                  "A network that spans a country"),
        };
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

        // TODO (Challenge): write TryFirst here
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // ---------- Sample data ----------
            var questions = QuestionBank.All;
            var numbers   = new List<int> { 5, 3, 9, 1, 7, 3, 8 };
            var empty     = new List<Question>();
            var oneItem   = new List<int> { 42 };

            // ===================== PART 1: ElementAt / ElementAtOrDefault =====================
            // TODO: Task 1.1 ... 1.6


            // ===================== PART 2: First / FirstOrDefault =====================
            // TODO: Task 2.1 ... 2.7


            // ===================== PART 3: Last / LastOrDefault =====================
            // TODO: Task 3.1 ... 3.5


            // ===================== PART 4: Single / SingleOrDefault =====================
            // TODO: Task 4.1 ... 4.7


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.5

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: `ElementAt` / `ElementAtOrDefault`

**Task 1.1.** Get the question at index `0` and at index `3`.
*Expected:* `Q #1` and `Q #4`.

**Task 1.2.** Get the question at index `10`. Which question number is it? Why?
*Expected:* `Q #11`, because indexes start at 0.

**Task 1.3.** Call `ElementAt(100)` inside `try/catch`. Print the exception type.
*Expected:* `ArgumentOutOfRangeException`.

**Task 1.4.** Call `ElementAtOrDefault(100)`. If the result is `null`, print `"No question at this position"`.

**Task 1.5.** Call `numbers.ElementAtOrDefault(10)`. What is the result? Can you tell it apart from a real `0` in the list? (Answer in a comment.)
*Expected:* `0`; no, you cannot.

**Task 1.6.** Get the **last** question using `ElementAt` with `^1`, then using `Count - 1`.
*Expected:* `Q #13: What is PAN` both ways.

---

## Part 2: `First` / `FirstOrDefault`

**Task 2.1.** Get the first question.
*Expected:* `Q #1`.

**Task 2.2.** Get the **first question with 3 choices**.
*Expected:* `Q #6`.

**Task 2.3.** Use `First` with the rule `Title.Contains("blockchain")` inside `try/catch`. Print the exception type and message.
*Expected:* `InvalidOperationException`, message about *no matching element*.

**Task 2.4.** Do the same with `FirstOrDefault`. Print `"Not found"` when the result is `null`.

**Task 2.5.** Get the first question whose title contains `"Jitter"`.
*Expected:* `Q #7`.

**Task 2.6.** Call `First()` and `FirstOrDefault()` on the **empty** list. What happens in each case?
*Expected:* `InvalidOperationException`, and `null`.

**Task 2.7.** From `numbers`, get:
- the first number greater than `6` (expected `9`),
- the first number greater than `100` using `FirstOrDefault` (expected `0`),
- the same search with a custom default of `-1` (expected `-1`; this uses the .NET 6 overload).

---

## Part 3: `Last` / `LastOrDefault`

**Task 3.1.** Get the last question.
*Expected:* `Q #13: What is PAN`.

**Task 3.2.** Get the **last question with exactly 2 choices**.
*Expected:* `Q #7`.

**Task 3.3.** Get the last question whose `CorrectAnswer` is **not** `1`.
*Expected:* `Q #12`.

**Task 3.4.** Use `LastOrDefault` to look for a question with 6 choices. Handle the `null`.
*Expected:* `null` (print a message).

**Task 3.5.** From `numbers`, get the last number that is **less than 5**.
*Expected:* `3` (the second `3`, at index 5).

---

## Part 4: `Single` / `SingleOrDefault`

**Task 4.1.** Get the single question whose title starts with `"Q #7:"`.
*Expected:* `Q #7`.

**Task 4.2.** Try `Single` with the rule `Title.Contains("#1")` inside `try/catch`. Print the exception message. How many questions actually match? Print the count.
*Expected:* `InvalidOperationException` (more than one match). The count is `6` (`#1`, `#10`, `#11`, `#12` and the two `#13`).

**Task 4.3.** Complete this table by running the code (use `try/catch` where needed):

| Rule | `Single` | `SingleOrDefault` |
|---|---|---|
| Title starts with `"Q #7:"` (1 match) | ? | ? |
| Title contains `"blockchain"` (0 matches) | ? | ? |
| Title starts with `"Q #13:"` (2 matches) | ? | ? |

*Expected:*

| Rule | `Single` | `SingleOrDefault` |
|---|---|---|
| 1 match | the question | the question |
| 0 matches | throws | `null` |
| 2 matches | throws | **throws** |

**Task 4.4.** Explain in a comment why `SingleOrDefault` threw for the `#13` search even though it is the "OrDefault" version.

**Task 4.5.** Use `Single`, `SingleOrDefault`, `First` and `FirstOrDefault` on:
- `oneItem` (`{ 42 }`): all four should return `42`,
- an **empty** `List<int>`: what does each one do?

*Expected for the empty list:* `Single` throws, `SingleOrDefault` returns `0`, `First` throws, `FirstOrDefault` returns `0`.

**Task 4.6.** Find the **duplicate question number(s)** in the bank.
*Hint:* take the part of the title before the `":"` (for example `title.Split(':')[0]`), then use `GroupBy` and keep the groups with `Count() > 1`.
*Expected:* `Q #13`.

**Task 4.7.** Why is `Single` a good choice when searching by a unique ID, and why can it be a problem when the data has duplicates? Answer in a comment.

---

## Part 5: Mixed

**Task 5.1.** Get the question with the **most choices**. If several tie, take the **first** one.
*Hint:* `OrderByDescending(...)` + `First()`.
*Expected:* `Q #2` (the first question with 4 choices).

**Task 5.2.** Get the **first** question whose `CorrectAnswer` is not `1`, and the **last** one.
*Expected:* `Q #9` and `Q #12`.

**Task 5.3.** Write this method and test it with `7`, `13` and `99`:

```csharp
static string DescribeQuestion(List<Question> questions, int number)
```

It must return:
- the question title if exactly one question has that number,
- `"Not found"` if none,
- `"Duplicate!"` if there is more than one.

Do it **without** `try/catch` (count the matches first).
*Expected:* the title of `Q #7`, then `"Duplicate!"`, then `"Not found"`.

**Task 5.4.** Pick a **random question** using `ElementAt(random index)`. Explain why this can never throw if you use `random.Next(0, questions.Count)`.

**Task 5.5.** In a comment, choose the right operator for each situation and say why:
1. Find an employee by a unique employee ID.
2. Get the most recent order of a customer who may have no orders.
3. Get the 5th item of a list you know has at least 10 items.
4. Read the one and only config row that must exist.

---

## Challenge (optional)

Write the extension method:

```csharp
public static bool TryFirst<T>(this IEnumerable<T> source, Func<T, bool> rule, out T result)
```

It returns `true` and the first match if found, otherwise `false`. Use it like the `int.TryParse` pattern:

```csharp
if (questions.TryFirst(q => q.Choices.Count == 3, out var q))
    Console.WriteLine(q);
```

Why is this pattern useful for **value types**, where `FirstOrDefault` is ambiguous?

---

## Submission checklist

- [ ] Every task that should throw is wrapped in `try/catch` and prints the exception type
- [ ] You can explain `First` vs `Single`
- [ ] You can explain why `SingleOrDefault` still throws when there is more than one match
- [ ] You can explain why `FirstOrDefault` on numbers can be ambiguous
- [ ] You know when to prefer the `OrDefault` version and when not to
