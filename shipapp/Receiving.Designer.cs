namespace shipapp
{
    partial class Receiving
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridPackages = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox8 = new System.Windows.Forms.PictureBox();
            this.pictureBox6 = new System.Windows.Forms.PictureBox();
            this.pcBXRefreash = new System.Windows.Forms.PictureBox();
            this.pcBxPrint = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pcBxEdit = new System.Windows.Forms.PictureBox();
            this.btnAdd = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPackages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBXRefreash)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBxPrint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBxEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridPackages
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridPackages.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridPackages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridPackages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridPackages.Location = new System.Drawing.Point(0, 149);
            this.dataGridPackages.Name = "dataGridPackages";
            this.dataGridPackages.Size = new System.Drawing.Size(1234, 512);
            this.dataGridPackages.TabIndex = 0;
            this.dataGridPackages.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(107, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "User";
            this.toolTip1.SetToolTip(this.label1, "Click to Sign Out");
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // pictureBox8
            // 
            this.pictureBox8.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox8.BackgroundImage = global::shipapp.Properties.Resources.android_contact;
            this.pictureBox8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox8.Location = new System.Drawing.Point(63, 12);
            this.pictureBox8.Name = "pictureBox8";
            this.pictureBox8.Size = new System.Drawing.Size(38, 35);
            this.pictureBox8.TabIndex = 10;
            this.pictureBox8.TabStop = false;
            this.pictureBox8.Click += new System.EventHandler(this.pictureBox8_Click);
            // 
            // pictureBox6
            // 
            this.pictureBox6.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox6.BackgroundImage = global::shipapp.Properties.Resources.arrow_left_c;
            this.pictureBox6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox6.Location = new System.Drawing.Point(12, 12);
            this.pictureBox6.Name = "pictureBox6";
            this.pictureBox6.Size = new System.Drawing.Size(35, 35);
            this.pictureBox6.TabIndex = 8;
            this.pictureBox6.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox6, "Back");
            this.pictureBox6.Click += new System.EventHandler(this.pictureBox6_Click);
            // 
            // pcBXRefreash
            // 
            this.pcBXRefreash.BackColor = System.Drawing.Color.Transparent;
            this.pcBXRefreash.Location = new System.Drawing.Point(176, 94);
            this.pcBXRefreash.Name = "pcBXRefreash";
            this.pcBXRefreash.Size = new System.Drawing.Size(35, 35);
            this.pcBXRefreash.TabIndex = 7;
            this.pcBXRefreash.TabStop = false;
            // 
            // pcBxPrint
            // 
            this.pcBxPrint.BackColor = System.Drawing.Color.Transparent;
            this.pcBxPrint.BackgroundImage = global::shipapp.Properties.Resources.android_printer;
            this.pcBxPrint.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pcBxPrint.Location = new System.Drawing.Point(135, 94);
            this.pcBxPrint.Name = "pcBxPrint";
            this.pcBxPrint.Size = new System.Drawing.Size(35, 35);
            this.pcBxPrint.TabIndex = 6;
            this.pcBxPrint.TabStop = false;
            this.toolTip1.SetToolTip(this.pcBxPrint, "Print Log");
            this.pcBxPrint.Click += new System.EventHandler(this.pcBxPrint_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::shipapp.Properties.Resources.android_close;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox3.Location = new System.Drawing.Point(94, 94);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(35, 35);
            this.pictureBox3.TabIndex = 5;
            this.pictureBox3.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox3, "Delete Package");
            this.pictureBox3.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // pcBxEdit
            // 
            this.pcBxEdit.BackColor = System.Drawing.Color.Transparent;
            this.pcBxEdit.BackgroundImage = global::shipapp.Properties.Resources.compose;
            this.pcBxEdit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pcBxEdit.Location = new System.Drawing.Point(53, 94);
            this.pcBxEdit.Name = "pcBxEdit";
            this.pcBxEdit.Size = new System.Drawing.Size(35, 35);
            this.pcBxEdit.TabIndex = 4;
            this.pcBxEdit.TabStop = false;
            this.toolTip1.SetToolTip(this.pcBxEdit, "Edit Package");
            this.pcBxEdit.Click += new System.EventHandler(this.pcBxEdit_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.Transparent;
            this.btnAdd.BackgroundImage = global::shipapp.Properties.Resources.android_add;
            this.btnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAdd.Image = global::shipapp.Properties.Resources.android_add;
            this.btnAdd.Location = new System.Drawing.Point(12, 94);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(35, 35);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.TabStop = false;
            this.toolTip1.SetToolTip(this.btnAdd, "Add Package");
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // Receiving
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1234, 661);
            this.Controls.Add(this.pictureBox8);
            this.Controls.Add(this.pictureBox6);
            this.Controls.Add(this.pcBXRefreash);
            this.Controls.Add(this.pcBxPrint);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pcBxEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridPackages);
            this.Name = "Receiving";
            this.Text = "Receiving";
            this.Load += new System.EventHandler(this.Receiving_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridPackages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBXRefreash)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBxPrint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcBxEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnAdd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox btnAdd;
        private System.Windows.Forms.PictureBox pcBxEdit;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pcBxPrint;
        private System.Windows.Forms.PictureBox pcBXRefreash;
        private System.Windows.Forms.PictureBox pictureBox6;
        private System.Windows.Forms.PictureBox pictureBox8;
        private System.Windows.Forms.ToolTip toolTip1;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.DataGridView dataGridPackages;
    }
}