﻿namespace SharepointBatchPrint
{
    partial class Main
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
            foreach (Document item in boxxy.Items) {
                item.deleteLocalCopy();
            }
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
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnPrint = new System.Windows.Forms.Button();
            this.boxxy = new System.Windows.Forms.CheckedListBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSelAll = new System.Windows.Forms.Button();
            this.btnInvertSel = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtNDocs = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(588, 406);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 28);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // boxxy
            // 
            this.boxxy.FormattingEnabled = true;
            this.boxxy.Location = new System.Drawing.Point(16, 15);
            this.boxxy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.boxxy.Name = "boxxy";
            this.boxxy.Size = new System.Drawing.Size(777, 361);
            this.boxxy.TabIndex = 3;
            this.boxxy.MouseMove += new System.Windows.Forms.MouseEventHandler(this.boxxy_updateCount);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(696, 406);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 28);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(16, 406);
            this.btnSelAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(100, 28);
            this.btnSelAll.TabIndex = 5;
            this.btnSelAll.Text = "Select All";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnInvertSel
            // 
            this.btnInvertSel.Location = new System.Drawing.Point(124, 406);
            this.btnInvertSel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnInvertSel.Name = "btnInvertSel";
            this.btnInvertSel.Size = new System.Drawing.Size(127, 28);
            this.btnInvertSel.TabIndex = 6;
            this.btnInvertSel.Text = "Invert Selection";
            this.btnInvertSel.UseVisualStyleBackColor = true;
            this.btnInvertSel.Click += new System.EventHandler(this.btnInvertSel_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(259, 406);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(100, 28);
            this.btnRefresh.TabIndex = 7;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(480, 406);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 28);
            this.button1.TabIndex = 8;
            this.button1.Text = "Help";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtNDocs
            // 
            this.txtNDocs.AutoSize = true;
            this.txtNDocs.Location = new System.Drawing.Point(367, 412);
            this.txtNDocs.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.txtNDocs.Name = "txtNDocs";
            this.txtNDocs.Size = new System.Drawing.Size(46, 17);
            this.txtNDocs.TabIndex = 9;
            this.txtNDocs.Text = "label1";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 449);
            this.Controls.Add(this.txtNDocs);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnInvertSel);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.boxxy);
            this.Controls.Add(this.btnPrint);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Main";
            this.Text = "Print";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        

        #endregion

        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.CheckedListBox boxxy;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSelAll;
        private System.Windows.Forms.Button btnInvertSel;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label txtNDocs;
    }
}

