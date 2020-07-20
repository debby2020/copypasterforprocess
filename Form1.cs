using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CopyPasteForUnallowedGamesetc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Size = new Size(447,208);
            panel2.Location = new Point(1,40);
            this.BackColor = Color.White;
            foreach(var a in Process.GetProcesses())
            {
                this.listBox1.Items.Add(a.ProcessName + " (PID: "+a.Id+ ")");
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (var a in Process.GetProcesses())
            {
                this.listBox1.Items.Add(a.ProcessName + " (PID: " + a.Id + ")");
            }
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            if(listBox1.SelectedItem == null)
            {
                MessageBox.Show("Cannot select process.","Warning!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            else
            {
                string selected = Regex.Replace(listBox1.SelectedItem.ToString(), "[0-9]", "").Replace("(PID: )","").Replace(" ", "");
                MessageBox.Show("Process Selected: " + selected, "Selected Process", MessageBoxButtons.OK, MessageBoxIcon.Information);
                processname = selected;
                await Task.Delay(450);
                panel2.Visible = false;
                timer1.Start();
            }
        }
        [DllImport("user32.dll")]
        internal static extern IntPtr SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        internal static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        string processname;
        private async void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == (Keys.Control | Keys.V))
            { 
                IntPtr hWnd;
                Process[] processRunning = Process.GetProcesses();
                foreach (Process pr in processRunning)
                {
                    string paste = Clipboard.GetText();
                    if (pr.ProcessName == processname)
                    {
                        hWnd = pr.MainWindowHandle;
                        //ShowWindow(hWnd, 3);
                        SetForegroundWindow(hWnd);
                        string now = paste;
                        await Task.Delay(350);
                        for (int i = 0; i<paste.Length; i++)
                        {
                            await Task.Delay(5);
                            char xef = now[i];
                            SendKeys.SendWait(xef.ToString());
                            await Task.Delay(5);
                        }
                    }
                }
            }
        }

        private void guna2HtmlLabel1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        private bool checkProcess(string text)
        {
            Process[] a = Process.GetProcessesByName(text);
            if(a.Length >0)
            {
                return false;
            }
            else
            {
                timer1.Stop();
                MessageBox.Show("Your process stopped working application restarting!","Upps!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                Application.Restart();
                return true;
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            checkProcess(processname);
        }
    }
}
