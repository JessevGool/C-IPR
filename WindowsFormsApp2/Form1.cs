using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IPRRepo
{
    public partial class Form1 : Form
    {
        Stopwatch stopwatch;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
           stopwatch = new Stopwatch();
        }


        private void Startbtn_Click(object sender, EventArgs e)
        {
            stopwatch.Start();
            timer1.Start();
        }
        private void Stopbtn_Click(object sender, EventArgs e)
        {
            stopwatch.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string timeString = $"Elapsed time: {stopwatch.Elapsed}";
            Timelbl.Text = timeString;
            double elapsedTime = stopwatch.Elapsed.TotalSeconds;
            if (elapsedTime < 120)
            {
                twominlbl.Text = $"Warming up: {120 - elapsedTime}";
            }
            else if(elapsedTime > 120 && elapsedTime < 360)
            {
                twominlbl.Text = $"Astrand Test: {360 - elapsedTime}";
            }
            else if(elapsedTime > 360 && elapsedTime < 420)
            {
                twominlbl.Text = $"Cool down: {420 - elapsedTime}";
            }
            else if(elapsedTime > 420)
            {
                twominlbl.Text = "You can stop now";
            }
        }

      
    }
}
