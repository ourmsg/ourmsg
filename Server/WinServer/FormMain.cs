using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using IMLibrary3.Net;
using IMLibrary3.Net.IO;
using IMLibrary3.Net.TCP;
using IMLibrary3.Net.UDP;

namespace OurMsgServer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        Server server = new Server();
        private void Form2_Load(object sender, EventArgs e)
        {
             
        } 
    
        private void butStrat_Click(object sender, EventArgs e)
        {
            butStrat.Enabled = false;
            butStop.Enabled = true;
            server.Start(DBHelper.settings.UdpFilePort,DBHelper.settings.UdpAVPort , DBHelper.settings.TcpFilePort, DBHelper.settings.TcpMessagePort);
        }

        private void butStop_Click(object sender, EventArgs e)
        {
            butStrat.Enabled =true ;
            butStop.Enabled =false ;
            server.Stop();
        }


       
    }
}
