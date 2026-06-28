using System;
using System.Windows.Forms;
using BirthdateConstellaDivination.Fortune;

namespace BirthdateConstellaDivination
{
    public partial class FirstWindow : Form
    {
        public FirstWindow()
        {
            InitializeComponent();
        }

        private void StartFunction_Click(object sender, EventArgs e)
        {
            if (!BirthdateParser.TryParse(inputBirth.Text, out var birthdate, out var birthError))
            {
                // TryParse の契約: false を返すときは birthError が必ず設定されている
                _ = MessageBox.Show(
                    BirthdateParser.DescribeError(birthError!.Value),
                    "エラー",
                    MessageBoxButtons.OK);
                return;
            }

            if (string.IsNullOrEmpty(inputName.Text))
            {
                _ = MessageBox.Show("名前を入力してください。", "エラー", MessageBoxButtons.OK);
                return;
            }

            using Result f2 = new Result(birthdate, inputName.Text);
            _ = f2.ShowDialog(this);
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
