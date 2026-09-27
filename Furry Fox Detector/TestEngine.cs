using System;
using System.Collections.Generic;

namespace GayFoxFemboyDetector
{
    public class TestEngine
    {
        private readonly List<Question> _questions;
        private readonly List<Answer> _answers = new List<Answer>();
        private readonly Random _rng = new Random();

        public int CurrentIndex { get; private set; } = 0;

        public TestEngine(List<Question> questions)
        {
            // отделяем последний вопрос шаблона — он должен остаться в конце
            var last = questions[questions.Count - 1];
            var rest = new List<Question>(questions);
            rest.RemoveAt(rest.Count - 1);

            // перемешиваем всё, кроме последнего
            _questions = Shuffle(rest, _rng);
            _questions.Add(last);

            // перемешиваем ответы внутри каждого
            foreach (var q in _questions)
                q.Answers = Shuffle(q.Answers, _rng);
        }

        private static List<T> Shuffle<T>(List<T> source, Random rng)
        {
            var list = new List<T>(source);
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
            return list;
        }

        public Question Current => _questions[CurrentIndex];
        public bool IsLast => CurrentIndex == _questions.Count - 1;
        public bool CanGoBack => CurrentIndex > 0;
        public int Total => _questions.Count;
        public int AnsweredCount => _answers.Count;
        public Answer GetAnswerAt(int index) => index < _answers.Count ? _answers[index] : null;

        public void SaveAnswer(Answer a)
        {
            if (CurrentIndex < _answers.Count) _answers[CurrentIndex] = a;
            else _answers.Add(a);
        }

        public void MoveNext() { if (!IsLast) CurrentIndex++; }
        public void MoveBack() { if (CanGoBack) CurrentIndex--; }

        public TestResult Calculate()
        {
            var raw = new Dictionary<string, int>();
            var max = new Dictionary<string, int>();

            max.Clear();
            foreach (var q in _questions)
            {
                var bestPerScale = new Dictionary<string, int>();
                foreach (var a in q.Answers)
                    foreach (var kv in a.Scores)
                        if (!bestPerScale.ContainsKey(kv.Key) || bestPerScale[kv.Key] < kv.Value)
                            bestPerScale[kv.Key] = kv.Value;

                foreach (var kv in bestPerScale)
                    max[kv.Key] = max.ContainsKey(kv.Key) ? max[kv.Key] + kv.Value : kv.Value;
            }

            foreach (var a in _answers)
            {
                if (a == null) continue;
                foreach (var kv in a.Scores)
                    raw[kv.Key] = raw.ContainsKey(kv.Key) ? raw[kv.Key] + kv.Value : kv.Value;
            }

            var percent = new Dictionary<string, double>();
            foreach (var kv in raw)
            {
                int m = max.ContainsKey(kv.Key) && max[kv.Key] > 0 ? max[kv.Key] : 1;
                percent[kv.Key] = 100.0 * kv.Value / m;
            }

            foreach (var kv in max)
                if (!percent.ContainsKey(kv.Key)) percent[kv.Key] = 0;

            return new TestResult
            {
                Raw = raw,
                Max = max,
                Percent = percent,
                Verdict = BuildVerdict(percent)
            };
        }

        private string BuildVerdict(Dictionary<string, double> p)
        {
            if (p.Count == 0) return "🥚 Фыр? Пусто. Попробуй пройти тест.";

            string topScale = null;
            double topValue = -1;
            foreach (var kv in p)
            {
                if (kv.Value > topValue) { topValue = kv.Value; topScale = kv.Key; }
            }

            double avg = 0;
            foreach (var kv in p) avg += kv.Value;
            avg /= p.Count;

            double dominance = topValue - avg;

            string title = topScale switch
            {
                "Гейность" => "🏳️‍🌈 Гей",
                "Лисность" => "🦊 Лис",
                "Фембойность" => "🌸 Фембой",
                "Фырность" => "🐾 Фыр-фурри",
                "Пушистость" => "☁️ Пушистик",
                _ => "🦊 Неизвестный зверь"
            };

            string strength;
            if (topValue >= 95) strength = "Абсолютный";
            else if (topValue >= 85) strength = "Легендарный";
            else if (topValue >= 70) strength = "Настоящий";
            else if (topValue >= 55) strength = "Уверенный";
            else if (topValue >= 40) strength = "Растущий";
            else if (topValue >= 25) strength = "Начинающий";
            else strength = "Сомнительный";

            string extra;
            if (dominance >= 30)
                extra = "и это твоя явная сторона. Остальные шкалы отдыхают. Фыр🦊";
            else if (dominance >= 15)
                extra = "с небольшим уклоном в остальные пушистости. Фыр.";
            else
                extra = "но в тебе намешано много всего - настоящий универсал. Фыр-фыр.";

            if (topValue < 25)
                return "🥚 Фыр? Ты - Пока Ещё Не Лис. Даже ведущая шкала еле дышит. Начни с плюшевого хвоста и возвращайся. 🦊";

            return $"{title} {strength.ToLower()}: {topValue:F1}% по шкале «{topScale}» - {extra}";
        }
    }
}