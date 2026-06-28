using System;
using System.Windows.Forms;

namespace BirthdateConstellaDivination
{
    public partial class Result : Form
    {
        public static string birtheight;
        public static string strusrname;

        public Result()
        {
            InitializeComponent();

            DateTime getdate = DateTime.Now;
            string showNow = getdate.ToString("yyyyMMdd");
            showName.Text = strusrname;

            int materialB = int.Parse(birtheight);
            int materialD = int.Parse(showNow);
            int calresult = materialD - materialB;

            int[] rsarrays = new int[6];
            rsarrays[0] = calresult % 10; calresult /= 10;
            rsarrays[1] = calresult % 10; calresult /= 10;
            rsarrays[2] = calresult % 10; calresult /= 10;
            rsarrays[3] = calresult % 10; calresult /= 10;
            rsarrays[4] = calresult % 10; calresult /= 10;
            rsarrays[5] = calresult % 10;
            Random rsd = new Random();
            int rdkey = rsd.Next(1, 4);
            int lfadj = rsd.Next(2, 7);

            int frlife = rsarrays[5] * rdkey * lfadj;
            int frgold = rsarrays[4] * rdkey;
            int frstdy = rsarrays[3] * rdkey;
            int frlove = rsarrays[2] * rdkey;
            int frwork = rsarrays[1] * rdkey;
            int frpatn = rsarrays[0] * rdkey;
            if (frpatn > 27)
            {
                frpatn /= lfadj;
            }

            if (frlife == 0)
            {
                Random seed1 = new Random(100);
                frlife = seed1.Next(1, 28);
            }
            if (frgold == 0)
            {
                Random seed2 = new Random(300);
                frgold = seed2.Next(1, 28);
            }
            if (frstdy == 0)
            {
                Random seed3 = new Random(600);
                frstdy = seed3.Next(1, 28);
            }
            if (frlove == 0)
            {
                Random seed4 = new Random(400);
                frlove = seed4.Next(1, 28);
            }
            if (frwork == 0)
            {
                Random seed5 = new Random(200);
                frwork = seed5.Next(1, 28);
            }
            if (frpatn == 0)
            {
                Random seed6 = new Random(500);
                frpatn = seed6.Next(1, 28);
            }

            prgLife.Value = frlife;
            prgGold.Value = frgold;
            prgStdy.Value = frstdy;
            prgLove.Value = frlove;
            prgWork.Value = frwork;
            prgPatn.Value = frpatn;

            if (0 <= frlife && frlife <= 4)
            {
                instLife.Text = "危ないかも。体調に気をつけよう。";
            }
            else if (5 <= frlife && frlife <= 9)
            {
                instLife.Text = "油断は禁物。お気をつけて。";
            }
            else if (10 <= frlife && frlife <= 14)
            {
                instLife.Text = "そこそこ。今日一日は何とかなりそう。";
            }
            else if (15 <= frlife && frlife <= 19)
            {
                instLife.Text = "割といい。快適に過ごせそう。";
            }
            else if (20 <= frlife && frlife <= 24)
            {
                instLife.Text = "かなりいい。すごく快適に過ごせそう。";
            }
            else if (25 <= frlife && frlife <= 27)
            {
                instLife.Text = "最高！明日も楽しく過ごせるかも。";
            }

            if (0 <= frgold && frgold <= 4)
            {
                instGold.Text = "かなりの損失になりそう。";
            }
            else if (5 <= frgold && frgold <= 9)
            {
                instGold.Text = "ちょっとツイてないかも。";
            }
            else if (10 <= frgold && frgold <= 14)
            {
                instGold.Text = "いつもと同じくらいかな。";
            }
            else if (15 <= frgold && frgold <= 19)
            {
                instGold.Text = "ちょっとラッキーかも。";
            }
            else if (20 <= frgold && frgold <= 24)
            {
                instGold.Text = "いいね、今日はツイてる！";
            }
            else if (25 <= frgold && frgold <= 27)
            {
                instGold.Text = "最高！億万長者にだってなれちゃうよ！";
            }

            if (0 <= frstdy && frstdy <= 4)
            {
                instStdy.Text = "内容があまり頭に入ってこなさそう。";
            }
            else if (5 <= frstdy && frstdy <= 9)
            {
                instStdy.Text = "忘れっぽくなるかも。";
            }
            else if (10 <= frstdy && frstdy <= 14)
            {
                instStdy.Text = "そこそこ。テストで平均点は取れるかも。";
            }
            else if (15 <= frstdy && frstdy <= 19)
            {
                instStdy.Text = "平均以上の成績を出せそう。";
            }
            else if (20 <= frstdy && frstdy <= 24)
            {
                instStdy.Text = "知識が大きく広がるかも。";
            }
            else if (25 <= frstdy && frstdy <= 27)
            {
                instStdy.Text = "範囲丸暗記間違いなし！";
            }

            if (0 <= frlove && frlove <= 4)
            {
                instLove.Text = "2人の間の雰囲気に気をつけて。";
            }
            else if (5 <= frlove && frlove <= 9)
            {
                instLove.Text = "うっかり失言してしまうことも？";
            }
            else if (10 <= frlove && frlove <= 14)
            {
                instLove.Text = "関係は進展なしかも。";
            }
            else if (15 <= frlove && frlove <= 19)
            {
                instLove.Text = "いい雰囲気にできるかも。";
            }
            else if (20 <= frlove && frlove <= 24)
            {
                instLove.Text = "思い切ってプロポーズ！";
            }
            else if (25 <= frlove && frlove <= 27)
            {
                instLove.Text = "すごいね！もう結婚できるかも！";
            }

            if (0 <= frwork && frwork <= 4)
            {
                instWork.Text = "ミスを連発しちゃうかも。";
            }
            else if (5 <= frwork && frwork <= 9)
            {
                instWork.Text = "重要なことを忘れちゃう！？";
            }
            else if (10 <= frwork && frwork <= 14)
            {
                instWork.Text = "いつもどおりの日々をおくれそう。";
            }
            else if (15 <= frwork && frwork <= 19)
            {
                instWork.Text = "上司に怒られなくなるかも。";
            }
            else if (20 <= frwork && frwork <= 24)
            {
                instWork.Text = "いい仕事ができそう。";
            }
            else if (25 <= frwork && frwork <= 27)
            {
                instWork.Text = "いいね！昇進間違いなし！";
            }

            if (0 <= frpatn && frpatn <= 4)
            {
                instPatn.Text = "ギクシャクした一日になりそう。";
            }
            else if (5 <= frpatn && frpatn <= 9)
            {
                instPatn.Text = "話題が見つからないかも。";
            }
            else if (10 <= frpatn && frpatn <= 14)
            {
                instPatn.Text = "友達とは友達のまま。";
            }
            else if (15 <= frpatn && frpatn <= 19)
            {
                instPatn.Text = "相手が自分を見直してくれるかも。";
            }
            else if (20 <= frpatn && frpatn <= 24)
            {
                instPatn.Text = "話題をさらって目立ちやすい？";
            }
            else if (25 <= frpatn && frpatn <= 27)
            {
                instPatn.Text = "すごい！みんなの人気者だね！";
            }

            Random lcitem = new Random();
            int luckyitem = lcitem.Next(1, 10);

            if (1 == luckyitem)
            {
                whatLucky.Text = "真珠";
            }
            else if (2 <= luckyitem && luckyitem <= 4)
            {
                whatLucky.Text = "地球儀";
            }
            else if (5 <= luckyitem && luckyitem <= 7)
            {
                whatLucky.Text = "御守り";
            }
            else if (8 <= luckyitem && luckyitem <= 10)
            {
                whatLucky.Text = "レジャーシート";
            }

            int isMax3 = 0;
            if (frlife == 27)
            {
                isMax3 += 1;
            }
            if (frgold == 27)
            {
                isMax3 += 1;
            }
            if (frstdy == 27)
            {
                isMax3 += 1;
            }
            if (frlove == 27)
            {
                isMax3 += 1;
            }
            if (frwork == 27)
            {
                isMax3 += 1;
            }
            if (frpatn == 27)
            {
                isMax3 += 1;
            }

            if (isMax3 >= 3)
            {
                _ = MessageBox.Show("今日はすごく幸せな一日になりそうです！", "おめでとうございます", MessageBoxButtons.OK);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}