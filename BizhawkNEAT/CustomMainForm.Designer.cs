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
            this.start = new System.Windows.Forms.Button();
            this.networkGraph = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.networkGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(12, 12);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 23);
            this.start.TabIndex = 0;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.Start_Click);
            // 
            // networkGraph
            // 
            this.networkGraph.Location = new System.Drawing.Point(13, 42);
            this.networkGraph.Name = "networkGraph";
            this.networkGraph.Size = new System.Drawing.Size(606, 291);
            this.networkGraph.TabIndex = 1;
            this.networkGraph.TabStop = false;
            // 
            // CustomMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(631, 345);
            this.Controls.Add(this.networkGraph);
            this.Controls.Add(this.start);
            this.Name = "CustomMainForm";
            this.Text = "NEAT";
            ((System.ComponentModel.ISupportInitialize)(this.networkGraph)).EndInit();
            this.ResumeLayout(false);

		}

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.PictureBox networkGraph;
    }
}