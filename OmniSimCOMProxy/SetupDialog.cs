using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ASCOM.LocalServer
{
    public partial class SetupDialog : Form
    {

        public SetupDialog()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("http://localhost:32323");
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);            
            }
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetupDialog_Load(object sender, EventArgs e)
        {
            // This is a workaround to ensure the form is shown if it is minimised or behind other windows
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
    }
}