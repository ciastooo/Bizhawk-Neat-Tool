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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.start = new System.Windows.Forms.Button();
            this.networkGraph = new System.Windows.Forms.PictureBox();
            this.showGenome = new System.Windows.Forms.CheckBox();
            this.infoLabel = new System.Windows.Forms.Label();
            this.fitnessChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.loadButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.networkGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fitnessChart)).BeginInit();
            this.SuspendLayout();
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(12, 13);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(75, 22);
            this.start.TabIndex = 0;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.Start_Click);
            // 
            // networkGraph
            // 
            this.networkGraph.Location = new System.Drawing.Point(12, 257);
            this.networkGraph.Name = "networkGraph";
            this.networkGraph.Size = new System.Drawing.Size(500, 170);
            this.networkGraph.TabIndex = 1;
            this.networkGraph.TabStop = false;
            // 
            // showGenome
            // 
            this.showGenome.AutoSize = true;
            this.showGenome.Location = new System.Drawing.Point(93, 19);
            this.showGenome.Name = "showGenome";
            this.showGenome.Size = new System.Drawing.Size(131, 17);
            this.showGenome.TabIndex = 2;
            this.showGenome.Text = "Show Current genome";
            this.showGenome.UseVisualStyleBackColor = true;
            this.showGenome.CheckedChanged += new System.EventHandler(this.ShowGenome_CheckedChanged);
            // 
            // infoLabel
            // 
            this.infoLabel.AutoSize = true;
            this.infoLabel.Location = new System.Drawing.Point(93, 3);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(0, 13);
            this.infoLabel.TabIndex = 3;
            // 
            // fitnessChart
            // 
            chartArea1.AxisX.MajorGrid.LineWidth = 0;
            chartArea1.AxisX.Title = "Generation";
            chartArea1.AxisY.MajorGrid.LineWidth = 0;
            chartArea1.AxisY.Title = "Fitness";
            chartArea1.BorderWidth = 0;
            chartArea1.Name = "ChartArea1";
            this.fitnessChart.ChartAreas.Add(chartArea1);
            legend1.Enabled = false;
            legend1.Name = "Legend1";
            this.fitnessChart.Legends.Add(legend1);
            this.fitnessChart.Location = new System.Drawing.Point(12, 54);
            this.fitnessChart.Name = "fitnessChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Area;
            series1.IsXValueIndexed = true;
            series1.Legend = "Legend1";
            series1.Name = "Fitness";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Int32;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            series2.IsXValueIndexed = true;
            series2.Legend = "Legend1";
            series2.Name = "AverageFitness";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.fitnessChart.Series.Add(series1);
            this.fitnessChart.Series.Add(series2);
            this.fitnessChart.Size = new System.Drawing.Size(500, 200);
            this.fitnessChart.TabIndex = 4;
            this.fitnessChart.Text = "fitnessChart";
            // 
            // loadButton
            // 
            this.loadButton.Location = new System.Drawing.Point(438, 12);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(75, 23);
            this.loadButton.TabIndex = 5;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.LoadButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(357, 12);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 6;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "json";
            this.saveFileDialog.Filter = "\"neat.network (*.json)|*.json\"";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // CustomMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 440);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.fitnessChart);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.showGenome);
            this.Controls.Add(this.networkGraph);
            this.Controls.Add(this.start);
            this.Name = "CustomMainForm";
            this.Text = "NEAT";
            ((System.ComponentModel.ISupportInitialize)(this.networkGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.fitnessChart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

        #endregion

        private System.Windows.Forms.Button start;
        private System.Windows.Forms.PictureBox networkGraph;
        private System.Windows.Forms.CheckBox showGenome;
        private System.Windows.Forms.Label infoLabel;
        private System.Windows.Forms.DataVisualization.Charting.Chart fitnessChart;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
    }
}