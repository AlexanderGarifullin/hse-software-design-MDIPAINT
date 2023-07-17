namespace MDIPAINT
{
    partial class DocumentForm
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
            this.SuspendLayout();
            // 
            // DocumentForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.DoubleBuffered = true;
            this.Name = "DocumentForm";
            this.Text = "DocumentForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DocumentForm_FormClosing);
           this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DocumentForm_MouseDown);
            this.MouseLeave += new System.EventHandler(this.DocumentForm_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DocumentForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DocumentForm_MouseUp);
            this.ResumeLayout(false);

        }

        #endregion
    }
}