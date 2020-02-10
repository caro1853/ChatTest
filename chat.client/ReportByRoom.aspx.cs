using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace chat.client
{
    public partial class ReportByRoom : System.Web.UI.Page
    {
        private readonly IRoomService _roomService;
        private readonly IMessageService _messageService;

        public ReportByRoom(IRoomService roomService, IMessageService messageService)
        {
            _roomService = roomService;
            _messageService = messageService;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    var room = _roomService.GetAll();

                    this.grvRooms.DataSource = room;
                    this.grvRooms.DataBind();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
        }

        protected void lnkSelectRoom_Click(object sender, EventArgs e)
        {
            try
            {
                int roomId = Convert.ToInt32((sender as LinkButton).CommandArgument);
                string roomName = (sender as LinkButton).CommandName;

                var messages = _messageService.GetByRoom(roomId).ToList();

                this.grvMessages.DataSource = messages;
                this.grvMessages.DataBind();

                this.lblUserMessage.Text = $"Mensajes en la sala <strong>{roomName}</strong>";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
        }
    }
}