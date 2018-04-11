namespace shipapp
{
    partial class Reports
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.datGridHistory = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.pcBxAddToDaily = new System.Windows.Forms.PictureBox();
            this.pictureBox7 = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblSearch = new System.Windows.Forms.Label();
            this.pcBxRefreash = new System.Windows.Forms.PictureBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.dTFrom = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dTTo = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.datGridHistory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBxAddToDaily)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBxRefreash)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::shipapp.Properties.Resources.arrow_left_c;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(35, 35);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Back");
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // datGridHistory
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            this.datGridHistory.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.datGridHistory.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datGridHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.datGridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.datGridHistory.Location = new System.Drawing.Point(0, 120);
            this.datGridHistory.Name = "datGridHistory";
            this.datGridHistory.ReadOnly = true;
            this.datGridHistory.Size = new System.Drawing.Size(1234, 541);
            this.datGridHistory.TabIndex = 1;
            this.datGridHistory.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.datGridHistory_CellMouseClick);
            this.datGridHistory.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.datGridHistory_ColumnHeaderMouseClick);
            this.datGridHistory.Click += new System.EventHandler(this.datGridHistory_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(94, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "User";
            this.toolTip1.SetToolTip(this.label1, "Click to Sign Out");
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pcBxAddToDaily
            // 
            this.pcBxAddToDaily.BackColor = System.Drawing.Color.Transparent;
            this.pcBxAddToDaily.BackgroundImage = global::shipapp.Properties.Resources.archive;
            this.pcBxAddToDaily.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pcBxAddToDaily.Location = new System.Drawing.Point(12, 79);
            this.pcBxAddToDaily.Name = "pcBxAddToDaily";
            this.pcBxAddToDaily.Size = new System.Drawing.Size(35, 35);
            this.pcBxAddToDaily.TabIndex = 3;
            this.pcBxAddToDaily.TabStop = false;
            this.pcBxAddToDaily.Click += new System.EventHandler(this.pcBxAddToDaily_Click);
            // 
            // pictureBox7
            // 
            this.pictureBox7.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox7.BackgroundImage = global::shipapp.Properties.Resources.android_contact;
            this.pictureBox7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox7.Location = new System.Drawing.Point(53, 12);
            this.pictureBox7.Name = "pictureBox7";
            this.pictureBox7.Size = new System.Drawing.Size(35, 35);
            this.pictureBox7.TabIndex = 8;
            this.pictureBox7.TabStop = false;
            this.pictureBox7.Click += new System.EventHandler(this.pictureBox7_Click);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearch.Location = new System.Drawing.Point(349, 65);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(58, 20);
            this.lblSearch.TabIndex = 10;
            this.lblSearch.Text = "TODO";
            this.toolTip1.SetToolTip(this.lblSearch, "Click to Sign Out");
            this.lblSearch.Click += new System.EventHandler(this.lblSearch_Click);
            // 
            // pcBxRefreash
            // 
            this.pcBxRefreash.BackColor = System.Drawing.Color.Transparent;
            this.pcBxRefreash.BackgroundImage = global::shipapp.Properties.Resources.loop;
            this.pcBxRefreash.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pcBxRefreash.Location = new System.Drawing.Point(53, 79);
            this.pcBxRefreash.Name = "pcBxRefreash";
            this.pcBxRefreash.Size = new System.Drawing.Size(35, 35);
            this.pcBxRefreash.TabIndex = 9;
            this.pcBxRefreash.TabStop = false;
            this.pcBxRefreash.Click += new System.EventHandler(this.pcBxRefreash_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(297, 88);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(159, 26);
            this.txtSearch.TabIndex = 11;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            // 
            // dTFrom
            // 
            this.dTFrom.CustomFormat = "MMM dd, yyyy";
            this.dTFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dTFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTFrom.Location = new System.Drawing.Point(536, 52);
            this.dTFrom.Name = "dTFrom";
            this.dTFrom.Size = new System.Drawing.Size(146, 26);
            this.dTFrom.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(480, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 20);
            this.label2.TabIndex = 13;
            this.label2.Text = "From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(501, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 20);
            this.label3.TabIndex = 15;
            this.label3.Text = "To";
            // 
            // dTTo
            // 
            this.dTTo.CustomFormat = "MMM dd, yyyy";
            this.dTTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dTTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTTo.Location = new System.Drawing.Point(536, 88);
            this.dTTo.Name = "dTTo";
            this.dTTo.Size = new System.Drawing.Size(146, 26);
            this.dTTo.TabIndex = 14;
            // 
            // Reports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1234, 661);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dTTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dTFrom);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblSearch);
            this.Controls.Add(this.pcBxRefreash);
            this.Controls.Add(this.pictureBox7);
            this.Controls.Add(this.pcBxAddToDaily);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.datGridHistory);
            this.Controls.Add(this.pictureBox1);
            this.Name = "Reports";
            this.Text = "Reports";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Reports_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.datGridHistory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBxAddToDaily)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBxRefreash)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pcBxAddToDaily;
        private System.Windows.Forms.PictureBox pictureBox7;
        private System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.DataGridView datGridHistory;
        private System.Windows.Forms.PictureBox pcBxRefreash;
        internal System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DateTimePicker dTFrom;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dTTo;
    }
}