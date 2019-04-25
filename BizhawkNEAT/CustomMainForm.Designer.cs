namespace BizHawk.Client.EmuHawk
{
	partial class CustomMainForm
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
            this.Sel = new System.Windows.Forms.Button();
            this.Lewo = new System.Windows.Forms.Button();
            this.Góra = new System.Windows.Forms.Button();
            this.Prawo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Sel
            // 
            this.Sel.Location = new System.Drawing.Point(110, 173);
            this.Sel.Name = "Sel";
            this.Sel.Size = new System.Drawing.Size(75, 23);
            this.Sel.TabIndex = 2;
            this.Sel.Text = "Kupa";
            this.Sel.UseVisualStyleBackColor = true;
            this.Sel.Click += new System.EventHandler(this.Sel_Click);
            // 
            // Lewo
            // 
            this.Lewo.Location = new System.Drawing.Point(250, 173);
            this.Lewo.Name = "Lewo";
            this.Lewo.Size = new System.Drawing.Size(75, 23);
            this.Lewo.TabIndex = 3;
            this.Lewo.Text = "Lewo";
            this.Lewo.UseVisualStyleBackColor = true;
            this.Lewo.Click += new System.EventHandler(this.Lewo_Click);
            // 
            // Góra
            // 
            this.Góra.Location = new System.Drawing.Point(110, 91);
            this.Góra.Name = "Góra";
            this.Góra.Size = new System.Drawing.Size(75, 23);
            this.Góra.TabIndex = 4;
            this.Góra.Text = "Start";
            this.Góra.UseVisualStyleBackColor = true;
            this.Góra.Click += new System.EventHandler(this.Góra_Click);
            // 
            // Prawo
            // 
            this.Prawo.Location = new System.Drawing.Point(250, 91);
            this.Prawo.Name = "Prawo";
            this.Prawo.Size = new System.Drawing.Size(75, 23);
            this.Prawo.TabIndex = 5;
            this.Prawo.Text = "Prawo";
            this.Prawo.UseVisualStyleBackColor = true;
            this.Prawo.Click += new System.EventHandler(this.Prawo_Click);
            // 
            // CustomMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Prawo);
            this.Controls.Add(this.Góra);
            this.Controls.Add(this.Lewo);
            this.Controls.Add(this.Sel);
            this.Name = "CustomMainForm";
            this.Text = "CustomMainForm";
            this.ResumeLayout(false);

		}

        #endregion
        private System.Windows.Forms.Button Sel;
        private System.Windows.Forms.Button Lewo;
        private System.Windows.Forms.Button Góra;
        private System.Windows.Forms.Button Prawo;
    }
}