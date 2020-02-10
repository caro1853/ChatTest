using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace chat.client
{
    public partial class Start : System.Web.UI.Page
    {
        private static Socket ClientSocket ;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectToServer();
        }


        private void ConnectToServer()
        {
            int attempts = 0;
            ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            while (!ClientSocket.Connected)
            {
                try
                {
                    attempts++;
                    Console.WriteLine("Connection attempt " + attempts);
                    // Change IPAddress.Loopback to a remote IP to connect to a remote host.
                    ClientSocket.Connect(IPAddress.Loopback, 100);
                }
                catch (SocketException)
                {
                    Console.Clear();
                }
            }

            this.lblMen.Text = "Conectado";

            Thread receive = new Thread(Receive);
            receive.Start();
        }

        private void Receive()
        {
            while(true)
            {
                ReceiveResponse();
            }
        }


        private void ReceiveResponse()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            this.lblMen.Text = text;
            
        }
    }
}