using System;
using System.Windows.Forms;
using BirthdateConstellaDivination.Fortune;

namespace BirthdateConstellaDivination
{
    public partial class Result : Form
    {
        public Result(DateTime birthdate, string userName)
        {
            InitializeComponent();

            var fortune = FortuneCalculator.Calculate(birthdate, DateTime.Now, new Random());

            showName.Text = userName;
            AssignScore(prgLife, instLife, LuckCategory.Life,    fortune.Life);
            AssignScore(prgGold, instGold, LuckCategory.Gold,    fortune.Gold);
            AssignScore(prgStdy, instStdy, LuckCategory.Study,   fortune.Study);
            AssignScore(prgLove, instLove, LuckCategory.Love,    fortune.Love);
            AssignScore(prgWork, instWork, LuckCategory.Work,    fortune.Work);
            AssignScore(prgPatn, instPatn, LuckCategory.Pattern, fortune.Pattern);

            whatLucky.Text = LuckyItemSelector.GetName(LuckyItemSelector.Select(new Random()));

            if (fortune.IsSuperLucky)
            {
                _ = MessageBox.Show(
                    "今日はすごく幸せな一日になりそうです！",
                    "おめでとうございます",
                    MessageBoxButtons.OK);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Life スコアは元設計で 27 を超えうる (digits[5]*rdkey*lfadj、最大 9*3*6=162) が
        // ProgressBar.Maximum=27 のため Math.Min でキャップ。
        // 元実装ではここで ArgumentOutOfRangeException がスローされうる潜在バグだった。
        // ランクメッセージ側は LuckRankClassifier の `_ => Highest` で 27 超もすでに graceful に扱う。
        private static void AssignScore(ProgressBar progress, Label instructionLabel, LuckCategory category, int score)
        {
            progress.Value = Math.Min(score, progress.Maximum);
            instructionLabel.Text = LuckRankClassifier.GetMessageForScore(category, score);
        }
    }
}
