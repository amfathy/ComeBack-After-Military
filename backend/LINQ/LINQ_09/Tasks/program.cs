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
            // task 1 : 
            Enumerable.Range(1, 10).Print("Task1.1");

            // task 6 : 
            int sum = Enumerable.Range(1, 100).Sum();


            // ===================== PART 2: Range with the question bank =====================
            // TODO: Task 2.1 ... 2.4
            // task 2.1
            QuestionBank.GetQuestionRange(Enumerable.Range(1, 5)).ToQuiz();


            // ===================== PART 3: Repeat =====================
            // TODO: Task 3.1 ... 3.6
            //task 1 : 
            string stars = string.Concat(Enumerable.Repeat("*", 10));

            // Task 3.4:
            var q = new Question
            {
                Title = "Original question",
                Choices = new List<Choice>(),
                CorrectAnswer = 1
            };

            var repeatedQuestions = Enumerable.Repeat(q, 10).ToList();
            // ===================== PART 4: Empty and DefaultIfEmpty =====================
            // TODO: Task 4.1 ... 4.6


            // ===================== PART 5: Mixed =====================
            // TODO: Task 5.1 ... 5.5

            Console.ReadKey();
        }
    }
}