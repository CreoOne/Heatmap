namespace HeatmapSamples
{
    partial class FrontForm
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
            this.bSimpleSynchronous = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // bSimpleSynchronous
            // 
            this.bSimpleSynchronous.Location = new System.Drawing.Point(12, 12);
            this.bSimpleSynchronous.Name = "bSimpleSynchronous";
            this.bSimpleSynchronous.Size = new System.Drawing.Size(190, 23);
            this.bSimpleSynchronous.TabIndex = 0;
            this.bSimpleSynchronous.Text = "Simple";
            this.bSimpleSynchronous.UseVisualStyleBackColor = true;
            this.bSimpleSynchronous.Click += new System.EventHandler(this.bSimpleSynchronous_Click);
            // 
            // FrontForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(214, 235);
            this.Controls.Add(this.bSimpleSynchronous);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "FrontForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Heatmap samples";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bSimpleSynchronous;
    }
}

