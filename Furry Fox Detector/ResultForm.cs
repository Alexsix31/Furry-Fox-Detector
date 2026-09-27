using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GayFoxFemboyDetector
{
    public class ResultForm : Form
    {
        public ResultForm(TestResult result)
        {
            Text = "🦊 Вердикт Лисьей Академии";
            Width = 720;
            Height = 620;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(38, 22, 12);          // тёмно-коричневый
            ForeColor = Color.FromArgb(255, 230, 200);
            Font = new Font("Segoe UI", 10);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            var lblTitle = new Label
            {
                Text = "🦊 Твой Лисий Профиль 🦊",
                Dock = DockStyle.Top,
                Height = 60,
                Font = new Font("Segoe UI", 17, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 40),    // рыжий
                TextAlign = ContentAlignment.MiddleCenter
            };

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(30, 10, 30, 10),
                BackColor = Color.FromArgb(38, 22, 12)
            };

            // Лисья палитра для шкал — от золотого до медного
            var colors = new Dictionary<string, Color>
            {
                ["Гейность"] = Color.FromArgb(255, 160, 60),
                ["Лисность"] = Color.FromArgb(240, 100, 30),
                ["Фембойность"] = Color.FromArgb(255, 130, 170),
                ["Фырность"] = Color.FromArgb(220, 150, 80),
                ["Пушистость"] = Color.FromArgb(255, 200, 130)
            };

            var emoji = new Dictionary<string, string>
            {
                ["Гейность"] = "🏳️‍🌈",
                ["Лисность"] = "🦊",
                ["Фембойность"] = "🌸",
                ["Фырность"] = "🐾",
                ["Пушистость"] = "☁️"
            };

            int y = 10;
            foreach (var kv in result.Percent.OrderByDescending(k => k.Value))
            {
                string scale = kv.Key;
                double pct = kv.Value;

                var lbl = new Label
                {
                    Text = $"{emoji.GetValueOrDefault(scale, "•")} {scale}: {pct:F1}%",
                    Location = new Point(10, y),
                    Width = 620,
                    Height = 25,
                    Font = new Font("Segoe UI", 11, FontStyle.Bold),
                    ForeColor = colors.GetValueOrDefault(scale, Color.FromArgb(255, 200, 130))
                };
                panel.Controls.Add(lbl);

                var bar = new Panel
                {
                    Location = new Point(10, y + 30),
                    Width = 620,
                    Height = 22,
                    BackColor = Color.FromArgb(60, 35, 20)       // тёмная дорожка
                };

                var fill = new Panel
                {
                    Location = new Point(0, 0),
                    Width = (int)(620 * pct / 100.0),
                    Height = 22,
                    BackColor = colors.GetValueOrDefault(scale, Color.FromArgb(255, 140, 40))
                };
                bar.Controls.Add(fill);
                panel.Controls.Add(bar);

                y += 70;
            }

            var lblVerdict = new Label
            {
                Text = "📜 Вердикт:\n\n" + result.Verdict,
                Location = new Point(10, y + 20),
                Width = 620,
                Height = 120,
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                ForeColor = Color.FromArgb(255, 210, 150),       // тёплый песочный
                TextAlign = ContentAlignment.MiddleLeft
            };
            panel.Controls.Add(lblVerdict);

            var btnClose = new Button
            {
                Text = "Забрать диплом и уйти 🦊",
                Dock = DockStyle.Bottom,
                Height = 55,
                BackColor = Color.FromArgb(230, 110, 30),        // оранжевый акцент
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();

            Controls.Add(panel);
            Controls.Add(btnClose);
            Controls.Add(lblTitle);
        }
    }
}