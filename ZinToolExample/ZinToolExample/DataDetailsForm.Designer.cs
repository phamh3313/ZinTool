namespace ZinToolExample
{
    partial class DataDetailsForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtNumberOfDays = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblProcessing = new System.Windows.Forms.Label();
            this.btnGetData = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gvDataDetails = new System.Windows.Forms.DataGridView();
            this.cbCoinType = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbCoinType);
            this.panel1.Controls.Add(this.txtNumberOfDays);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.lblProcessing);
            this.panel1.Controls.Add(this.btnGetData);
            this.panel1.Controls.Add(this.txtSearch);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1019, 34);
            this.panel1.TabIndex = 5;
            // 
            // txtNumberOfDays
            // 
            this.txtNumberOfDays.Location = new System.Drawing.Point(627, 6);
            this.txtNumberOfDays.Name = "txtNumberOfDays";
            this.txtNumberOfDays.Size = new System.Drawing.Size(145, 20);
            this.txtNumberOfDays.TabIndex = 8;
            this.txtNumberOfDays.Text = "1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(540, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Number of days:";
            // 
            // lblProcessing
            // 
            this.lblProcessing.AutoSize = true;
            this.lblProcessing.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblProcessing.ForeColor = System.Drawing.Color.Blue;
            this.lblProcessing.Location = new System.Drawing.Point(876, 7);
            this.lblProcessing.Name = "lblProcessing";
            this.lblProcessing.Size = new System.Drawing.Size(102, 16);
            this.lblProcessing.TabIndex = 6;
            this.lblProcessing.Text = "Processing....";
            this.lblProcessing.Visible = false;
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(778, 4);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(75, 23);
            this.btnGetData.TabIndex = 2;
            this.btnGetData.Text = "Get Data";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(47, 6);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(401, 20);
            this.txtSearch.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name:";
            // 
            // gvDataDetails
            // 
            this.gvDataDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvDataDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvDataDetails.Location = new System.Drawing.Point(0, 34);
            this.gvDataDetails.Name = "gvDataDetails";
            this.gvDataDetails.Size = new System.Drawing.Size(1019, 505);
            this.gvDataDetails.TabIndex = 6;
            this.gvDataDetails.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.gvDataDetails_CellFormatting);
            // 
            // cbCoinType
            // 
            this.cbCoinType.FormattingEnabled = true;
            this.cbCoinType.Items.AddRange(new object[] {
            "BTC",
            "USDT"});
            this.cbCoinType.Location = new System.Drawing.Point(454, 6);
            this.cbCoinType.Name = "cbCoinType";
            this.cbCoinType.Size = new System.Drawing.Size(80, 21);
            this.cbCoinType.TabIndex = 9;
            this.cbCoinType.Text = "BTC";
            // 
            // DataDetailsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1019, 539);
            this.Controls.Add(this.gvDataDetails);
            this.Controls.Add(this.panel1);
            this.Name = "DataDetailsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataDetailsForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gvDataDetails)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGetData;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView gvDataDetails;
        private System.Windows.Forms.Label lblProcessing;
        private System.Windows.Forms.TextBox txtNumberOfDays;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbCoinType;
    }
}