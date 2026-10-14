# LINQ Session 10: Tasks (`Range`, `Repeat`, `Empty`, `DefaultIfEmpty`)

## How to use

1. Create a new Console project (.NET 5 or later).
2. Replace `Program.cs` with the **Starter Code** below. It already has the data, classes and helpers, so **no shared project is needed**.
3. Solve each task in its `// TODO` area and print the results.
4. Use **method syntax**. Remember: `Range`, `Repeat` and `Empty` are called on `Enumerable`, for example `Enumerable.Range(1, 5)`.

## The data

A small **question bank of 12 multiple-choice questions** (networking and multimedia). Each question has a `Title`, a list of `Choices` and the number of the correct choice (`CorrectAnswer`, starting at 1).

| # | Choices | Topic |
|---|---|---|
| 1, 7 | 2 (True / False) | streaming, jitter |
| 6, 8 | 3 | I-frames, RTP ports |
| 2, 3, 4, 5, 9, 10, 11, 12 | 4 | quantization, Nyquist, PCM, MPEG, BitTorrent, Kademlia, SSL, search overlays |

---

## Starter Code (copy into `Program.cs`)

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession10Tasks
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

        // Placeholder shown when there is nothing to show
        public static readonly Question Default = new Question
        {
            Title = "<<<<< QUESTION TITLE GOES HERE >>>>>",
            Choices = new List<Choice>
            {
                new Choice { Order = 1, Description = "<<<<< CHOICE #1 GOES HERE >>>>>" },
                new Choice { Order = 2, Description = "<<<<< CHOICE #2 GOES HERE >>>>>" },
            },
            CorrectAnswer = 0
        };

        public override string ToString()
        {
            var choices = "";
            foreach (var c in Choices)
                choices += $"\n\t{c.Order}) {c.Description}";
            return Title + choices;
        }
    }

    // ---------- Data (12 questions) ----------
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
        };

        public static Question PickOne() => All[random.Next(0, All.Count)];

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
            // ===================== PART 1: Range =====================
            // TODO: Task 1.1 ... 1.8


            // ===================== PART 2: Range with the question bank =====================
            // TODO: Task 2.1 ... 2.4


            // ===================== PART 3: Repeat =====================
            // TODO: Task 3.1 ... 3.6


            // ===================== PART 4: Empty and DefaultIfEmpty =====================
            // TODO: Task 4.1 ... 4.6


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.5

            Console.ReadKey();
        }
    }
}
```

---

## Part 1: `Range`

**Task 1.1.** Generate the numbers `1` to `10` and print them.
*Expected:* `1, 2, 3, 4, 5, 6, 7, 8, 9, 10`

**Task 1.2.** Call `Enumerable.Range(5, 3)`. What does it return? Explain what the second parameter means in a comment.
*Expected:* `5, 6, 7`

**Task 1.3.** Generate the **even numbers from 1 to 20**.
*Expected:* `2, 4, 6, 8, 10, 12, 14, 16, 18, 20`

**Task 1.4.** Generate the **squares** of the numbers `1` to `10`.
*Expected:* `1, 4, 9, 16, 25, 36, 49, 64, 81, 100`

**Task 1.5.** Print the **multiplication table of 7**, like `7 x 1 = 7`.
*Expected last line:* `7 x 10 = 70`

**Task 1.6.** Calculate the **sum of the numbers from 1 to 100** with one line of LINQ.
*Expected:* `5050`

**Task 1.7.** What happens with `Enumerable.Range(1, 0)`? And with `Enumerable.Range(1, -1)`? Test both (use `try/catch` for the second) and describe the result.
*Expected:* an empty sequence, then an `ArgumentOutOfRangeException`.

**Task 1.8.** Generate the letters `A` to `Z`.
*Hint:* `Range` + `Select`, and `(char)('A' + i)`.
*Expected:* 26 letters.

---

## Part 2: `Range` with the question bank

**Task 2.1.** Using `QuestionBank.GetQuestionRange(...)` and `Enumerable.Range`, get the **first 5 questions** and print them with `ToQuiz()`.
*Expected:* `Q #1` to `Q #5`.

**Task 2.2.** Now use `Enumerable.Range(1, 5)` with the same method. Which questions do you get? Why?
*Expected:* `Q #2` to `Q #6`, because the range is used as **zero-based indexes**.

**Task 2.3.** Print the questions **at positions 1, 3, 5, 7, 9, 11** (the odd question numbers).
*Hint:* build the indexes with `Range(0, 6).Select(i => i * 2)`.
*Expected:* `Q #1, #3, #5, #7, #9, #11`.

**Task 2.4.** Print the **last 3 questions** without hard-coding the numbers.
*Hint:* `Enumerable.Range(All.Count - 3, 3)`.
*Expected:* `Q #10`, `Q #11`, `Q #12`.

---

## Part 3: `Repeat`

**Task 3.1.** Create the text `"**********"` (10 stars) with `Repeat`.

**Task 3.2.** Create a list of **five zeros**.
*Expected:* `0, 0, 0, 0, 0`

**Task 3.3.** Print a separator line of 40 characters, made from the repeated pattern `"=-"`.
*Hint:* `string.Concat(Enumerable.Repeat("=-", 20))`.

**Task 3.4. (the trap)** Create **your own** `Question` object (do not take one from the bank) and make a list of 10 with `Enumerable.Repeat(q, 10).ToList()`.
1. Print `ReferenceEquals(list[0], list[1])`.
2. Change `list[0].Title`.
3. Print the title of `list[5]`.

*Expected:* `True`, and the title of `list[5]` has changed too. Explain why in a comment.

**Task 3.5.** Create 10 **separate** `Question` objects with `Enumerable.Range(0, 10).Select(...)`. Print `ReferenceEquals(list[0], list[1])`.
*Expected:* `False`.

**Task 3.6.** Create 10 separate objects a third way with a `for` loop and compare. What is the advantage of the LINQ way? (Answer in a comment.)

---

## Part 4: `Empty` and `DefaultIfEmpty`

**Task 4.1.** Print:
- `ReferenceEquals(Enumerable.Empty<int>(), Enumerable.Empty<int>())`
- `ReferenceEquals(new List<int>(), new List<int>())`

*Expected:* `True`, then `False`.

**Task 4.2.** Write a method:

```csharp
static IEnumerable<Question> FindQuestions(string text)
```

It must return `Enumerable.Empty<Question>()` (never `null`) when `text` is null or blank, and otherwise return the questions whose title contains `text` (ignore case). Test it with `""`, `"jitter"` and `"zzz"`, and loop over each result **without** a null check.
*Expected counts:* `0`, `1`, `0`.

**Task 4.3.** Call `DefaultIfEmpty()` (no argument) on an **empty `List<int>`** and on an **empty `List<Question>`**. How many items does each give, and what are they?
*Expected:* 1 item each: `0` for the ints, `null` for the questions.

**Task 4.4.** Call `DefaultIfEmpty(Question.Default)` on an **empty** question list and print it with `ToQuiz()`. Then call it on the **full** bank. How many items do you get?
*Expected:* the placeholder question first; then `12` items (the bank is unchanged).

**Task 4.5.** Search the bank for `"blockchain"` and for `"RTP"`, using `Where` + `DefaultIfEmpty(Question.Default)`. Print both with `ToQuiz()`.
*Expected:* the placeholder for `"blockchain"`, and `Q #8` for `"RTP"`.

**Task 4.6.** Calculate the average of an empty `List<int>`. What happens? Fix it with `DefaultIfEmpty`.
*Expected:* `InvalidOperationException`; with the fix the result is `0`.

---

## Part 5: Mixed

**Task 5.1.** **Generate 5 test questions** named `"Test question 1"` to `"Test question 5"`, each with 4 choices named `"Choice 1"` to `"Choice 4"`. Use `Range` + `Select` for both the questions and the choices.
*Expected:* 5 questions with 4 choices each.

**Task 5.2.** Pick **5 random, different** questions from the bank (shuffle and take 5).
*Hint:* `OrderBy(x => Guid.NewGuid()).Take(5)`.
*Expected:* 5 distinct questions.

**Task 5.3.** Group the bank by the **number of choices** and print the count per group.
*Expected:* `4 choices: 8`, `3 choices: 2`, `2 choices: 2`.

**Task 5.4.** Given `var existing = new[] { 1, 2, 4, 7 };`, find the **missing numbers** between 1 and 10.
*Hint:* `Range` + `Except`.
*Expected:* `3, 5, 6, 8, 9, 10`

**Task 5.5. FizzBuzz.** For the numbers 1 to 20, print `Fizz` for multiples of 3, `Buzz` for multiples of 5, `FizzBuzz` for both, and the number otherwise.
*Expected:* `1, 2, Fizz, 4, Buzz, Fizz, 7, 8, Fizz, Buzz, 11, Fizz, 13, 14, FizzBuzz, 16, 17, Fizz, 19, Buzz`

---

## Challenge (optional)

Print a **triangle** of 5 lines using only `Range`, `Repeat` and `Select`:

```
*
**
***
****
*****
```

Then print it as a centered **pyramid** (spaces before the stars).

---

## Submission checklist

- [ ] Every task has its own clear output
- [ ] You can explain that the second parameter of `Range` is a **count**, not an end value
- [ ] You can explain why `Repeat` is dangerous with your own classes
- [ ] You can explain why returning `Enumerable.Empty<T>()` is better than returning `null`
- [ ] You can explain when `DefaultIfEmpty` returns `null`, `0` or your own value
