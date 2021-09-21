
namespace Carnage_Clips
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelMenuContainer = new System.Windows.Forms.Panel();
            this.btnDashboard = new System.Windows.Forms.Button();
            this.panelCarnageSettings = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCarnageSettings = new System.Windows.Forms.Button();
            this.panelCharacterContainer = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.btnCharacter3 = new System.Windows.Forms.Button();
            this.btnCharacter2 = new System.Windows.Forms.Button();
            this.btnCharacter1 = new System.Windows.Forms.Button();
            this.btnUserSearch = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panelSelectionIndication = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panelFormContainer = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelMenuContainer.SuspendLayout();
            this.panelCarnageSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panelCharacterContainer.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(202, 137);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // panelMenuContainer
            // 
            this.panelMenuContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.panelMenuContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelMenuContainer.Controls.Add(this.btnDashboard);
            this.panelMenuContainer.Controls.Add(this.panelCarnageSettings);
            this.panelMenuContainer.Controls.Add(this.btnCarnageSettings);
            this.panelMenuContainer.Controls.Add(this.panelCharacterContainer);
            this.panelMenuContainer.Controls.Add(this.btnUserSearch);
            this.panelMenuContainer.Controls.Add(this.panel3);
            this.panelMenuContainer.Controls.Add(this.button2);
            this.panelMenuContainer.Controls.Add(this.pictureBox1);
            this.panelMenuContainer.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMenuContainer.Location = new System.Drawing.Point(0, 0);
            this.panelMenuContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelMenuContainer.Name = "panelMenuContainer";
            this.panelMenuContainer.Size = new System.Drawing.Size(204, 608);
            this.panelMenuContainer.TabIndex = 6;
            // 
            // btnDashboard
            // 
            this.btnDashboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnDashboard.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnDashboard.FlatAppearance.BorderSize = 0;
            this.btnDashboard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDashboard.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDashboard.ForeColor = System.Drawing.Color.White;
            this.btnDashboard.Location = new System.Drawing.Point(10, 479);
            this.btnDashboard.Name = "btnDashboard";
            this.btnDashboard.Size = new System.Drawing.Size(192, 45);
            this.btnDashboard.TabIndex = 12;
            this.btnDashboard.Text = "Dashboard";
            this.btnDashboard.UseVisualStyleBackColor = false;
            this.btnDashboard.Click += new System.EventHandler(this.btnDashboard_Click);
            // 
            // panelCarnageSettings
            // 
            this.panelCarnageSettings.Controls.Add(this.comboBox2);
            this.panelCarnageSettings.Controls.Add(this.numericUpDown1);
            this.panelCarnageSettings.Controls.Add(this.label2);
            this.panelCarnageSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCarnageSettings.Location = new System.Drawing.Point(10, 389);
            this.panelCarnageSettings.Name = "panelCarnageSettings";
            this.panelCarnageSettings.Size = new System.Drawing.Size(192, 90);
            this.panelCarnageSettings.TabIndex = 11;
            // 
            // comboBox2
            // 
            this.comboBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "\tNo filter",
            "\tAll PvP",
            "\tAll PvE",
            "\tTrials",
            "\tIron Banner"});
            this.comboBox2.Location = new System.Drawing.Point(0, 53);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(192, 27);
            this.comboBox2.TabIndex = 18;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            this.comboBox2.SelectedValueChanged += new System.EventHandler(this.comboBox2_SelectedValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Dock = System.Windows.Forms.DockStyle.Top;
            this.numericUpDown1.Location = new System.Drawing.Point(0, 29);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(192, 24);
            this.numericUpDown1.TabIndex = 1;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "Match count";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCarnageSettings
            // 
            this.btnCarnageSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnCarnageSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCarnageSettings.FlatAppearance.BorderSize = 0;
            this.btnCarnageSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCarnageSettings.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCarnageSettings.ForeColor = System.Drawing.Color.White;
            this.btnCarnageSettings.Location = new System.Drawing.Point(10, 344);
            this.btnCarnageSettings.Name = "btnCarnageSettings";
            this.btnCarnageSettings.Size = new System.Drawing.Size(192, 45);
            this.btnCarnageSettings.TabIndex = 10;
            this.btnCarnageSettings.Text = "Carnage Report Settings";
            this.btnCarnageSettings.UseVisualStyleBackColor = false;
            this.btnCarnageSettings.Click += new System.EventHandler(this.btnCarnageSettings_Click);
            // 
            // panelCharacterContainer
            // 
            this.panelCharacterContainer.Controls.Add(this.comboBox1);
            this.panelCharacterContainer.Controls.Add(this.btnCharacter3);
            this.panelCharacterContainer.Controls.Add(this.btnCharacter2);
            this.panelCharacterContainer.Controls.Add(this.btnCharacter1);
            this.panelCharacterContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCharacterContainer.Location = new System.Drawing.Point(10, 182);
            this.panelCharacterContainer.Name = "panelCharacterContainer";
            this.panelCharacterContainer.Size = new System.Drawing.Size(192, 162);
            this.panelCharacterContainer.TabIndex = 9;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(0, 135);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(192, 27);
            this.comboBox1.TabIndex = 17;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1_SelectionChangeCommitted);
            // 
            // btnCharacter3
            // 
            this.btnCharacter3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.btnCharacter3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCharacter3.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCharacter3.FlatAppearance.BorderSize = 0;
            this.btnCharacter3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharacter3.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCharacter3.ForeColor = System.Drawing.Color.White;
            this.btnCharacter3.Location = new System.Drawing.Point(0, 90);
            this.btnCharacter3.Name = "btnCharacter3";
            this.btnCharacter3.Size = new System.Drawing.Size(192, 45);
            this.btnCharacter3.TabIndex = 15;
            this.btnCharacter3.Text = "Char 3";
            this.btnCharacter3.UseVisualStyleBackColor = false;
            this.btnCharacter3.Click += new System.EventHandler(this.btnCharacter3_Click);
            // 
            // btnCharacter2
            // 
            this.btnCharacter2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.btnCharacter2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCharacter2.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCharacter2.FlatAppearance.BorderSize = 0;
            this.btnCharacter2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharacter2.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCharacter2.ForeColor = System.Drawing.Color.White;
            this.btnCharacter2.Location = new System.Drawing.Point(0, 45);
            this.btnCharacter2.Name = "btnCharacter2";
            this.btnCharacter2.Size = new System.Drawing.Size(192, 45);
            this.btnCharacter2.TabIndex = 14;
            this.btnCharacter2.Text = "Char 2";
            this.btnCharacter2.UseVisualStyleBackColor = false;
            this.btnCharacter2.Click += new System.EventHandler(this.btnCharacter2_Click);
            // 
            // btnCharacter1
            // 
            this.btnCharacter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.btnCharacter1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCharacter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCharacter1.FlatAppearance.BorderSize = 0;
            this.btnCharacter1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharacter1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCharacter1.ForeColor = System.Drawing.Color.White;
            this.btnCharacter1.Location = new System.Drawing.Point(0, 0);
            this.btnCharacter1.Name = "btnCharacter1";
            this.btnCharacter1.Size = new System.Drawing.Size(192, 45);
            this.btnCharacter1.TabIndex = 13;
            this.btnCharacter1.Text = "Char 1";
            this.btnCharacter1.UseVisualStyleBackColor = false;
            this.btnCharacter1.Click += new System.EventHandler(this.btnCharacter1_Click);
            // 
            // btnUserSearch
            // 
            this.btnUserSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnUserSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnUserSearch.FlatAppearance.BorderSize = 0;
            this.btnUserSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUserSearch.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUserSearch.ForeColor = System.Drawing.Color.White;
            this.btnUserSearch.Location = new System.Drawing.Point(10, 137);
            this.btnUserSearch.Name = "btnUserSearch";
            this.btnUserSearch.Size = new System.Drawing.Size(192, 45);
            this.btnUserSearch.TabIndex = 8;
            this.btnUserSearch.Text = "Guardian Search";
            this.btnUserSearch.UseVisualStyleBackColor = false;
            this.btnUserSearch.Click += new System.EventHandler(this.btnUserSearch_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panelSelectionIndication);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 137);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 424);
            this.panel3.TabIndex = 7;
            // 
            // panelSelectionIndication
            // 
            this.panelSelectionIndication.BackColor = System.Drawing.Color.White;
            this.panelSelectionIndication.Location = new System.Drawing.Point(0, 0);
            this.panelSelectionIndication.Name = "panelSelectionIndication";
            this.panelSelectionIndication.Size = new System.Drawing.Size(10, 45);
            this.panelSelectionIndication.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.button2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(0, 561);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(202, 45);
            this.button2.TabIndex = 6;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(204, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(808, 57);
            this.panel2.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(808, 57);
            this.label1.TabIndex = 7;
            this.label1.Text = "Carange Clips";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.label1_MouseDown);
            this.label1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.label1_MouseMove);
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.label1_MouseUp);
            // 
            // panelFormContainer
            // 
            this.panelFormContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.panelFormContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelFormContainer.Location = new System.Drawing.Point(204, 57);
            this.panelFormContainer.Name = "panelFormContainer";
            this.panelFormContainer.Size = new System.Drawing.Size(808, 551);
            this.panelFormContainer.TabIndex = 8;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1012, 608);
            this.Controls.Add(this.panelFormContainer);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelMenuContainer);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(1012, 608);
            this.MinimumSize = new System.Drawing.Size(1012, 608);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelMenuContainer.ResumeLayout(false);
            this.panelCarnageSettings.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panelCharacterContainer.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panelMenuContainer;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Panel panelCarnageSettings;
        private System.Windows.Forms.Button btnCarnageSettings;
        private System.Windows.Forms.Panel panelCharacterContainer;
        private System.Windows.Forms.Button btnCharacter3;
        private System.Windows.Forms.Button btnCharacter2;
        private System.Windows.Forms.Button btnCharacter1;
        private System.Windows.Forms.Button btnUserSearch;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panelSelectionIndication;
        private System.Windows.Forms.Panel panelFormContainer;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox2;
    }
}

