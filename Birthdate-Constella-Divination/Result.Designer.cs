using System.Windows.Forms;

namespace BirthdateConstellaDivination
{
    partial class Result
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.showName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.prgLife = new System.Windows.Forms.ProgressBar();
            this.prgGold = new System.Windows.Forms.ProgressBar();
            this.prgStdy = new System.Windows.Forms.ProgressBar();
            this.prgLove = new System.Windows.Forms.ProgressBar();
            this.prgWork = new System.Windows.Forms.ProgressBar();
            this.prgPatn = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.instLife = new System.Windows.Forms.Label();
            this.instGold = new System.Windows.Forms.Label();
            this.instStdy = new System.Windows.Forms.Label();
            this.instPatn = new System.Windows.Forms.Label();
            this.instLove = new System.Windows.Forms.Label();
            this.instWork = new System.Windows.Forms.Label();
            this.labelLuckIs = new System.Windows.Forms.Label();
            this.whatLucky = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // showName
            // 
            this.showName.AutoSize = true;
            this.showName.Font = new System.Drawing.Font("HGPｺﾞｼｯｸE", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.showName.Location = new System.Drawing.Point(14, 13);
            this.showName.Name = "showName";
            this.showName.Size = new System.Drawing.Size(86, 19);
            this.showName.TabIndex = 0;
            this.showName.Text = "the Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(404, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 19);
            this.label2.TabIndex = 1;
            this.label2.Text = "さんの今日の運勢は…!";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(13, 74);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "健康運";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(32, 135);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 19);
            this.label3.TabIndex = 3;
            this.label3.Text = "金運";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label4.Location = new System.Drawing.Point(12, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 19);
            this.label4.TabIndex = 4;
            this.label4.Text = "勉強運";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label5.Location = new System.Drawing.Point(13, 265);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 19);
            this.label5.TabIndex = 5;
            this.label5.Text = "恋愛運";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label6.Location = new System.Drawing.Point(13, 329);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 19);
            this.label6.TabIndex = 6;
            this.label6.Text = "仕事運";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label7.Location = new System.Drawing.Point(13, 394);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(66, 19);
            this.label7.TabIndex = 7;
            this.label7.Text = "対人運";
            // 
            // prgLife
            // 
            this.prgLife.Location = new System.Drawing.Point(109, 69);
            this.prgLife.Maximum = 27;
            this.prgLife.Name = "prgLife";
            this.prgLife.Size = new System.Drawing.Size(289, 23);
            this.prgLife.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgLife.TabIndex = 8;
            // 
            // prgGold
            // 
            this.prgGold.Location = new System.Drawing.Point(109, 131);
            this.prgGold.Maximum = 27;
            this.prgGold.Name = "prgGold";
            this.prgGold.Size = new System.Drawing.Size(289, 23);
            this.prgGold.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgGold.TabIndex = 9;
            // 
            // prgStdy
            // 
            this.prgStdy.Location = new System.Drawing.Point(109, 192);
            this.prgStdy.Maximum = 27;
            this.prgStdy.Name = "prgStdy";
            this.prgStdy.Size = new System.Drawing.Size(289, 23);
            this.prgStdy.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgStdy.TabIndex = 10;
            // 
            // prgLove
            // 
            this.prgLove.Location = new System.Drawing.Point(109, 261);
            this.prgLove.Maximum = 27;
            this.prgLove.Name = "prgLove";
            this.prgLove.Size = new System.Drawing.Size(289, 23);
            this.prgLove.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgLove.TabIndex = 11;
            // 
            // prgWork
            // 
            this.prgWork.Location = new System.Drawing.Point(109, 329);
            this.prgWork.Maximum = 27;
            this.prgWork.Name = "prgWork";
            this.prgWork.Size = new System.Drawing.Size(289, 23);
            this.prgWork.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgWork.TabIndex = 12;
            // 
            // prgPatn
            // 
            this.prgPatn.Location = new System.Drawing.Point(109, 390);
            this.prgPatn.Maximum = 27;
            this.prgPatn.Name = "prgPatn";
            this.prgPatn.Size = new System.Drawing.Size(289, 23);
            this.prgPatn.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prgPatn.TabIndex = 13;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(656, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 24);
            this.button1.TabIndex = 14;
            this.button1.Text = "閉じる";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.Button1_Click);
            // 
            // instLife
            // 
            this.instLife.AutoSize = true;
            this.instLife.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.instLife.Location = new System.Drawing.Point(404, 73);
            this.instLife.Name = "instLife";
            this.instLife.Size = new System.Drawing.Size(58, 19);
            this.instLife.TabIndex = 15;
            this.instLife.Text = "解説1";
            // 
            // instGold
            // 
            this.instGold.AutoSize = true;
            this.instGold.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.instGold.Location = new System.Drawing.Point(404, 135);
            this.instGold.Name = "instGold";
            this.instGold.Size = new System.Drawing.Size(58, 19);
            this.instGold.TabIndex = 16;
            this.instGold.Text = "解説2";
            // 
            // instStdy
            // 
            this.instStdy.AutoSize = true;
            this.instStdy.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.instStdy.Location = new System.Drawing.Point(404, 196);
            this.instStdy.Name = "instStdy";
            this.instStdy.Size = new System.Drawing.Size(58, 19);
            this.instStdy.TabIndex = 17;
            this.instStdy.Text = "解説3";
            // 
            // instPatn
            // 
            this.instPatn.AutoSize = true;
            this.instPatn.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.instPatn.Location = new System.Drawing.Point(404, 394);
            this.instPatn.Name = "instPatn";
            this.instPatn.Size = new System.Drawing.Size(58, 19);
            this.instPatn.TabIndex = 20;
            this.instPatn.Text = "解説6";
            // 
            // instLove
            // 
            this.instLove.AutoSize = true;
            this.instLove.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.instLove.Location = new System.Drawing.Point(404, 265);
            this.instLove.Name = "instLove";
            this.instLove.Size = new System.Drawing.Size(58, 19);
            this.instLove.TabIndex = 21;
            this.instLove.Text = "解説4";
            // 
            // instWork
            // 
            this.instWork.AutoSize = true;
            this.instWork.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.instWork.Location = new System.Drawing.Point(404, 333);
            this.instWork.Name = "instWork";
            this.instWork.Size = new System.Drawing.Size(58, 19);
            this.instWork.TabIndex = 22;
            this.instWork.Text = "解説5";
            // 
            // labelLuckIs
            // 
            this.labelLuckIs.AutoSize = true;
            this.labelLuckIs.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.labelLuckIs.Location = new System.Drawing.Point(14, 469);
            this.labelLuckIs.Name = "labelLuckIs";
            this.labelLuckIs.Size = new System.Drawing.Size(406, 21);
            this.labelLuckIs.TabIndex = 23;
            this.labelLuckIs.Text = "今日のあなたのラッキーアイテムは？→\r\n";
            // 
            // whatLucky
            // 
            this.whatLucky.AutoSize = true;
            this.whatLucky.Font = new System.Drawing.Font("HGSｺﾞｼｯｸM", 15.75F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.whatLucky.ForeColor = System.Drawing.Color.Red;
            this.whatLucky.Location = new System.Drawing.Point(447, 469);
            this.whatLucky.Name = "whatLucky";
            this.whatLucky.Size = new System.Drawing.Size(178, 21);
            this.whatLucky.TabIndex = 24;
            this.whatLucky.Text = "ここに表示される";
            // 
            // Result
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(777, 532);
            this.Controls.Add(this.whatLucky);
            this.Controls.Add(this.labelLuckIs);
            this.Controls.Add(this.instWork);
            this.Controls.Add(this.instLove);
            this.Controls.Add(this.instPatn);
            this.Controls.Add(this.instStdy);
            this.Controls.Add(this.instGold);
            this.Controls.Add(this.instLife);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.prgPatn);
            this.Controls.Add(this.prgWork);
            this.Controls.Add(this.prgLove);
            this.Controls.Add(this.prgStdy);
            this.Controls.Add(this.prgGold);
            this.Controls.Add(this.prgLife);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.showName);
            this.Name = "Result";
            this.Text = "Result";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label showName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar prgLife;
        private System.Windows.Forms.ProgressBar prgGold;
        private System.Windows.Forms.ProgressBar prgStdy;
        private System.Windows.Forms.ProgressBar prgLove;
        private System.Windows.Forms.ProgressBar prgWork;
        private System.Windows.Forms.ProgressBar prgPatn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label instLife;
        private System.Windows.Forms.Label instGold;
        private System.Windows.Forms.Label instStdy;
        private System.Windows.Forms.Label instPatn;
        private Label instLove;
        private Label instWork;
        private Label labelLuckIs;
        private Label whatLucky;
    }
}