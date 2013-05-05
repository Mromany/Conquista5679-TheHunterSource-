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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }
        private static bool Exists(string username)
        {
            try
            {
               /* AccountCollection accounts = new AccountCollection();
                accounts.LoadAndCloseReader(Account.FetchByParameter("Username", username));
                if (accounts.Count > 0)
                {
                    return true;

                }*/
            }
            catch
            {
            }
            return false;
        }
        private void button5_Click(object sender, EventArgs e)
        {

            byte state = 0;
            if (comboBox1.Text == "Project Manager")
            {

                state = 4;
            }
            else if (comboBox1.Text == "Game Master")
            {
                state = 3;
            }
            else if (comboBox1.Text == "Player")
            {
                state = 2;
            }

            else if (comboBox1.Text == "NotActivited")
            {
                state = 100;
            }
            else
            {
                MessageBox.Show("you are not choose any Choice, you should choose any choice to continue register");
                return;
            }
            if (textBox10.Text == "")
            {
                textBox10.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (textBox10.Text != "")
            {
                textBox10.BackColor = System.Drawing.Color.White;
            }
            if (textBox9.Text == "")
            {
                textBox9.BackColor = System.Drawing.Color.Red;
                return;
            }
            if (textBox9.Text != "")
            {
                textBox9.BackColor = System.Drawing.Color.White;
            }
            if (!Exists(textBox10.Text))
            {
               // Account.Insert(textBox10.Text, textBox9.Text, "127.0.0.1", (ulong)0, (byte)state, 0, textBox1.Text);
                label16.Visible = true;
            }
            else
            {
                label16.ForeColor = Color.Red;
                label16.Text = "Account Name Is Exit";
                label16.Visible = true;
            }
         
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
      
    }
}
