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

namespace labBrandCarTable
{
    public partial class ProgressBarLoadingForm : Form
    {
        public ProgressBarLoadingForm()
        {
            InitializeComponent();
        }
        public string BrandName { get; set; }
        public Socket Socket {  get; set; }

        private void ProgressBarLoadingForm_Load(object sender, EventArgs e)
        {
            progressBar1.Style = ProgressBarStyle.Blocks;

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;

            timerDataLoading.Interval = 100;
            timerDataLoading.Start();
        }

        private void timerDataLoading_Tick(object sender, EventArgs e)
        {   
            string brandName = BrandName;

            progressBar1.Value = Loader.GetProgress(brandName);

            if (progressBar1.Value >= 100)
            {
                timerDataLoading.Stop();
            }
        }

        private void SocketSendMessage(string message)
        {
            byte[] bytes = new byte[1024];

            byte[] msg = Encoding.UTF8.GetBytes(message);

            // Отправляем данные через сокет
            Socket socket = Socket;
            int bytesSent = socket.Send(msg);
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }
    }
}
