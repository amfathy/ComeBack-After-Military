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
            // Task 1.1
            var task11 = new[] { 1, 2, 3 }
                .Concat(new[] { 4, 5 });

            task11.Print("Task 1.1");

            // Task 1.2
            var task12 = new[] { 1, 2, 3 }
                .Concat(new[] { 3, 4 });

            task12.Print("Task 1.2");
            Console.WriteLine($"Count: {task12.Count()}");
            

            // Task 1.3
            var a = new[] { 1, 2 };
            var b = new[] { 3, 4 };

            a.Concat(b).Print("a.Concat(b)");
            b.Concat(a).Print("b.Concat(a)");


            // ===================== PART 2: Questions =====================
            // TODO: Task 2.1 ... 2.6
            // Task 2.1
            var task21 = all.Take(3)
                .Concat(all.TakeLast(2));

            task21.Select(q => q.Title).Print("Task 2.1");

            // Task 2.2
            var randomQuiz = QuestionBank.Randomize(3)
                .Concat(QuestionBank.Randomize(2))
                .ToList();

            // Task 2.3
            var task23 = QuestionBank.Randomize(3)
                .Select(q => q.Title)
                .Concat(QuestionBank.Randomize(2).Select(q => q.Title))
                .ToList();

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