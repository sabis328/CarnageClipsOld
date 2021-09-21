
namespace Carnage_Clips
{
    partial class CarnageReportForm
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
            this.lblStatus = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.treeCarnageReports = new System.Windows.Forms.TreeView();
            this.treePlayers = new System.Windows.Forms.TreeView();
            this.lblCarnageReports = new System.Windows.Forms.Label();
            this.lblPlayerReports = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.ForeColor = System.Drawing.Color.White;
            this.lblStatus.Location = new System.Drawing.Point(0, 517);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(808, 34);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Idle";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 507);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(808, 10);
            this.progressBar1.TabIndex = 2;
            // 
            // treeCarnageReports
            // 
            this.treeCarnageReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.treeCarnageReports.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treeCarnageReports.ForeColor = System.Drawing.Color.White;
            this.treeCarnageReports.Location = new System.Drawing.Point(12, 63);
            this.treeCarnageReports.Name = "treeCarnageReports";
            this.treeCarnageReports.Size = new System.Drawing.Size(365, 390);
            this.treeCarnageReports.TabIndex = 3;
            // 
            // treePlayers
            // 
            this.treePlayers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.treePlayers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.treePlayers.ForeColor = System.Drawing.Color.White;
            this.treePlayers.Location = new System.Drawing.Point(414, 63);
            this.treePlayers.Name = "treePlayers";
            this.treePlayers.Size = new System.Drawing.Size(382, 333);
            this.treePlayers.TabIndex = 4;
            this.treePlayers.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treePlayers_AfterSelect);
            this.treePlayers.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.treePlayers_MouseDoubleClick);
            // 
            // lblCarnageReports
            // 
            this.lblCarnageReports.BackColor = System.Drawing.Color.Transparent;
            this.lblCarnageReports.ForeColor = System.Drawing.Color.White;
            this.lblCarnageReports.Location = new System.Drawing.Point(12, 26);
            this.lblCarnageReports.Name = "lblCarnageReports";
            this.lblCarnageReports.Size = new System.Drawing.Size(365, 34);
            this.lblCarnageReports.TabIndex = 5;
            this.lblCarnageReports.Text = "Idle";
            this.lblCarnageReports.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblPlayerReports
            // 
            this.lblPlayerReports.BackColor = System.Drawing.Color.Transparent;
            this.lblPlayerReports.ForeColor = System.Drawing.Color.White;
            this.lblPlayerReports.Location = new System.Drawing.Point(414, 26);
            this.lblPlayerReports.Name = "lblPlayerReports";
            this.lblPlayerReports.Size = new System.Drawing.Size(382, 34);
            this.lblPlayerReports.TabIndex = 6;
            this.lblPlayerReports.Text = "Idle";
            this.lblPlayerReports.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnSearch.Enabled = false;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(12, 459);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(156, 34);
            this.btnSearch.TabIndex = 14;
            this.btnSearch.Text = "Cancel all";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(609, 402);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 34);
            this.button1.TabIndex = 15;
            this.button1.Text = "Refresh Matches";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(434, 402);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(169, 34);
            this.button2.TabIndex = 16;
            this.button2.Text = "Export Streams";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(406, 439);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(390, 60);
            this.label3.TabIndex = 17;
            this.label3.Text = "Double click a stream link to copy it to your clipboard. Click export to log all " +
    "loaded streams into a text file.";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Enabled = false;
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(174, 456);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(203, 37);
            this.label4.TabIndex = 18;
            this.label4.Text = "Click until 0 shows consistently";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CarnageReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(808, 551);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.lblPlayerReports);
            this.Controls.Add(this.lblCarnageReports);
            this.Controls.Add(this.treePlayers);
            this.Controls.Add(this.treeCarnageReports);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(808, 551);
            this.MinimumSize = new System.Drawing.Size(808, 551);
            this.Name = "CarnageReportForm";
            this.Text = "CarnageReportForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.TreeView treeCarnageReports;
        private System.Windows.Forms.TreeView treePlayers;
        private System.Windows.Forms.Label lblCarnageReports;
        private System.Windows.Forms.Label lblPlayerReports;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}