using chat.core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace chat.client
{
    public partial class ReportByUser : System.Web.UI.Page
    {
        private readonly IUserService _userService;
        private readonly IRoomService _roomService;
        private readonly IMessageService _messageService;
        private static int userIdSelected;
        private static string userNameSelected;

        public ReportByUser(IUserService userService, IRoomService roomService, IMessageService messageService)
        {
            _userService = userService;
            _roomService = roomService;
            _messageService = messageService;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    var users = _userService.GetAll();

                    this.grvUsers.DataSource = users;
                    this.grvUsers.DataBind();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
        }

        protected void lnkSelectUser_Click(object sender, EventArgs e)
        {
            try
            {
                userIdSelected = Convert.ToInt32((sender as LinkButton).CommandArgument);
                userNameSelected = (sender as LinkButton).CommandName;

                var rooms = _roomService.GetByUser(userIdSelected);

                this.grvRooms.DataSource = rooms;
                this.grvRooms.DataBind();

                this.lblUserCurrent.Text = $"Sala del usuario <strong>{userNameSelected}</strong>";
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

                var messages = _messageService.GetByUserRoom(userIdSelected, roomId);

                this.grvMessages.DataSource = messages;
                this.grvMessages.DataBind();

                this.lblUserMessage.Text = $"Mensajes del usuario <strong>{userNameSelected}</strong> en la sala <strong>{roomName}</strong>";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error. {ex.Message}");
            }
        }
    }
}