using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoctorApplication
{
    public partial class DoctorGui : Form
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static string _prefixmessage = "doctor/message##";
        private static string _prefixfile = "doctor/file##";
        
        public DoctorGui()
        {
            InitializeComponent();
        }

        private void Getlogsbtn_Click(object sender, EventArgs e)
        {

        }
    }
}
