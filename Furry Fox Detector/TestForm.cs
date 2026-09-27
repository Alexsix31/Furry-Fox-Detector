using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GayFoxFemboyDetector
{
    public class TestForm : Form
    {
        private TestEngine _engine;
        private Label lblProgress, lblQuestion;
        private FlowLayoutPanel panelAnswers;
        private Button btnNext, btnBack, btnAbort;

        public TestForm()
        {
            _engine = new TestEngine(Questions.All);

            Text = "🦊 Лисья Академия - Тестирование";
            Width = 800;
            Height = 620;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(38, 22, 12);
            ForeColor = Color.FromArgb(255, 230, 200);
            Font = new Font("Segoe UI", 11);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            lblProgress = new Label
            {
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(255, 190, 130),
                TextAlign = ContentAlignment.MiddleCenter
            };

            lblQuestion = new Label
            {
                Dock = DockStyle.Top,
                Height = 130,
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 220, 170),
                TextAlign = ContentAlignment.MiddleCenter,
                Padding = new Padding(10)
            };

            panelAnswers = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoScroll = true,
                Padding = new Padding(40, 10, 40, 10),
                BackColor = Color.FromArgb(48, 28, 15)
            };

            var bottom = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 70,
                BackColor = Color.FromArgb(28, 16, 8)
            };

            btnBack = MakeButton("◀ Назад", 120, Color.FromArgb(120, 70, 35));
            btnBack.Location = new Point(20, 12);
            btnBack.Click += BtnBack_Click;

            btnAbort = MakeButton("Сбежать 🦊💨", 130, Color.FromArgb(90, 55, 25));
            btnAbort.Location = new Point(150, 12);
            btnAbort.Click += (s, e) =>
            {
                if (MessageBox.Show("Уверен? Лисы не любят беглецов.",
                    "Побег?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    Close();
            };

            btnNext = MakeButton("Далее ▶", 180, Color.FromArgb(230, 110, 30));
            btnNext.Click += BtnNext_Click;

            bottom.Controls.Add(btnNext);
            bottom.Controls.Add(btnBack);
            bottom.Controls.Add(btnAbort);

            bottom.Resize += (s, e) =>
            {
                btnNext.Location = new Point(bottom.Width - btnNext.Width - 20, 12);
            };

            Controls.Add(panelAnswers);
            Controls.Add(bottom);
            Controls.Add(lblQuestion);
            Controls.Add(lblProgress);

            Shown += (s, e) =>
            {
                btnNext.Location = new Point(bottom.Width - btnNext.Width - 20, 12);
                RenderQuestion();
            };
        }

        private Button MakeButton(string text, int width, Color back)
        {
            var b = new Button
            {
                Text = text,
                Width = width,
                Height = 45,
                BackColor = back,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            b.FlatAppearance.BorderSize = 0;
            return b;
        }

        private void RenderQuestion()
        {
            var q = _engine.Current;
            lblProgress.Text = $"Вопрос {_engine.CurrentIndex + 1} из {_engine.Total}";
            lblQuestion.Text = q.Text;

            panelAnswers.SuspendLayout();
            panelAnswers.Controls.Clear();

            var prev = _engine.GetAnswerAt(_engine.CurrentIndex);

            foreach (var a in q.Answers)
            {
                var rb = new RadioButton
                {
                    Text = a.Text,
                    AutoSize = true,
                    MaximumSize = new Size(680, 0),
                    Font = new Font("Segoe UI", 11),
                    ForeColor = Color.FromArgb(255, 225, 185),
                    Tag = a,
                    Padding = new Padding(5),
                    Margin = new Padding(5, 8, 5, 8),
                    Checked = ReferenceEquals(prev, a)
                };
                panelAnswers.Controls.Add(rb);
            }

            panelAnswers.ResumeLayout();
            btnBack.Enabled = _engine.CanGoBack;
            btnNext.Text = _engine.IsLast ? "ЗАВЕРШИТЬ 🦊" : "Далее ▶";
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            _engine.MoveBack();
            RenderQuestion();
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            var selected = panelAnswers.Controls.OfType<RadioButton>()
                                               .FirstOrDefault(r => r.Checked);
            if (selected == null)
            {
                MessageBox.Show("Выбери ответ, пушистик. Молчание - тоже ответ, но не в этом тесте.",
                    "🦊 Эй", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _engine.SaveAnswer((Answer)selected.Tag);

            if (_engine.IsLast)
            {
                var result = _engine.Calculate();
                Hide();
                using (var rf = new ResultForm(result))
                    rf.ShowDialog();
                Close();
            }
            else
            {
                _engine.MoveNext();
                RenderQuestion();
            }
        }
    }
}