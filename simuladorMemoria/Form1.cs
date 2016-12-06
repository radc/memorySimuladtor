using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace memorySimulator
{
    public partial class FormInputPrefix : Form
    {
        public FormInputPrefix()
        {
            InitializeComponent();
        }
        List<Label> listLaberPower = new List<Label>(12*12);
        List<Label> listLabelRead = new List<Label>(12 * 12 * 16);
        Memory mem = new Memory();


        SystemControl control;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.radioButtonME.Checked = true;

            Console.WriteLine("CHEGUEI AQUI");
            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    Label lbl = new Label();
                    lbl.Size = new Size(38, 38);
                    lbl.Location = new Point(j * 40, i * 40);
                    lbl.BackColor = Color.Black;
                    lbl.Visible = true;
                    panel2.Controls.Add(lbl);
                    listLaberPower.Insert((int)Constants.memoryNumberOfSectors * i + j, lbl);
                }
            }

            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    for (int k = 0; k < Constants.linesPerMemoryBlock; k++)
                    {
                        Label lbl = new Label();
                        lbl.Size = new Size(38, 1);
                        lbl.Location = new Point(j * 40, i * 40 + k * 2);
                        lbl.BackColor = Color.Black;
                        lbl.Visible = true;
                        panel3.Controls.Add(lbl);
                        listLabelRead.Insert(((int)Constants.memoryNumberOfBanks * (int)Constants.linesPerMemoryBlock) * i + (int)Constants.linesPerMemoryBlock * j + k, lbl); 
                    }
                    
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchRange sr = new SearchRange(int.Parse(this.textBoxLTH.Text), int.Parse(this.textBoxLTV.Text), int.Parse(this.textBoxRBH.Text), int.Parse(this.textBoxRBV.Text));
            CodingBlock cb = new CodingBlock(int.Parse(this.textBoxSize.Text), int.Parse(this.textBoxPosX.Text), int.Parse(this.textBoxPosY.Text), sr);

            mem.definePowerOnRange(cb);

            for (int i = 0; i < Constants.memoryNumberOfSectors; i++)
            {
                for (int j = 0; j < Constants.memoryNumberOfBanks; j++)
                {
                    listLaberPower[(int)Constants.memoryNumberOfBanks * i + j].BackColor = (mem.memoryStatus[i][j] == PowerStatus.POWER_ON ? Color.LightBlue : Color.Gray);
                }
            }

            /*
            for (int i = 4; i < 8; i++)
            {
                for (int j = 4; j < 8; j++)
                {
                    listLaberPower[12 * i + j].BackColor = Color.Blue;
                }
            }

            for (int i = cb.posYinCtu / 16 + 4; i < cb.posYinCtu / 16 + 4+ cb.size / 16; i++)
            {
                for (int j = cb.posXinCtu / 16 + 4; j < cb.posXinCtu / 16 + 4 + cb.size / 16; j++)
                {
                    listLaberPower[12 * i + j].BackColor = Color.DarkBlue;
                }
            }

            */

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonRead_Click(object sender, EventArgs e)
        {

            SearchRange sr = new SearchRange(int.Parse(this.textBoxLTH.Text), int.Parse(this.textBoxLTV.Text), int.Parse(this.textBoxRBH.Text), int.Parse(this.textBoxRBV.Text));
            CodingBlock cb = new CodingBlock(int.Parse(this.textBoxSize.Text), int.Parse(this.textBoxPosX.Text), int.Parse(this.textBoxPosY.Text), sr);
            mem.clearMemoryCounters();
            try
            {
                mem.increaseRegionReading(cb, int.Parse(this.textBoxTZx.Text), int.Parse(this.textBoxTZy.Text));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                return;
            }
            

            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    for (int k = 0; k < 16; k++)
                    {
                        listLabelRead[(12 * 16) * i + 16 * j + k].BackColor = (mem.totalReadings[i][j][k] == 1 ? Color.Red : Color.Black);
                    }                    
                }
            }

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                bool isDe = radioButtonDE.Checked;

                control = new SystemControl(textBoxTracePath.Text, isDe);
                //control.scrollEntireFile();

                Console.WriteLine("Here we are!");
                control.startMemorySimulation();
                Console.WriteLine("Done!");

                if (isDe)
                {
                    control.Mem.clearNonDeCounter();
                    control.testMemory();
                }


                ReportForm rf = new ReportForm(control);
                rf.Show();

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }
            
            

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButtonME_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonME.Checked)
            {
                Constants.setMeClocks();
                //MessageBox.Show("Set ME!");
            }
        }

        private void radioButtonDE_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButtonDE.Checked)
            {
                Constants.setDeClocks();
                //MessageBox.Show("Set DE!");
            }
                
        }

        private void radioButton1024_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1024.Checked)
            {
                Constants.videoWidth = 1024;
                Constants.videoHeight = 768;
                MessageBox.Show("Set 1024x768 as video resolution!");
            }

        }

        private void radioButton1920_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioButton1920.Checked)
            {
                Constants.videoWidth = 1920;
                Constants.videoHeight = 1088;
                MessageBox.Show("Set 1920x1088 as video resolution!");
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void textBoxQP_TextChanged(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string path = "";
            path += this.textBoxCaminho.Text;
            path += this.textBoxVersion.Text;
            path += this.textBoxVideo.Text;
            path += this.textBoxQP.Text;
            path += "_";
            path += this.textBoxSearch.Text;
            path += "_";
            path += this.textBoxCh.Text;
            path += "_";
            path += this.textBoxVIdx.Text;
            path += ".csv";

            this.textBoxTracePath.Text = path;

            if (this.textBoxVideo.Text == "balloons") this.radioButton1024.Checked = true;
            if (this.textBoxVideo.Text == "kendo") this.radioButton1024.Checked = true;
            if (this.textBoxVideo.Text == "newspaper") this.radioButton1024.Checked = true;

            if (this.textBoxVideo.Text == "gtfly") this.radioButton1920.Checked = true;
            if (this.textBoxVideo.Text == "shark") this.radioButton1920.Checked = true;
            if (this.textBoxVideo.Text == "poznanstreet") this.radioButton1920.Checked = true;
            if (this.textBoxVideo.Text == "poznanhall") this.radioButton1920.Checked = true;
            if (this.textBoxVideo.Text == "dancer") this.radioButton1920.Checked = true;

            if (this.textBoxSearch.Text == "ME") this.radioButtonME.Checked = true;
            if (this.textBoxSearch.Text == "DE") this.radioButtonDE.Checked = true;
        }

        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {

        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxSearch_Leave(object sender, EventArgs e)
        {
            
        }

        private void textBoxVideo_Leave(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            try
            {
                StreamReader file = new StreamReader(textBoxMasterTrace.Text);
                StreamWriter file2 = new StreamWriter(this.textBoxOutFile.Text);
                string totais = "";
                while (!file.EndOfStream)
                {
                    string str = file.ReadLine();
                    Console.WriteLine("PROCESSANDO LINHA: " + this.textBoxPrefix.Text + str);
                    if (str.Contains("ME"))
                    {
                        this.radioButtonME.Checked = true;
                    }

                    else
                    {
                        if (str.Contains("DE"))
                        {
                            this.radioButtonDE.Checked = true;
                            if (str.Contains("_0."))
                            {
                                totais += "0 0 0 0 0 0\n";
                                continue;
                            }

                        }
                        else
                        {
                            throw new Exception("ME/DE error");
                        }
                    }
                    totais += manda(this.textBoxPrefix.Text + str) + "\n";
                }
                file.Close();
                file2.Write(totais);
                file2.Close();

                MessageBox.Show("WELL DONE!");

            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

            
        }

        private string manda(string str)
        {
            string cpData = "";
            try
            {
                bool isDe = radioButtonDE.Checked;

                control = new SystemControl(str, isDe);
                //control.scrollEntireFile();
                control.Mem.clearMemoryCounters();
                Console.WriteLine("Here we are!");
                control.startMemorySimulation();
                Console.WriteLine("Done!");

                if (isDe)
                {
                    control.Mem.clearNonDeCounter();
                    control.testMemory();
                }

                
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

                
            }
            catch (Exception exp)
            {
                MessageBox.Show(exp.Message);
            }

            return cpData;
        }

    }
}
