using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PhoenixProject
{
    public partial class Server : Form
    {
        public static DateTime StartDate = DateTime.Now;
        public static bool InvisibleState = false;
        public string AllText = "";
        public Boolean Changed;

        Timer update = new Timer();
        bool Console = false;
        public Server()
        {
            InitializeComponent();
            update.Interval = 1000;
            update.Tick += new EventHandler(update_Tick);
            update.Start();
            timer1.Interval = 100;
            timer1.Tick += new EventHandler(timer1_Tick);
            timer1.Start();
        }

        void update_Tick(object sender, EventArgs e)
        {
            var proc = System.Diagnostics.Process.GetCurrentProcess();
            txtOnline.Text = PhoenixProject.ServerBase.Kernel.GamePool.Count + " players";
            label12.Text = PhoenixProject.ServerBase.Kernel.Maps.Count + " Map";
            label6.Text = PhoenixProject.ServerBase.Constants.ServerName2 + "";
            label7.Text = PhoenixProject.ServerBase.Kernel.Guilds.Count + " Guilds";
            label8.Text = PhoenixProject.ServerBase.Kernel.Clans.Count + " Clan";
            label9.Text = ((double)((double)proc.WorkingSet64 / 1024)) / 1024 + " MB";
            label10.Text = Program.StartDate.ToString("dd MM yyyy hh:mm") + " GMT";
            label13.Text = Convert.ToString(proc.Threads.Count);
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Program.CommandsAI("@exit");
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            Program.CommandsAI("@restart");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IntPtr hWnd = Program.FindWindow(null, PhoenixProject.Console.Title);
            if (hWnd != IntPtr.Zero)
            {
                if (!Console)
                {
                    Program.ShowWindow(hWnd, 1);
                    Console = true;
                }
                else
                {
                    Program.ShowWindow(hWnd, 0);
                    Console = false;
                }
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (ServerStarted.Visible != InvisibleState)
            {
                ServerStarted.Visible = InvisibleState;
            }
            if (ServerStarted.ForeColor == Color.Black)
            {
                ServerStarted.ForeColor = Color.Red;
                goto jump;
            }
            else if (ServerStarted.ForeColor == Color.Red)
            {
                ServerStarted.ForeColor = Color.Green;
                goto jump;
            }
            if (ServerStarted.ForeColor == Color.Green)
            {
                ServerStarted.ForeColor = Color.Blue;
                goto jump;
            }
            else
            {
                ServerStarted.ForeColor = Color.Black;
                goto jump;
            }
        jump:
            if (Changed)
            {
                this.txt_console.Text = AllText;
                Changed = false;
            }
        }

        private void txt_console_TextChanged(object sender, EventArgs e)
        {
            txt_console.SelectionStart = txt_console.Text.Length;
            txt_console.ScrollToCaret();
        }

        public static void WriteLine(string Text)
        {
            string mynewtext = Program.GUI.AllText + "\r\n";
            if (Program.GUI.AllText != "")
                mynewtext += Text;
            else
                mynewtext = Text;
            Program.GUI.AllText = mynewtext;
            Program.GUI.Changed = true;
        }

        public static void WriteLine2(string Text)
        {
            string mynewtext = Program.GUI.AllText + "\r\n";
            if (Program.GUI.AllText != "")
                mynewtext += Text;
            else
                mynewtext = Text;
            Program.GUI.AllText = mynewtext;
            Program.GUI.Changed = true;
        }

        private void Server_Load(object sender, EventArgs e)
        {
            Program.StartEngine();
        }

        private void lblOnline_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            txt_console.SelectionStart = txt_console.Text.Length;
            txt_console.ScrollToCaret();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Program.CommandsAI(Command.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {

            DateTime now = DateTime.Now;
            TimeSpan t2 = new TimeSpan(StartDate.ToBinary());
            TimeSpan t1 = new TimeSpan(now.ToBinary());
            MessageBox.Show("The server has been online " + (int)(t1.TotalHours - t2.TotalHours) + " hours, " + (int)((t1.TotalMinutes - t2.TotalMinutes) % 60) + " minutes.");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.CommandsAI("@exit");
        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.CommandsAI("@restart");
        }

        private void controlToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void npcMakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.Show();
        }

        private void controlToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Control control = new Control();
            control.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void Command_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
