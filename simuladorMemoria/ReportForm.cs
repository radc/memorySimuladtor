using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace memorySimulator
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        public ReportForm(SystemControl control)
        {
            InitializeComponent();
            // TODO: Complete member initialization
            this.control = control;
        }

        List<Label> listLaberPower = new List<Label>(12 * 12);
        TextBox txt = new TextBox();
        private SystemControl control;


        private void ReportForm_Load(object sender, EventArgs e)
        {   
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    Label lbl = new Label();
                    lbl.Size = new Size(78, 38);
                    lbl.Location = new Point(j * 80, i * 40);
                    lbl.BackColor = Color.Black;
                    lbl.Visible = true;
                    panel2.Controls.Add(lbl);
                    listLaberPower.Insert(12 * i + j, lbl);
                }
            }

            txt.Multiline = true;            
            txt.Location = new Point(0, 0);
            txt.Visible = false;
            txt.Size = panel2.Size;
            panel2.Controls.Add(txt);

        }

        private void buttonSleep_Click(object sender, EventArgs e)
        {
            txt.Visible = false;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    listLaberPower[12 * i + j].Text = control.Mem.cyclesStaticPower[(int)PowerStatus.SLEEP][i][j].ToString();
                    listLaberPower[12 * i + j].BackColor = Color.White;
                    listLaberPower[12 * i + j].Visible = true;
                }
            }
            labelTitle.Text = "Total Sleep";
            labelN.Text = control.sumSleep.ToString("N0");
        }

        private void buttonPowerOn_Click(object sender, EventArgs e)
        {
            txt.Visible = false;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    listLaberPower[12 * i + j].Text = control.Mem.cyclesStaticPower[(int)PowerStatus.POWER_ON][i][j].ToString();
                    listLaberPower[12 * i + j].BackColor = Color.White;
                    listLaberPower[12 * i + j].Visible = true;
                }
            }
            labelTitle.Text = "Total Power On";
            labelN.Text = control.sumPowerOn.ToString("N0");
        }

        private void buttonReadings_Click(object sender, EventArgs e)
        {
            txt.Visible = false;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    ulong sum = 0;
                    for (int k = 0; k < 16; k++)
                    {
                        sum += control.Mem.totalReadings[i][j][k];
                    }
                    listLaberPower[12 * i + j].Text = sum.ToString();
                    listLaberPower[12 * i + j].BackColor = Color.White;
                    listLaberPower[12 * i + j].Visible = true;
                }
            }
            labelTitle.Text = "Total Readings";
            labelN.Text = control.sumReadings.ToString("N0");
        }
        
        private void buttonWritings_Click(object sender, EventArgs e)
        {
            txt.Visible = false;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    ulong sum = 0;
                    for (int k = 0; k < 16; k++)
                    {
                        sum += control.Mem.totalWritings[i][j][k];
                    }
                    listLaberPower[12 * i + j].Text = sum.ToString();
                    listLaberPower[12 * i + j].BackColor = Color.White;
                    listLaberPower[12 * i + j].Visible = true;
                }
            }
            labelTitle.Text = "Total Writings";
            labelN.Text = control.sumWritings.ToString("N0");
        }

        private void buttonTgOn2Sleep_Click(object sender, EventArgs e)
        {
            txt.Visible = false;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {   
                    listLaberPower[12 * i + j].Text = control.Mem.toogleOn2Sleep[i][j].ToString();
                    listLaberPower[12 * i + j].BackColor = Color.White;
                    listLaberPower[12 * i + j].Visible = true;
                }
            }
            labelTitle.Text = "Total Toggle On to Sleep";
            labelN.Text = control.sumToogleOn2Sleep.ToString("N0");
        }

        private void buttonTgSleep2On_Click(object sender, EventArgs e)
        {
            txt.Visible = false;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    listLaberPower[12 * i + j].Text = control.Mem.toogleSleep2On[i][j].ToString();
                    listLaberPower[12 * i + j].BackColor = Color.White;
                    listLaberPower[12 * i + j].Visible = true;
                }
            }
            labelTitle.Text = "Total Toggle Sleep to On";
            labelN.Text = control.sumToogleSleep2On.ToString("N0");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txt.Visible = false;
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    double sl, pw, sum;

                    pw = control.Mem.cyclesStaticPower[(int)PowerStatus.POWER_ON][i][j];
                    sl = control.Mem.cyclesStaticPower[(int)PowerStatus.SLEEP][i][j];

                    sum = pw + sl;

                    double percentage = (double)((100* pw) / (sum));

                    listLaberPower[12 * i + j].Text = percentage.ToString("F2") + "%";
                    listLaberPower[12 * i + j].BackColor = Color.White;
                    listLaberPower[12 * i + j].Visible = true;
                }
            }
            labelTitle.Text = "Duty Cycle";
            labelN.Text = (100 * (decimal)control.sumPowerOn / ((decimal)control.sumPowerOn + (decimal)control.sumSleep)).ToString("F2") + "%";
        }

        private void buttonCounterReports_Click(object sender, EventArgs e)
        {
            //ulong sumCyclesCb, sumCb, sumCtu, sumFrame, sumCtuSkip, sumIdleCycles; 

            this.textBoxCountersReport.Clear();
            this.textBoxCountersReport.Text += "Total Active Cycles = " + control.sumActiveCyclesCb.ToString("N0") + "\r\n";
            this.textBoxCountersReport.Text += "Total Idle Cycles = " + control.sumIdleCycles.ToString("N0") + "\r\n";
            this.textBoxCountersReport.Text += "Processed CB = " + control.sumCb.ToString("N0") + "\r\n";
            this.textBoxCountersReport.Text += "Processed CTU = " + control.sumCtu.ToString("N0") + "\r\n";
            this.textBoxCountersReport.Text += "Total CTU Skip = " + control.sumCtuSkip.ToString("N0") + "\r\n";
            this.textBoxCountersReport.Text += "Total Frames = " + control.sumFrame.ToString("N0") + "\r\n";
            this.textBoxCountersReport.Enabled = false;

            labelTitle.Text = "Power Results";
            labelN.Text = "N";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonExportCsv_Click(object sender, EventArgs e)
        {
            txt.Clear();
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    listLaberPower[12 * i + j].Visible = false;
                    txt.AppendText(listLaberPower[12 * i + j].Text + ((j==11)?"":", "));
                }
                if (i != 11) txt.AppendText("\r\n");
            }
            txt.Visible = true;
        }

        private void buttonClipboard_Click(object sender, EventArgs e)
        {
            string cpData;
            cpData = control.sumSleep.ToString();
            cpData += " ";
            cpData += control.sumPowerOn.ToString();
            cpData += " ";
            cpData += control.sumReadings.ToString();
            cpData += " ";
            cpData += control.sumWritings.ToString();
            cpData += " ";
            cpData += control.sumToogleOn2Sleep.ToString();
            cpData += " ";
            cpData += control.sumToogleSleep2On.ToString();

            Clipboard.SetText(cpData);
            MessageBox.Show("Data transfered to clipboard!");
        }
    }
}
