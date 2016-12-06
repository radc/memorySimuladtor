namespace memorySimulator
{
    partial class ReportForm
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonSleep = new System.Windows.Forms.Button();
            this.buttonPowerOn = new System.Windows.Forms.Button();
            this.buttonReadings = new System.Windows.Forms.Button();
            this.buttonWritings = new System.Windows.Forms.Button();
            this.buttonTgOn2Sleep = new System.Windows.Forms.Button();
            this.buttonTgSleep2On = new System.Windows.Forms.Button();
            this.buttonDutyCycle = new System.Windows.Forms.Button();
            this.buttonCountersReport = new System.Windows.Forms.Button();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelN = new System.Windows.Forms.Label();
            this.textBoxCountersReport = new System.Windows.Forms.TextBox();
            this.buttonExportCsv = new System.Windows.Forms.Button();
            this.buttonClipboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Location = new System.Drawing.Point(17, 74);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1000, 500);
            this.panel2.TabIndex = 6;
            // 
            // buttonSleep
            // 
            this.buttonSleep.Location = new System.Drawing.Point(1023, 74);
            this.buttonSleep.Name = "buttonSleep";
            this.buttonSleep.Size = new System.Drawing.Size(229, 23);
            this.buttonSleep.TabIndex = 7;
            this.buttonSleep.Text = "Sleep Cycles";
            this.buttonSleep.UseVisualStyleBackColor = true;
            this.buttonSleep.Click += new System.EventHandler(this.buttonSleep_Click);
            // 
            // buttonPowerOn
            // 
            this.buttonPowerOn.Location = new System.Drawing.Point(1023, 103);
            this.buttonPowerOn.Name = "buttonPowerOn";
            this.buttonPowerOn.Size = new System.Drawing.Size(229, 23);
            this.buttonPowerOn.TabIndex = 8;
            this.buttonPowerOn.Text = "Power On Cycles";
            this.buttonPowerOn.UseVisualStyleBackColor = true;
            this.buttonPowerOn.Click += new System.EventHandler(this.buttonPowerOn_Click);
            // 
            // buttonReadings
            // 
            this.buttonReadings.Location = new System.Drawing.Point(1023, 132);
            this.buttonReadings.Name = "buttonReadings";
            this.buttonReadings.Size = new System.Drawing.Size(229, 23);
            this.buttonReadings.TabIndex = 9;
            this.buttonReadings.Text = "Total Readings";
            this.buttonReadings.UseVisualStyleBackColor = true;
            this.buttonReadings.Click += new System.EventHandler(this.buttonReadings_Click);
            // 
            // buttonWritings
            // 
            this.buttonWritings.Location = new System.Drawing.Point(1023, 161);
            this.buttonWritings.Name = "buttonWritings";
            this.buttonWritings.Size = new System.Drawing.Size(229, 23);
            this.buttonWritings.TabIndex = 10;
            this.buttonWritings.Text = "Total Writings";
            this.buttonWritings.UseVisualStyleBackColor = true;
            this.buttonWritings.Click += new System.EventHandler(this.buttonWritings_Click);
            // 
            // buttonTgOn2Sleep
            // 
            this.buttonTgOn2Sleep.Location = new System.Drawing.Point(1023, 190);
            this.buttonTgOn2Sleep.Name = "buttonTgOn2Sleep";
            this.buttonTgOn2Sleep.Size = new System.Drawing.Size(229, 23);
            this.buttonTgOn2Sleep.TabIndex = 11;
            this.buttonTgOn2Sleep.Text = "Total Toggles On 2 Sleep";
            this.buttonTgOn2Sleep.UseVisualStyleBackColor = true;
            this.buttonTgOn2Sleep.Click += new System.EventHandler(this.buttonTgOn2Sleep_Click);
            // 
            // buttonTgSleep2On
            // 
            this.buttonTgSleep2On.Location = new System.Drawing.Point(1023, 219);
            this.buttonTgSleep2On.Name = "buttonTgSleep2On";
            this.buttonTgSleep2On.Size = new System.Drawing.Size(229, 23);
            this.buttonTgSleep2On.TabIndex = 12;
            this.buttonTgSleep2On.Text = "Total Toggles Sleep 2 On";
            this.buttonTgSleep2On.UseVisualStyleBackColor = true;
            this.buttonTgSleep2On.Click += new System.EventHandler(this.buttonTgSleep2On_Click);
            // 
            // buttonDutyCycle
            // 
            this.buttonDutyCycle.Location = new System.Drawing.Point(1023, 248);
            this.buttonDutyCycle.Name = "buttonDutyCycle";
            this.buttonDutyCycle.Size = new System.Drawing.Size(229, 23);
            this.buttonDutyCycle.TabIndex = 13;
            this.buttonDutyCycle.Text = "Duty Cycle";
            this.buttonDutyCycle.UseVisualStyleBackColor = true;
            this.buttonDutyCycle.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCountersReport
            // 
            this.buttonCountersReport.Location = new System.Drawing.Point(1023, 395);
            this.buttonCountersReport.Name = "buttonCountersReport";
            this.buttonCountersReport.Size = new System.Drawing.Size(229, 23);
            this.buttonCountersReport.TabIndex = 14;
            this.buttonCountersReport.Text = "Counters Report";
            this.buttonCountersReport.UseVisualStyleBackColor = true;
            this.buttonCountersReport.Click += new System.EventHandler(this.buttonCounterReports_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(13, 31);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(115, 20);
            this.labelTitle.TabIndex = 15;
            this.labelTitle.Text = "Power Result";
            // 
            // labelN
            // 
            this.labelN.AutoSize = true;
            this.labelN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelN.Location = new System.Drawing.Point(13, 51);
            this.labelN.Name = "labelN";
            this.labelN.Size = new System.Drawing.Size(20, 20);
            this.labelN.TabIndex = 16;
            this.labelN.Text = "N";
            this.labelN.Click += new System.EventHandler(this.label1_Click);
            // 
            // textBoxCountersReport
            // 
            this.textBoxCountersReport.Location = new System.Drawing.Point(1023, 424);
            this.textBoxCountersReport.Multiline = true;
            this.textBoxCountersReport.Name = "textBoxCountersReport";
            this.textBoxCountersReport.Size = new System.Drawing.Size(229, 150);
            this.textBoxCountersReport.TabIndex = 17;
            // 
            // buttonExportCsv
            // 
            this.buttonExportCsv.Location = new System.Drawing.Point(1023, 366);
            this.buttonExportCsv.Name = "buttonExportCsv";
            this.buttonExportCsv.Size = new System.Drawing.Size(229, 23);
            this.buttonExportCsv.TabIndex = 18;
            this.buttonExportCsv.Text = "Show as CSV";
            this.buttonExportCsv.UseVisualStyleBackColor = true;
            this.buttonExportCsv.Click += new System.EventHandler(this.buttonExportCsv_Click);
            // 
            // buttonClipboard
            // 
            this.buttonClipboard.Location = new System.Drawing.Point(1023, 337);
            this.buttonClipboard.Name = "buttonClipboard";
            this.buttonClipboard.Size = new System.Drawing.Size(229, 23);
            this.buttonClipboard.TabIndex = 19;
            this.buttonClipboard.Text = "Copy data to clipbloard";
            this.buttonClipboard.UseVisualStyleBackColor = true;
            this.buttonClipboard.Click += new System.EventHandler(this.buttonClipboard_Click);
            // 
            // ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 601);
            this.Controls.Add(this.buttonClipboard);
            this.Controls.Add(this.buttonExportCsv);
            this.Controls.Add(this.textBoxCountersReport);
            this.Controls.Add(this.labelN);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.buttonCountersReport);
            this.Controls.Add(this.buttonDutyCycle);
            this.Controls.Add(this.buttonTgSleep2On);
            this.Controls.Add(this.buttonTgOn2Sleep);
            this.Controls.Add(this.buttonWritings);
            this.Controls.Add(this.buttonReadings);
            this.Controls.Add(this.buttonPowerOn);
            this.Controls.Add(this.buttonSleep);
            this.Controls.Add(this.panel2);
            this.Name = "ReportForm";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.ReportForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonSleep;
        private System.Windows.Forms.Button buttonPowerOn;
        private System.Windows.Forms.Button buttonReadings;
        private System.Windows.Forms.Button buttonWritings;
        private System.Windows.Forms.Button buttonTgOn2Sleep;
        private System.Windows.Forms.Button buttonTgSleep2On;
        private System.Windows.Forms.Button buttonDutyCycle;
        private System.Windows.Forms.Button buttonCountersReport;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelN;
        private System.Windows.Forms.TextBox textBoxCountersReport;
        private System.Windows.Forms.Button buttonExportCsv;
        private System.Windows.Forms.Button buttonClipboard;
    }
}