using chat.client.Common;
using chat.core.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace chat.client
{
    public partial class ManageConnectedUsers : System.Web.UI.Page
    {               
        private static List<ManageArray> manageArrayAll;        
        private byte[] buffer;
        private static string Mensajes;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Mensajes))
                {
                    lblMessagesError.Text = Mensajes;
                    btnCleanMessageError.Visible = true;
                }

                if (!Page.IsPostBack)
                {
                    

                    Conectarse();
                }
            }
            catch (SocketException ex)
            {
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {                
                EscribirMensaje(ex.Message);
            }
        }

        private void EscribirMensaje(string mensaje)
        {
            Mensajes += mensaje + Environment.NewLine;
        }

        private void Conectarse()
        {
            //if (Variables.clientSocket == null)
            {
                Variables.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 100);
                Variables.clientSocket.BeginConnect(endPoint, ConnectCallback, null);
            }
        }

        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                Variables.clientSocket.EndConnect(AR);                
                buffer = new byte[Variables.clientSocket.ReceiveBufferSize];
                Variables.clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {
                EscribirMensaje(ex.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = Variables.clientSocket.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }

                if (manageArrayAll == null)
                {
                    manageArrayAll = new List<ManageArray>();
                }

                if (buffer != null)
                {
                    manageArrayAll.Add(new ManageArray()
                    {
                        buffer = buffer,
                        MessageTransport = ByteToMessageTransport(buffer)
                    });
                }

                // Start receiving data again.
                buffer = new byte[Variables.clientSocket.ReceiveBufferSize];
                Variables.clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            // Avoid Pokemon exception handling in cases like these.
            catch (SocketException ex)
            {
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {
                EscribirMensaje(ex.Message);
            }
        }

        private void ManageMessages()
        {
            byte[] bufferTransport;

            if (manageArrayAll != null && manageArrayAll.Count > 0)
            {
                var man = manageArrayAll.FirstOrDefault(p =>                     
                    p.MessageTransport != null && 
                    p.MessageTransport._typeObject == Enums.TypeObject.ConnectedUsers);

                if (man != null)
                {
                    man.InUse = true;
                    bufferTransport = man.buffer;

                    if (bufferTransport != null)
                    {
                        MessageTransport objMessageTransport = man.MessageTransport;

                        if (objMessageTransport != null)
                        {
                            switch (objMessageTransport._typeObject)
                            {
                                case Enums.TypeObject.ConnectedUsers:
                                    this.grvUsers.DataSource = objMessageTransport.ConnectedUsers;
                                    this.grvUsers.DataBind();
                                    break;                                
                            }
                        }
                    }

                    //manageArrayAll.Remove(man);
                }
            }
        }

        private MessageTransport ByteToMessageTransport(byte[] data)
        {
            try
            {
                string strMessageTransport = Encoding.ASCII.GetString(data);
                MessageTransport objMessageTransport = JsonConvert.DeserializeObject<MessageTransport>(strMessageTransport);
                return objMessageTransport;
            }
            catch
            {
                return null;
            }
        }

        private byte[] MessageTransportToByte(MessageTransport messageTransport)
        {
            string strobjMessageUser = JsonConvert.SerializeObject(messageTransport);
            byte[] bM = Encoding.ASCII.GetBytes(strobjMessageUser);
            return bM;
        }

        protected void SendInformationToServer(byte[] buffer)
        {
            Variables.clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }

        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                Variables.clientSocket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {
                EscribirMensaje(ex.Message);
            }
        }

        protected void lnkSelectUser_Click(object sender, EventArgs e)
        {
            try
            {
                string nickName = (sender as LinkButton).CommandName;

                MessageTransport messageTransport = new MessageTransport();
                messageTransport._typeObject = Enums.TypeObject.DisconnectUser;
                messageTransport.NickName = nickName;

                SendInformationToServer(MessageTransportToByte(messageTransport));
            }
            catch (Exception ex)
            {
                EscribirMensaje(ex.Message);
            }
        }

        protected void btnConnectedUsers_Click(object sender, EventArgs e)
        {
            try
            {
                ManageMessages();
            }
            catch (Exception ex)
            {
                EscribirMensaje(ex.Message);
            }
        }

        protected void btnGetUser_Click(object sender, EventArgs e)
        {
            try
            {
                MessageTransport messageTransport = new MessageTransport();
                messageTransport._typeObject = Enums.TypeObject.ConnectedUsers;
                SendInformationToServer(MessageTransportToByte(messageTransport));
                EscribirMensaje("Esperando respuesta del servidor");
            }
            catch (Exception ex)
            {
                EscribirMensaje(ex.Message);
            }
        }

        protected void btnJoin_Click(object sender, EventArgs e)
        {
            try
            {
                Conectarse();
            }
            catch (Exception ex)
            {
                EscribirMensaje(ex.Message);
            }
        }

        protected void btnCleanMessageError_Click(object sender, EventArgs e)
        {
            lblMessagesError.Text = string.Empty;
            btnCleanMessageError.Visible = false;
        }
    }
}