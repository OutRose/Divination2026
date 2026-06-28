using System;
using System.Windows.Forms;

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
            bool birthOk;
            if (inputBirth.Text == "")
            {
                _ = MessageBox.Show("誕生日を入力してください。", "エラー", MessageBoxButtons.OK);
                birthOk = false;
            }
            else
            {
                birthOk = true;
                Result.birtheight = inputBirth.Text;
            }

            bool nameOk;
            if (inputName.Text == "")
            {
                _ = MessageBox.Show("名前を入力してください。", "エラー", MessageBoxButtons.OK);
                nameOk = false;
            }
            else
            {
                nameOk = true;
                Result.strusrname = inputName.Text;
            }

            if (birthOk == true && nameOk == true)
            {
                using Result f2 = new Result();
                _ = f2.ShowDialog(this);
            }
        }

        private void ButtonExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}