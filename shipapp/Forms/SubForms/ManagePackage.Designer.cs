namespace shipapp
{
    partial class ManagePackage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManagePackage));
            this.lblPO = new System.Windows.Forms.Label();
            this.txtPO = new System.Windows.Forms.TextBox();
            this.lblCarrier = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtTracking = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmboCarrier = new System.Windows.Forms.ComboBox();
            this.cmboVendor = new System.Windows.Forms.ComboBox();
            this.cmboRecipiant = new System.Windows.Forms.ComboBox();
            this.cmboDelBy = new System.Windows.Forms.ComboBox();
            this.cmboSignedBy = new System.Windows.Forms.ComboBox();
            this.cmboStatus = new System.Windows.Forms.ComboBox();
            this.btnViewNote = new System.Windows.Forms.Button();
            this.btnAddNote = new System.Windows.Forms.Button();
            this.btnReceive = new System.Windows.Forms.Button();
            this.dTRec = new System.Windows.Forms.DateTimePicker();
            this.dTDel = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRoleId = new System.Windows.Forms.TextBox();
            this.cmboBuilding = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lblroom = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblPO
            // 
            this.lblPO.AutoSize = true;
            this.lblPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPO.Location = new System.Drawing.Point(13, 13);
            this.lblPO.Name = "lblPO";
            this.lblPO.Size = new System.Drawing.Size(43, 20);
            this.lblPO.TabIndex = 0;
            this.lblPO.Text = "PO#";
            // 
            // txtPO
            // 
            this.txtPO.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPO.Location = new System.Drawing.Point(109, 10);
            this.txtPO.Name = "txtPO";
            this.txtPO.Size = new System.Drawing.Size(169, 26);
            this.txtPO.TabIndex = 1;
            this.txtPO.Leave += new System.EventHandler(this.txtPO_Leave);
            // 
            // lblCarrier
            // 
            this.lblCarrier.AutoSize = true;
            this.lblCarrier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCarrier.Location = new System.Drawing.Point(13, 72);
            this.lblCarrier.Name = "lblCarrier";
            this.lblCarrier.Size = new System.Drawing.Size(63, 20);
            this.lblCarrier.TabIndex = 2;
            this.lblCarrier.Text = "Carrier";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Vendor";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 136);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Recipiant";
            // 
            // txtTracking
            // 
            this.txtTracking.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTracking.Location = new System.Drawing.Point(109, 39);
            this.txtTracking.Name = "txtTracking";
            this.txtTracking.Size = new System.Drawing.Size(169, 26);
            this.txtTracking.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 42);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(92, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Tracking #";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(13, 200);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 20);
            this.label6.TabIndex = 10;
            this.label6.Text = "Date Rec";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(303, 77);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 20);
            this.label7.TabIndex = 12;
            this.label7.Text = "Date Del";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(303, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Delivered By";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(303, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 20);
            this.label2.TabIndex = 16;
            this.label2.Text = "Signed By";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(303, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 20);
            this.label8.TabIndex = 18;
            this.label8.Text = "Status";
            // 
            // cmboCarrier
            // 
            this.cmboCarrier.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboCarrier.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboCarrier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboCarrier.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboCarrier.FormattingEnabled = true;
            this.cmboCarrier.Location = new System.Drawing.Point(109, 69);
            this.cmboCarrier.Name = "cmboCarrier";
            this.cmboCarrier.Size = new System.Drawing.Size(169, 28);
            this.cmboCarrier.TabIndex = 3;
            this.cmboCarrier.SelectionChangeCommitted += new System.EventHandler(this.cmboCarrier_SelectionChangeCommitted);
            // 
            // cmboVendor
            // 
            this.cmboVendor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboVendor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboVendor.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboVendor.FormattingEnabled = true;
            this.cmboVendor.Location = new System.Drawing.Point(109, 101);
            this.cmboVendor.Name = "cmboVendor";
            this.cmboVendor.Size = new System.Drawing.Size(169, 28);
            this.cmboVendor.TabIndex = 4;
            this.cmboVendor.SelectedIndexChanged += new System.EventHandler(this.cmboVendor_SelectedIndexChanged);
            this.cmboVendor.SelectionChangeCommitted += new System.EventHandler(this.cmboVendor_SelectionChangeCommitted);
            this.cmboVendor.TextChanged += new System.EventHandler(this.cmboVendor_TextChanged);
            // 
            // cmboRecipiant
            // 
            this.cmboRecipiant.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboRecipiant.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboRecipiant.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboRecipiant.FormattingEnabled = true;
            this.cmboRecipiant.Location = new System.Drawing.Point(109, 133);
            this.cmboRecipiant.Name = "cmboRecipiant";
            this.cmboRecipiant.Size = new System.Drawing.Size(169, 28);
            this.cmboRecipiant.TabIndex = 5;
            this.cmboRecipiant.SelectedIndexChanged += new System.EventHandler(this.cmboRecipiant_SelectedIndexChanged);
            this.cmboRecipiant.SelectionChangeCommitted += new System.EventHandler(this.cmboRecipiant_SelectionChangeCommitted);
            this.cmboRecipiant.TextChanged += new System.EventHandler(this.cmboRecipiant_TextChanged);
            // 
            // cmboDelBy
            // 
            this.cmboDelBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboDelBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboDelBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboDelBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboDelBy.FormattingEnabled = true;
            this.cmboDelBy.ItemHeight = 20;
            this.cmboDelBy.Location = new System.Drawing.Point(417, 10);
            this.cmboDelBy.Name = "cmboDelBy";
            this.cmboDelBy.Size = new System.Drawing.Size(158, 28);
            this.cmboDelBy.TabIndex = 8;
            // 
            // cmboSignedBy
            // 
            this.cmboSignedBy.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboSignedBy.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboSignedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboSignedBy.FormattingEnabled = true;
            this.cmboSignedBy.ItemHeight = 20;
            this.cmboSignedBy.Location = new System.Drawing.Point(417, 42);
            this.cmboSignedBy.Name = "cmboSignedBy";
            this.cmboSignedBy.Size = new System.Drawing.Size(158, 28);
            this.cmboSignedBy.TabIndex = 9;
            this.cmboSignedBy.SelectionChangeCommitted += new System.EventHandler(this.cmboSignedBy_SelectionChangeCommitted);
            this.cmboSignedBy.TextChanged += new System.EventHandler(this.cmboSignedBy_TextChanged);
            // 
            // cmboStatus
            // 
            this.cmboStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboStatus.FormattingEnabled = true;
            this.cmboStatus.Location = new System.Drawing.Point(417, 104);
            this.cmboStatus.Name = "cmboStatus";
            this.cmboStatus.Size = new System.Drawing.Size(158, 28);
            this.cmboStatus.TabIndex = 11;
            this.cmboStatus.SelectedIndexChanged += new System.EventHandler(this.cmboStatus_SelectedIndexChanged);
            this.cmboStatus.SelectionChangeCommitted += new System.EventHandler(this.cmboStatus_SelectionChangeCommitted);
            // 
            // btnViewNote
            // 
            this.btnViewNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnViewNote.Location = new System.Drawing.Point(307, 240);
            this.btnViewNote.Name = "btnViewNote";
            this.btnViewNote.Size = new System.Drawing.Size(132, 41);
            this.btnViewNote.TabIndex = 14;
            this.btnViewNote.Text = "View Note";
            this.btnViewNote.UseVisualStyleBackColor = true;
            this.btnViewNote.Click += new System.EventHandler(this.btnViewNote_Click);
            // 
            // btnAddNote
            // 
            this.btnAddNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNote.Location = new System.Drawing.Point(445, 240);
            this.btnAddNote.Name = "btnAddNote";
            this.btnAddNote.Size = new System.Drawing.Size(130, 41);
            this.btnAddNote.TabIndex = 15;
            this.btnAddNote.Text = "Add Note";
            this.btnAddNote.UseVisualStyleBackColor = true;
            this.btnAddNote.Click += new System.EventHandler(this.btnAddNote_Click);
            // 
            // btnReceive
            // 
            this.btnReceive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReceive.Location = new System.Drawing.Point(109, 241);
            this.btnReceive.Name = "btnReceive";
            this.btnReceive.Size = new System.Drawing.Size(169, 40);
            this.btnReceive.TabIndex = 13;
            this.btnReceive.Text = "Receive";
            this.btnReceive.UseVisualStyleBackColor = true;
            this.btnReceive.Click += new System.EventHandler(this.btnReceive_Click);
            // 
            // dTRec
            // 
            this.dTRec.CustomFormat = "MMM dd, yyyy";
            this.dTRec.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dTRec.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTRec.Location = new System.Drawing.Point(109, 197);
            this.dTRec.Name = "dTRec";
            this.dTRec.ShowUpDown = true;
            this.dTRec.Size = new System.Drawing.Size(169, 26);
            this.dTRec.TabIndex = 7;
            this.dTRec.Value = new System.DateTime(2018, 3, 30, 0, 0, 0, 0);
            // 
            // dTDel
            // 
            this.dTDel.CustomFormat = "MMM dd, yyyy";
            this.dTDel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dTDel.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dTDel.Location = new System.Drawing.Point(417, 74);
            this.dTDel.Name = "dTDel";
            this.dTDel.ShowUpDown = true;
            this.dTDel.Size = new System.Drawing.Size(158, 26);
            this.dTDel.TabIndex = 10;
            this.dTDel.Value = new System.DateTime(2018, 3, 30, 0, 0, 0, 0);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(303, 141);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(87, 20);
            this.label9.TabIndex = 35;
            this.label9.Text = "Unique Id";
            // 
            // txtRoleId
            // 
            this.txtRoleId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRoleId.Location = new System.Drawing.Point(417, 137);
            this.txtRoleId.Name = "txtRoleId";
            this.txtRoleId.ReadOnly = true;
            this.txtRoleId.Size = new System.Drawing.Size(158, 26);
            this.txtRoleId.TabIndex = 12;
            // 
            // cmboBuilding
            // 
            this.cmboBuilding.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmboBuilding.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmboBuilding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboBuilding.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmboBuilding.FormattingEnabled = true;
            this.cmboBuilding.Location = new System.Drawing.Point(109, 163);
            this.cmboBuilding.Name = "cmboBuilding";
            this.cmboBuilding.Size = new System.Drawing.Size(127, 28);
            this.cmboBuilding.TabIndex = 6;
            this.cmboBuilding.SelectionChangeCommitted += new System.EventHandler(this.cmboBuilding_SelectionChangeCommitted);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(13, 170);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 20);
            this.label10.TabIndex = 39;
            this.label10.Text = "Building";
            // 
            // lblroom
            // 
            this.lblroom.AutoSize = true;
            this.lblroom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblroom.Location = new System.Drawing.Point(242, 166);
            this.lblroom.Name = "lblroom";
            this.lblroom.Size = new System.Drawing.Size(0, 20);
            this.lblroom.TabIndex = 40;
            // 
            // ManagePackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(602, 293);
            this.Controls.Add(this.lblroom);
            this.Controls.Add(this.cmboBuilding);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtRoleId);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.dTDel);
            this.Controls.Add(this.dTRec);
            this.Controls.Add(this.btnReceive);
            this.Controls.Add(this.btnAddNote);
            this.Controls.Add(this.btnViewNote);
            this.Controls.Add(this.cmboStatus);
            this.Controls.Add(this.cmboSignedBy);
            this.Controls.Add(this.cmboDelBy);
            this.Controls.Add(this.cmboRecipiant);
            this.Controls.Add(this.cmboVendor);
            this.Controls.Add(this.cmboCarrier);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtTracking);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblCarrier);
            this.Controls.Add(this.txtPO);
            this.Controls.Add(this.lblPO);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ManagePackage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Package";
            this.Load += new System.EventHandler(this.AddPackage_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPO;
        private System.Windows.Forms.TextBox txtPO;
        private System.Windows.Forms.Label lblCarrier;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtTracking;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmboCarrier;
        private System.Windows.Forms.ComboBox cmboVendor;
        private System.Windows.Forms.ComboBox cmboRecipiant;
        private System.Windows.Forms.ComboBox cmboDelBy;
        private System.Windows.Forms.ComboBox cmboSignedBy;
        private System.Windows.Forms.ComboBox cmboStatus;
        private System.Windows.Forms.Button btnViewNote;
        private System.Windows.Forms.Button btnAddNote;
        private System.Windows.Forms.Button btnReceive;
        private System.Windows.Forms.DateTimePicker dTRec;
        private System.Windows.Forms.DateTimePicker dTDel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRoleId;
        private System.Windows.Forms.ComboBox cmboBuilding;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblroom;
    }
}