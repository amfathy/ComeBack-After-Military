using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqSession14Tasks
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
        public int Marks { get; set; }

        public override string ToString() => $"{Title}  [{Marks} mark(s), {Choices.Count} choices]";
    }

    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public override string ToString() => $"{Name} ({Price:N0} x {Quantity})";
    }

    // ---------- Data ----------
    public static class QuestionBank
    {
        private static Question Q(int n, string title, int marks, int correct, params string[] choices) =>
            new Question
            {
                Title = $"Q #{n}: {title}",
                Marks = marks,
                CorrectAnswer = correct,
                Choices = choices.Select((text, i) => new Choice { Order = i + 1, Description = text }).ToList()
            };

        public static readonly List<Question> All = new()
        {
            Q(1,  "Streaming stored audio/video is sometimes referred to as on-demand audio/video", 1, 1,
                  "True", "False"),
            Q(2,  "Quantization refers to", 1, 1,
                  "Sampling in the voltage dimension", "Sampling in the frequency dimension",
                  "Sampling in the time dimension", "Sampling in the bitrate dimension"),
            Q(3,  "Nyquist theorem states", 2, 1,
                  "If the highest frequency is f, sample 2f times per second",
                  "If the highest frequency is f, sample f times per second",
                  "If the highest frequency is f, sample 3f times per second",
                  "If the highest frequency is f, sample 4f times per second"),
            Q(4,  "Converting analog audio to compressed binary form (sampling, quantization, encoding) is known as", 2, 1,
                  "Pulse code modulation", "Polar quantization", "Pulse code quantization", "Polar code modulation"),
            Q(5,  "The 3 categories that MPEG divides its frames into are", 2, 1,
                  "I-frames, P-frames, and B-frames", "A-frames, B-frames, and C-frames",
                  "B-frames, P-frames, and R-frames", "T-frames, I-frames, and B-frames"),
            Q(6,  "___________ is an independent frame that is not related to any other frame", 1, 1,
                  "I-frames", "P-frames", "B-frames"),
            Q(7,  "Jitter is the variation of packet delay", 1, 1,
                  "True", "False"),
            Q(8,  "RTP port numbers must be", 1, 1,
                  "An even number", "An odd number", "A prime number"),
            Q(9,  "A peer in a swarm that has the complete content file is called a(n)", 2, 2,
                  "Swarm", "Seed", "Leech", "Tracker"),
            Q(10, "In the Kademlia protocol, the distance between nodes is determined by bitwise", 3, 1,
                  "XOR", "XAND", "OR", "AND"),
            Q(11, "___________ is a standard security technology for establishing an encrypted link between a server and a client", 2, 1,
                  "SSL", "RSA", "SHA-2", "TCP"),
            Q(12, "Which unstructured overlay algorithm increases the TTL value by a small amount if the query fails?", 3, 2,
                  "Flooding", "Expanding ring", "Dijkstra", "Pastry"),
        };

        // Returns the questions whose ZERO-BASED index is in the given range
        public static List<Question> GetQuestionRange(IEnumerable<int> range) =>
            All.Where((x, i) => range.Contains(i)).ToList();
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

    internal class Program
    {
        static void Main(string[] args)
        {
            // ---------- Sample data ----------
            var quiz = QuestionBank.All;
            var numbers = new List<int> { 4, 8, 15, 16, 23, 42 };
            var names = new[] { "Ali", "Salem", "Abeer", "Reem", "Jalal" };

            var products = new List<Product>
            {
                new Product { Name = "Laptop",  Price = 25000, Quantity = 2  },
                new Product { Name = "Phone",   Price = 15000, Quantity = 5  },
                new Product { Name = "Tablet",  Price = 9000,  Quantity = 3  },
                new Product { Name = "Monitor", Price = 6000,  Quantity = 4  },
                new Product { Name = "Mouse",   Price = 300,   Quantity = 20 },
            };

            // ===================== PART 1: Count, Sum, Average, Min, Max (numbers) =====================
            // TODO: Task 1.1 ... 1.8
            // Task 1.3
            Console.WriteLine(numbers.Sum(n => n * n));

            // Task 1.4
            var smallNumbers = new[] { 1, 2, 4 };
            Console.WriteLine(smallNumbers.Sum() / smallNumbers.Count());
            Console.WriteLine(smallNumbers.Average());

            // ===================== PART 2: The question bank =====================
            // TODO: Task 2.1 ... 2.8
            // Task 2.3
            Console.WriteLine(quiz.MaxBy(q => q.Marks));
            Console.WriteLine(quiz.MinBy(q => q.Marks));

            // Task 2.5
            Console.WriteLine("\n=== Task 2.5 ===");
            Console.WriteLine(quiz.Where(q => q.Choices.Count < 3).Sum(q => q.Marks));

            // ===================== PART 3: Aggregate =====================
            // TODO: Task 3.1 ... 3.9
            // Task 3.3
            Console.WriteLine(new[] { 1, 2, 3, 4, 5 }
            .Aggregate(2, (a, b) => a + b));

            // Task 3.4
            Console.WriteLine(
                $"5!: {Enumerable.Range(1, 5).Aggregate(1, (a, b) => a * b)}");

            // ===================== PART 4: Products =====================
            // TODO: Task 4.1 ... 4.7
            // Task 4.1
            Console.WriteLine(products.Sum(p => p.Price * p.Quantity));

            // Task 4.2
            Console.WriteLine( products.Average(p => p.Price));
            Console.WriteLine(products.MaxBy(p => p.Price));
            Console.WriteLine(products.MinBy(p => p.Price));

            // Task 4.3
            Console.WriteLine("\n=== Task 4.3 ===");
            Console.WriteLine(products.Sum(p => p.Quantity));
            Console.WriteLine(
                products.Count(p => p.Price > 10000));

            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.5

            Console.ReadKey();
        }
    }
}