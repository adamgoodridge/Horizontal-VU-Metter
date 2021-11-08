using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VU_Metter
{
    public partial class Form1 : Form
    {
        private static readonly Color ColourHigh = Color.Red, ColourLow = Color.Green, ColourMid = Color.Yellow, ColourBack = Color.Black;
        private static readonly Boolean UP = false;
        private static int _columns;
        private static int _rows;
        private static readonly int _count = 20;
        private static readonly Double _lowLimit = 6, _midLimit = 9 + _lowLimit, _maxVolume = 80.0;
        private readonly Panel[,] panels = new Panel[2, _count];


        private MMDeviceEnumerator devEnum;
        private MMDevice defaultDevice;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "Adam's sound";
            this.Opacity = .7;
            this.TopMost = true;
            if(UP==false)
            {
                _columns = _count;
                _rows = 2;
            } else
            {
                _columns = 2;
                _rows = _count;
            }
            MainPanel.RowStyles.Clear();
            MainPanel.ColumnStyles.Clear();
            MainPanel.RowCount = _rows;
            MainPanel.ColumnCount = _columns;
            float columnPercent = (float)100 / _columns;
            float rowPercent = (float)100 / _rows;
            for (int i = 0; i < _rows; i++)
            {
                this.MainPanel.RowStyles.Add(new ColumnStyle(System.Windows.Forms.SizeType.Percent, rowPercent));
            }
            for (int i = 0; i < _columns; i++)
            {
                this.MainPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, columnPercent));
            }
            for (int i = 0; i < 2; i++)
            {
                for (int k = 0; k < _count; k++)
                {

                    panels[i, k] = new Panel
                    {
                        Dock = DockStyle.Fill,
                        Location = new System.Drawing.Point(403, 3)
                    };
                    panels[i, k].Size = new System.Drawing.Size(394, 204);
                    panels[i, k].TabIndex = 1;
                    if(UP == false)
                        MainPanel.Controls.Add(panels[i, k], k, i);
                    else
                        MainPanel.Controls.Add(panels[i, k], i, k);
                    panels[i, k].BackColor = Color.Black;
                    
                }
            }


        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            devEnum = new MMDeviceEnumerator();
            defaultDevice = devEnum.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            int levelLeft = (int)(defaultDevice.AudioMeterInformation.PeakValues[0] * 100 + 0.5);
            levelLeft = (int) Math.Ceiling((Double) (levelLeft / _maxVolume) *  _count);
            int levelRight = (int)(defaultDevice.AudioMeterInformation.PeakValues[1] * 100 + 0.5);
            levelRight = (int) Math.Ceiling((Double) (levelRight / _maxVolume) *  _count);
            int[] channels = { levelLeft, levelRight};
            for (int i = 0; i < channels.Length; i++)
            {
                Color color = ColourLow;
                for (int k = 0; k < _count; k++)
                {
                    double sum = k;
                    panels[i, k].BackColor = sum < channels[i] ? color : ColourBack;
                    if (sum > _midLimit)
                        color = ColourHigh;
                    else if (sum > _lowLimit)
                        color = ColourMid;
                }
            }
        }
    }
}





