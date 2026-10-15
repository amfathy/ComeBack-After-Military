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
            var numbers = new List<int> { 5, 3, 9, 1, 7, 3, 8 };
            var empty = new List<Question>();
            var oneItem = new List<int> { 42 };

            // ===================== PART 1: ElementAt / ElementAtOrDefault =====================
            // TODO: Task 1.1 ... 1.6
            // task 1.1 
            var QuestionAt10 = questions.ElementAt(10);
            Console.WriteLine(QuestionAt10);

            // Task 1.3
            try
            {
                questions.ElementAt(100);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            }

            //Task1.4 
            var QuestionAT245 = questions.ElementAtOrDefault(245);
            Console.WriteLine(QuestionAT245);

            // ===================== PART 2: First / FirstOrDefault =====================
            // TODO: Task 2.1 ... 2.7
            // Task 2.1
            Console.WriteLine(questions.First().Title);

            // Task 2.2
            Console.WriteLine(questions.First(q => q.Choices.Count == 3).Title);

            // Task 2.3
            try
            {
                questions.First(q => q.Title.Contains("blockchain"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
            }

            // ===================== PART 3: Last / LastOrDefault =====================
            // TODO: Task 3.1 ... 3.5
            // Task 3.1
            Console.WriteLine(questions.Last().Title);

            // Task 3.2
            Console.WriteLine(questions.Last(q => q.Choices.Count == 2).Title);

            // Task 3.3
            Console.WriteLine(questions.Last(q => q.CorrectAnswer != 1).Title);

            // ===================== PART 4: Single / SingleOrDefault =====================
            // TODO: Task 4.1 ... 4.7


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.5

            Console.ReadKey();
        }
    }
}