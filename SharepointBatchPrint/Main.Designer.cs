namespace SharepointBatchPrint
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
            this.btnPrint.Location = new System.Drawing.Point(441, 330);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // boxxy
            // 
            this.boxxy.FormattingEnabled = true;
            this.boxxy.Location = new System.Drawing.Point(12, 12);
            this.boxxy.Name = "boxxy";
            this.boxxy.Size = new System.Drawing.Size(584, 304);
            this.boxxy.TabIndex = 3;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(522, 330);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSelAll
            // 
            this.btnSelAll.Location = new System.Drawing.Point(12, 330);
            this.btnSelAll.Name = "btnSelAll";
            this.btnSelAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelAll.TabIndex = 5;
            this.btnSelAll.Text = "Select All";
            this.btnSelAll.UseVisualStyleBackColor = true;
            this.btnSelAll.Click += new System.EventHandler(this.btnSelAll_Click);
            // 
            // btnInvertSel
            // 
            this.btnInvertSel.Location = new System.Drawing.Point(93, 330);
            this.btnInvertSel.Name = "btnInvertSel";
            this.btnInvertSel.Size = new System.Drawing.Size(95, 23);
            this.btnInvertSel.TabIndex = 6;
            this.btnInvertSel.Text = "Invert Selection";
            this.btnInvertSel.UseVisualStyleBackColor = true;
            this.btnInvertSel.Click += new System.EventHandler(this.btnInvertSel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(629, 365);
            this.Controls.Add(this.btnInvertSel);
            this.Controls.Add(this.btnSelAll);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.boxxy);
            this.Controls.Add(this.btnPrint);
            this.Name = "Form1";
            this.Text = "Print";
            this.ResumeLayout(false);

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
    }
}

