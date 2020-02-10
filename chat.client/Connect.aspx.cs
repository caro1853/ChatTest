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
    public partial class Connect : System.Web.UI.Page
    {              
        private static List<ManageArray> manageArrayAll;        
        private byte[] buffer;
        private static string Mensajes;

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    if(!string.IsNullOrEmpty( Mensajes))
                    {
                        lblMessagesError.Text = Mensajes;
                        btnCleanMessageError.Visible = true;
                    }
                    Conectarse();
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
        }

        private void EscribirMensaje(string mensaje)
        {
            Mensajes += mensaje + Environment.NewLine;
        }

        protected void btnJoin_Click(object sender, EventArgs e)
        {
            try
            {
                Conectarse();                
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                MessageTransport messageTransport = new MessageTransport();
                messageTransport._typeObject = Enums.TypeObject.NormalMessage;
                messageTransport.Message = this.txtMensaje.Text;

                SendInformationToServer(MessageTransportToByte(messageTransport));
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
        }

        protected void Unnamed2_Tick(object sender, EventArgs e)
        {
            try
            {
                ManageMessages();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
        }

        protected void btnJoinRoom_Click(object sender, EventArgs e)
        {
            try
            {
                MessageTransport objMessageTransport = new MessageTransport();
                objMessageTransport._typeObject = Enums.TypeObject.NewUser;
                objMessageTransport.NickName = this.txtNickName.Text;
                if (string.IsNullOrEmpty(this.txtSala.Text))
                {
                    if (string.IsNullOrEmpty(this.lstRooms.SelectedValue))
                    {
                        objMessageTransport.Room = "Home";
                    }
                    else
                    {
                        objMessageTransport.Room = this.lstRooms.SelectedValue;
                    }
                }
                else
                {
                    objMessageTransport.Room = this.txtSala.Text;
                }

                SendInformationToServer(MessageTransportToByte(objMessageTransport));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
        }

        protected void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                ManageMessages();
                //Timmer1.Enabled = Timmer1.Enabled ? false : true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
        }

        #endregion Events

        #region Methods
        private void Conectarse()
        {
            //if (Variables.clientSocket == null)
            {
                Variables.clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);                
            }
            
            var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 100);
            Variables.clientSocket.BeginConnect(endPoint, ConnectCallback, null);
        }

        private void ManageMessages()
        {
            byte[] bufferTransport;

            if (manageArrayAll != null && manageArrayAll.Count > 0)
            {
                var man = manageArrayAll.FirstOrDefault(p => p.InUse == false);

                if (man != null)
                {
                    man.InUse = true;
                    bufferTransport = man.buffer;

                    if (bufferTransport != null)
                    {
                        MessageTransport objMessageTransport = ByteToMessageTransport(bufferTransport);

                        if (objMessageTransport != null)
                        {
                            switch (objMessageTransport._typeObject)
                            {
                                case Enums.TypeObject.Room:
                                    this.lstRooms.DataSource = objMessageTransport.LstRooms;
                                    this.lstRooms.DataBind();
                                    this.lblInfo.Text = objMessageTransport.Message;
                                    break;

                                case Enums.TypeObject.NormalMessage:
                                    this.txtAllMessages.Text += objMessageTransport.Message + Environment.NewLine;
                                    break;
                                case Enums.TypeObject.FromServer:
                                case Enums.TypeObject.Rejected:
                                    this.lblInfo.Text = objMessageTransport.Message;
                                    break;
                            }
                        }
                    }
                    manageArrayAll.Remove(man);
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

        #endregion Methods


        #region Asyn Events With Server
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
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
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
                        buffer = buffer
                    });
                }

                // Start receiving data again.
                buffer = new byte[Variables.clientSocket.ReceiveBufferSize];
                Variables.clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }            
            catch (SocketException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
        }

        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                Variables.clientSocket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
                EscribirMensaje(ex.Message);
            }
        }
        #endregion Asyn Events With Server

        protected void btnCleanMessageError_Click(object sender, EventArgs e)
        {
            lblMessagesError.Text = string.Empty;
            btnCleanMessageError.Visible = false;
        }
    }
}