namespace ksmith70DungeonFinalProject
{
    partial class GameScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameScreen));
            this.heroPb1 = new System.Windows.Forms.PictureBox();
            this.heroPb2 = new System.Windows.Forms.PictureBox();
            this.heroPb3 = new System.Windows.Forms.PictureBox();
            this.enemyPb3 = new System.Windows.Forms.PictureBox();
            this.enemyPb1 = new System.Windows.Forms.PictureBox();
            this.enemyPb2 = new System.Windows.Forms.PictureBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.attackBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.specialBtn = new System.Windows.Forms.Button();
            this.defendBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.heroPb1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heroPb2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.heroPb3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemyPb3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemyPb1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemyPb2)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // heroPb1
            // 
            this.heroPb1.BackColor = System.Drawing.Color.Transparent;
            this.heroPb1.Image = ((System.Drawing.Image)(resources.GetObject("heroPb1.Image")));
            this.heroPb1.Location = new System.Drawing.Point(117, 107);
            this.heroPb1.Name = "heroPb1";
            this.heroPb1.Size = new System.Drawing.Size(60, 50);
            this.heroPb1.TabIndex = 0;
            this.heroPb1.TabStop = false;
            this.heroPb1.Tag = "1";
            this.heroPb1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // heroPb2
            // 
            this.heroPb2.BackColor = System.Drawing.Color.Transparent;
            this.heroPb2.Image = ((System.Drawing.Image)(resources.GetObject("heroPb2.Image")));
            this.heroPb2.Location = new System.Drawing.Point(57, 51);
            this.heroPb2.Name = "heroPb2";
            this.heroPb2.Size = new System.Drawing.Size(60, 50);
            this.heroPb2.TabIndex = 1;
            this.heroPb2.TabStop = false;
            this.heroPb2.Tag = "2";
            // 
            // heroPb3
            // 
            this.heroPb3.BackColor = System.Drawing.Color.Transparent;
            this.heroPb3.Image = ((System.Drawing.Image)(resources.GetObject("heroPb3.Image")));
            this.heroPb3.Location = new System.Drawing.Point(57, 161);
            this.heroPb3.Name = "heroPb3";
            this.heroPb3.Size = new System.Drawing.Size(60, 50);
            this.heroPb3.TabIndex = 2;
            this.heroPb3.TabStop = false;
            this.heroPb3.Tag = "3";
            // 
            // enemyPb3
            // 
            this.enemyPb3.BackColor = System.Drawing.Color.Transparent;
            this.enemyPb3.Image = ((System.Drawing.Image)(resources.GetObject("enemyPb3.Image")));
            this.enemyPb3.Location = new System.Drawing.Point(590, 138);
            this.enemyPb3.Name = "enemyPb3";
            this.enemyPb3.Size = new System.Drawing.Size(60, 50);
            this.enemyPb3.TabIndex = 3;
            this.enemyPb3.TabStop = false;
            this.enemyPb3.Tag = "3";
            this.enemyPb3.Click += new System.EventHandler(this.pictureBox4_Click);
            // 
            // enemyPb1
            // 
            this.enemyPb1.BackColor = System.Drawing.Color.Transparent;
            this.enemyPb1.Image = ((System.Drawing.Image)(resources.GetObject("enemyPb1.Image")));
            this.enemyPb1.Location = new System.Drawing.Point(524, 85);
            this.enemyPb1.Name = "enemyPb1";
            this.enemyPb1.Size = new System.Drawing.Size(60, 50);
            this.enemyPb1.TabIndex = 4;
            this.enemyPb1.TabStop = false;
            this.enemyPb1.Tag = "1";
            // 
            // enemyPb2
            // 
            this.enemyPb2.BackColor = System.Drawing.Color.Transparent;
            this.enemyPb2.Image = ((System.Drawing.Image)(resources.GetObject("enemyPb2.Image")));
            this.enemyPb2.Location = new System.Drawing.Point(590, 41);
            this.enemyPb2.Name = "enemyPb2";
            this.enemyPb2.Size = new System.Drawing.Size(60, 50);
            this.enemyPb2.TabIndex = 5;
            this.enemyPb2.TabStop = false;
            this.enemyPb2.Tag = "2";
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(513, 277);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(275, 161);
            this.textBox1.TabIndex = 6;
            // 
            // attackBtn
            // 
            this.attackBtn.Location = new System.Drawing.Point(19, 3);
            this.attackBtn.Name = "attackBtn";
            this.attackBtn.Size = new System.Drawing.Size(163, 23);
            this.attackBtn.TabIndex = 7;
            this.attackBtn.Text = "Attack";
            this.attackBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.attackBtn.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.specialBtn);
            this.panel1.Controls.Add(this.defendBtn);
            this.panel1.Controls.Add(this.attackBtn);
            this.panel1.Location = new System.Drawing.Point(91, 307);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(210, 100);
            this.panel1.TabIndex = 8;
            // 
            // specialBtn
            // 
            this.specialBtn.Location = new System.Drawing.Point(19, 61);
            this.specialBtn.Name = "specialBtn";
            this.specialBtn.Size = new System.Drawing.Size(163, 23);
            this.specialBtn.TabIndex = 9;
            this.specialBtn.Text = "Special";
            this.specialBtn.UseVisualStyleBackColor = true;
            // 
            // defendBtn
            // 
            this.defendBtn.Location = new System.Drawing.Point(19, 32);
            this.defendBtn.Name = "defendBtn";
            this.defendBtn.Size = new System.Drawing.Size(163, 23);
            this.defendBtn.TabIndex = 8;
            this.defendBtn.Text = "Defend";
            this.defendBtn.UseVisualStyleBackColor = true;
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.enemyPb2);
            this.Controls.Add(this.enemyPb1);
            this.Controls.Add(this.enemyPb3);
            this.Controls.Add(this.heroPb3);
            this.Controls.Add(this.heroPb2);
            this.Controls.Add(this.heroPb1);
            this.Enabled = false;
            this.Name = "GameScreen";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.heroPb1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heroPb2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.heroPb3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemyPb3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemyPb1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.enemyPb2)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox heroPb1;
        private System.Windows.Forms.PictureBox heroPb2;
        private System.Windows.Forms.PictureBox heroPb3;
        private System.Windows.Forms.PictureBox enemyPb3;
        private System.Windows.Forms.PictureBox enemyPb1;
        private System.Windows.Forms.PictureBox enemyPb2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button attackBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button specialBtn;
        private System.Windows.Forms.Button defendBtn;
    }
}

