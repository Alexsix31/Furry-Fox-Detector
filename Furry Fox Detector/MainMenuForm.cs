using System;
using System.Drawing;
using System.Windows.Forms;

namespace GayFoxFemboyDetector
{
    public class MainMenuForm : Form
    {
        private CheckBox chkGay;
        private Button btnEnter;
        private Label lblTitle, lblSubtitle, lblWarning;
        private int warningStage = 0;

        public MainMenuForm()
        {
            Text = "🦊 Gay Fox Femboy Detector v1.0";
            Width = 640;
            Height = 520;
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(38, 22, 12);          // тёмно-коричневый
            ForeColor = Color.FromArgb(255, 230, 200);       // кремовый
            Font = new Font("Segoe UI", 10);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            lblTitle = new Label
            {
                Text = "🦊 Gay Fox Femboy Detector 🦊",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 140, 40),    // рыжий
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 70
            };

            lblSubtitle = new Label
            {
                Text = "Официальный тест Лисьей Академии Фурри-Фембойства\n" +
                       "имени Пушистого Хвоста\n\n" +
                       "Отвечай честно. Лисы чувствуют ложь.",
                Font = new Font("Segoe UI", 11, FontStyle.Italic),
                ForeColor = Color.FromArgb(255, 200, 150),   // светло-персиковый
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 100
            };

            chkGay = new CheckBox
            {
                Text = "Я гей? (обязательное подтверждение)",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 180, 90),    // золотистый
                AutoSize = true,
                Location = new Point(180, 260)
            };

            btnEnter = new Button
            {
                Text = "ВОЙТИ В ЛИСЬЮ АКАДЕМИЮ",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(230, 110, 30),    // насыщенный оранжевый
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Width = 300,
                Height = 60,
                Location = new Point((Width - 300) / 2 - 8, 320)
            };
            btnEnter.FlatAppearance.BorderSize = 0;
            btnEnter.Click += BtnEnter_Click;

            lblWarning = new Label
            {
                Text = "",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.FromArgb(255, 90, 60),     // красновато-оранжевый
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 80
            };

            Controls.Add(lblWarning);
            Controls.Add(btnEnter);
            Controls.Add(chkGay);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
        }

        private void BtnEnter_Click(object sender, EventArgs e)
        {
            if (!chkGay.Checked)
            {
                warningStage++;
                string msg = warningStage switch
                {
                    1 => "⚠️ СТОП. Ты не отметил галочку «Я гей?».\nЛисья Академия не пропускает без подтверждения.",
                    2 => "🦊 Эй, пушистик, я серьёзно. Галочку поставь.\nИначе хвост не активируется.",
                    3 => "😾 Это третье предупреждение.\nЕщё раз - и тебе выдадут кошачьи ушки вместо лисьих.",
                    _ => "💢 Всьо, я устал. Вот тебе особая форма.\nОтметь галочку или иди отсюда."
                };
                lblWarning.Text = msg;
                var t = new System.Windows.Forms.Timer { Interval = 40 };
                int count = 0;
                var orig = btnEnter.Location;
                t.Tick += (s, ea) =>
                {
                    btnEnter.Left = orig.X + (count % 2 == 0 ? 8 : -8);
                    count++;
                    if (count > 8) { btnEnter.Left = orig.X; t.Stop(); }
                };
                t.Start();
                return;
            }

            Hide();
            using (var test = new TestForm())
            {
                test.ShowDialog();
            }
            Close();
        }
    }
}