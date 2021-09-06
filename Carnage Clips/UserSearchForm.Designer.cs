
namespace Carnage_Clips
{
    partial class UserSearchForm
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
            this.lstFoundUsers = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDetailedUser = new System.Windows.Forms.Label();
            this.treeUserDetailed = new System.Windows.Forms.TreeView();
            this.btnClearSearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserSearch = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblStatus
            // 
            this.lblStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(137)))), ((int)(((byte)(58)))), ((int)(((byte)(51)))));
            this.lblStatus.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblStatus.Location = new System.Drawing.Point(0, 517);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(808, 34);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "Idle";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lstFoundUsers
            // 
            this.lstFoundUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.lstFoundUsers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstFoundUsers.Dock = System.Windows.Forms.DockStyle.Right;
            this.lstFoundUsers.ForeColor = System.Drawing.Color.White;
            this.lstFoundUsers.HideSelection = false;
            this.lstFoundUsers.Location = new System.Drawing.Point(316, 0);
            this.lstFoundUsers.Name = "lstFoundUsers";
            this.lstFoundUsers.Size = new System.Drawing.Size(492, 298);
            this.lstFoundUsers.TabIndex = 1;
            this.lstFoundUsers.UseCompatibleStateImageBehavior = false;
            this.lstFoundUsers.View = System.Windows.Forms.View.List;
            this.lstFoundUsers.DoubleClick += new System.EventHandler(this.lstFoundUsers_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.panel1.Controls.Add(this.lblDetailedUser);
            this.panel1.Controls.Add(this.treeUserDetailed);
            this.panel1.Controls.Add(this.lstFoundUsers);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 219);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(808, 298);
            this.panel1.TabIndex = 2;
            // 
            // lblDetailedUser
            // 
            this.lblDetailedUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDetailedUser.Location = new System.Drawing.Point(0, 0);
            this.lblDetailedUser.Name = "lblDetailedUser";
            this.lblDetailedUser.Size = new System.Drawing.Size(316, 43);
            this.lblDetailedUser.TabIndex = 3;
            this.lblDetailedUser.Text = "No user selected";
            this.lblDetailedUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // treeUserDetailed
            // 
            this.treeUserDetailed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.treeUserDetailed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeUserDetailed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.treeUserDetailed.ForeColor = System.Drawing.Color.White;
            this.treeUserDetailed.Location = new System.Drawing.Point(0, 43);
            this.treeUserDetailed.Name = "treeUserDetailed";
            this.treeUserDetailed.Size = new System.Drawing.Size(316, 255);
            this.treeUserDetailed.TabIndex = 2;
            // 
            // btnClearSearch
            // 
            this.btnClearSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnClearSearch.FlatAppearance.BorderSize = 0;
            this.btnClearSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearSearch.ForeColor = System.Drawing.Color.White;
            this.btnClearSearch.Location = new System.Drawing.Point(656, 179);
            this.btnClearSearch.Name = "btnClearSearch";
            this.btnClearSearch.Size = new System.Drawing.Size(152, 34);
            this.btnClearSearch.TabIndex = 5;
            this.btnClearSearch.Text = "Clear search results";
            this.btnClearSearch.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(172, 179);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(448, 34);
            this.label3.TabIndex = 12;
            this.label3.Text = "Double click a user in the grid to load detailed information and characters. You " +
    "can then click the character on the left you wish to view matches for.\r\n";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtUserSearch
            // 
            this.txtUserSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserSearch.Location = new System.Drawing.Point(302, 94);
            this.txtUserSearch.Name = "txtUserSearch";
            this.txtUserSearch.Size = new System.Drawing.Size(190, 25);
            this.txtUserSearch.TabIndex = 14;
            this.txtUserSearch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(40)))), ((int)(((byte)(46)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.Location = new System.Drawing.Point(302, 125);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(190, 34);
            this.btnSearch.TabIndex = 13;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(172, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(448, 20);
            this.label4.TabIndex = 15;
            this.label4.Text = "Enter name with or without bungie id code";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // UserSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(44)))), ((int)(((byte)(50)))), ((int)(((byte)(56)))));
            this.ClientSize = new System.Drawing.Size(808, 551);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUserSearch);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClearSearch);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblStatus);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(808, 551);
            this.MinimumSize = new System.Drawing.Size(808, 551);
            this.Name = "UserSearchForm";
            this.Text = "UserSearchForm";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ListView lstFoundUsers;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDetailedUser;
        private System.Windows.Forms.TreeView treeUserDetailed;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserSearch;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label4;
    }
}