using System.Collections.Generic;

namespace GayFoxFemboyDetector
{
    public class Answer
    {
        public string Text { get; set; }
        // ключ — название шкалы, значение — сколько очков даёт
        public Dictionary<string, int> Scores { get; set; } = new Dictionary<string, int>();
    }

    public class Question
    {
        public string Text { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }

    public class TestResult
    {
        public Dictionary<string, double> Percent { get; set; } = new Dictionary<string, double>();
        public Dictionary<string, int> Raw { get; set; } = new Dictionary<string, int>();
        public Dictionary<string, int> Max { get; set; } = new Dictionary<string, int>();
        public string Verdict { get; set; }
    }
}