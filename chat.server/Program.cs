using chat.core.Entities;
using chat.core.Interfaces;
using chat.server.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace chat.server
{
    class Program
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<SessionCurrent> clientSockets = new List<SessionCurrent>();
        private static List<string> ListRooms = new List<string>() { "Home" };
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 100;
        private static byte[] buffer = new byte[BUFFER_SIZE];
        private static IServiceCollection services = new ServiceCollection();

        static void Main()
        {
            
            Startup startup = new Startup();
            startup.ConfigureServices(services);           

            Console.Title = "Server";
            SetupServer();
            Console.ReadLine(); // When we press enter close everything
            CloseAllSockets();
        }

        #region Access to bd

        private static void CreateUser(User user)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<IUserService>();            
            service.Create(user);
        }

        private static void CreateRoom(Room room)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<IRoomService>();
            service.Create(room);
        }

        private static void CreateMessage(Message message)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<IMessageService>();
            service.Create(message);
        }

        private static void CreateSession(Session session)
        {
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            var service = serviceProvider.GetService<ISessionService>();
            service.Create(session);
        }

        #endregion Access to bd

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("Server setup complete");            
        }

        /// <summary>
        /// Close all connected client (we do not need to shutdown the server socket as its connections
        /// are already closed with the clients).
        /// </summary>
        private static void CloseAllSockets()
        {
            foreach (SessionCurrent session in clientSockets)
            {
                session.ObjSocket.Shutdown(SocketShutdown.Both);
                session.ObjSocket.Close();
            }

            serverSocket.Close();
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }
            SessionCurrent objSession = new SessionCurrent() { ObjSocket = socket };

            clientSockets.Add(objSession);
            byte[] b = new byte[socket.ReceiveBufferSize];
            string nombre = Encoding.ASCII.GetString(b);
            Array.Copy(buffer, b, BUFFER_SIZE);

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
            Console.WriteLine($"{nombre} connected, waiting for request...");                                    

            serverSocket.BeginAccept(AcceptCallback, null);

            SendRooms(socket);
        }

        private static void SendRooms(Socket socket)
        {
            MessageTransport objMessageTransport = new MessageTransport();
            objMessageTransport._typeObject = Enums.TypeObject.Room;
            objMessageTransport.LstRooms = ListRooms;//new List<string>() { "Principal 1", "Principal 2", "Principal 3" }; 
            objMessageTransport.Message = "Conectado";

            socket.Send(MessageTransportToByte(objMessageTransport));
        }

        private static void SendCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket= serverSocket.EndAccept(AR);
                socket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            try
            {                
                Socket current = (Socket)AR.AsyncState;
                int received;

                try
                {
                    received = current.EndReceive(AR);
                }
                catch (SocketException ex)
                {
                    Console.WriteLine("Client forcefully disconnected");
                    Console.WriteLine($"Error. {ex.Message}");
                    // Don't shutdown because the socket may be disposed and its disconnected anyway.
                    current.Close();
                    //clientSockets.Remove(current);
                    RemoveSession(current);
                    return;
                }

                byte[] recBuf = new byte[received];
                Array.Copy(buffer, recBuf, received);

                ManageMessages(recBuf, current);

                string text = Encoding.ASCII.GetString(recBuf);
                Console.WriteLine("Received Text: " + text);
                
                
                buffer = new byte[BUFFER_SIZE];
                current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
        }

        private static void RemoveSession(Socket socket)
        {

        }

        private static void ManageMessages(byte[] bufferTransport, Socket current)
        {
            MessageTransport objMessageUser;
            SessionCurrent currentSession;

            if (bufferTransport != null)
            {                
                MessageTransport objMessageTransport = ByteToMessageTransport(bufferTransport);

                switch (objMessageTransport._typeObject)
                {                 
                    case Enums.TypeObject.NormalMessage:

                        currentSession = CurrentSession(current, objMessageTransport);

                        //Create message bd
                        Message message = new Message()
                        {
                            SessionId = currentSession.SessionId,
                            MessageText = objMessageTransport.Message
                        };

                        if (currentSession.SessionId != 0)
                        {
                            CreateMessage(message);
                        }

                        //Excepto al actual
                        SendAllClients(MessageTransportToByte(objMessageTransport));

                        //this.txtAllMessages.Text += objMessageTransport.JsonObject;
                        break;
                    case Enums.TypeObject.NewUser:
                        if (FindCantSocketByUser(objMessageTransport.NickName) >= 1)
                        {
                            objMessageUser = new MessageTransport();
                            objMessageUser._typeObject = Enums.TypeObject.Rejected;
                            objMessageUser.Message = "El nombre de usuario ya está siendo usado.";                                                       
                            current.Send(MessageTransportToByte(objMessageUser));
                        }  
                        else
                        {
                            //Enviar al actual bienvenido
                            objMessageUser = new MessageTransport();
                            objMessageUser._typeObject = Enums.TypeObject.NormalMessage;
                            objMessageUser.Message = $"{objMessageTransport.NickName}. Ha ingresado";


                            currentSession = CurrentSession(current, objMessageTransport);                            

                            //Se crea usuario en bd y session y sala si no existe
                            Session session = new Session();
                            session.Room = new Room() { Name = objMessageTransport.Room };
                            session.User = new User() { Name = objMessageTransport.NickName };

                            CreateSession(session);

                            currentSession.SessionId = session.SessionId;

                            //Excepto al actual
                            SendAllClients(MessageTransportToByte(objMessageUser), currentSession.Room);

                            //Add the room
                            if (ListRooms.FirstOrDefault(p => p == objMessageTransport.Room) == null)
                            {
                                ListRooms.Add(objMessageTransport.Room);
                            }
                        }
                        break;
                    case Enums.TypeObject.ConnectedUsers:
                        objMessageUser = new MessageTransport();
                        objMessageUser._typeObject = Enums.TypeObject.ConnectedUsers;
                        objMessageUser.ConnectedUsers =
                            (from cs in clientSockets
                             where (!string.IsNullOrEmpty(cs.NickName))
                             select new Session
                             {
                                 User = new User() { Name = cs.NickName },
                                 Room = new Room() { Name = cs.Room }
                             }
                             ).ToList();

                        current.Send(MessageTransportToByte(objMessageUser));
                        break;
                    case Enums.TypeObject.DisconnectUser:

                        DisconnetUser(objMessageTransport.NickName);                        
                        break;
                }
            }
        }

        private static void DisconnetUser(string nickName)
        {
            SessionCurrent sessionDisconnect = GetSessionByNickName(nickName);
            MessageTransport objMessageUser = new MessageTransport();
            try
            {                              
                objMessageUser._typeObject = Enums.TypeObject.FromServer;
                objMessageUser.Message = "Ud será desconectado";
                sessionDisconnect.ObjSocket.Send(MessageTransportToByte(objMessageUser));

                Thread.Sleep(3000);

                sessionDisconnect.ObjSocket.Shutdown(SocketShutdown.Both);
                sessionDisconnect.ObjSocket.Close();
            }
            catch
            {
                sessionDisconnect.ObjSocket.Close();
            }
            finally
            {
                clientSockets.Remove(sessionDisconnect);
            }
        }


        private static SessionCurrent CurrentSession(Socket current, MessageTransport messageTransport)
        {
            SessionCurrent currentSession = null;

            foreach (var item in clientSockets)
            {
                if (current.Equals(item.ObjSocket))
                {
                    item.NickName = messageTransport.NickName;
                    item.Room = messageTransport.Room;
                    currentSession = item;
                    break;
                }
            }

            return currentSession;
        }

        private static SessionCurrent GetSessionByNickName(string nickName)
        {
            SessionCurrent currentSession = null;

            foreach (var item in clientSockets)
            {
                if (item.NickName == nickName)
                {                    
                    currentSession = item;
                    break;
                }
            }

            return currentSession;
        }

        private static byte[] MessageTransportToByte(MessageTransport messageTransport)
        {
            string strobjMessageUser = JsonConvert.SerializeObject(messageTransport);
            byte[] bM = Encoding.ASCII.GetBytes(strobjMessageUser);
            return bM;
        }

        private static MessageTransport ByteToMessageTransport(byte[] data)
        {
            string strMessageTransport = Encoding.ASCII.GetString(data);
            MessageTransport objMessageTransport = JsonConvert.DeserializeObject<MessageTransport>(strMessageTransport);
            return objMessageTransport;
        }


        private static int FindCantSocketByUser(string nickName)
        {
            int cant;

            cant = clientSockets.Count(p => p.NickName == nickName);

            return cant;
        }        

        private static void SendAllClients(byte[] data)
        {
            int count = clientSockets.Count;            

            for (int i = count-1; i >= 0; i--)
            {
                SendInfoClient(clientSockets[i], data);
            }            
        }

        private static void SendAllClients(byte[] data, string room)
        {
            List<SessionCurrent> sessions = clientSockets.FindAll(p => p.Room == room).ToList();
            int count = sessions.Count;

            for (int i = count - 1; i >= 0; i--)
            {
                SendInfoClient(sessions[i], data);
            }            
        }

        private static void SendInfoClient(SessionCurrent current, byte[] data)
        {
            try
            {
                if (current.ObjSocket.Connected)
                    current.ObjSocket.Send(data);
                else
                    clientSockets.Remove(current);
            }
            catch(Exception ex)
            {
                clientSockets.Remove(current);
            }
        }

    }
}
