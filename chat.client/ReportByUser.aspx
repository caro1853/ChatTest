<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportByUser.aspx.cs" Inherits="chat.client.ReportByUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    

    <div class="form-group">
        <h3> Usuarios</h3>
        <asp:GridView ID="grvUsers" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="UserId" HeaderText="Id"/>
                <asp:BoundField DataField="Name" HeaderText="Nombre"/>
                <asp:BoundField DataField="DateCreate" HeaderText="Fecha de creación"/>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSelectUser" Text="Ver Salas" runat="server" CommandName='<%# Eval("Name") %>' CommandArgument='<%# Eval("UserId") %>' OnClick="lnkSelectUser_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>

    <div class="form-group">
        <h3>   <asp:Label ID="lblUserCurrent" runat="server" Text=""></asp:Label>  </h3>
        <asp:GridView ID="grvRooms" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="RoomId" HeaderText="Id"/>
                <asp:BoundField DataField="Name" HeaderText="Nombre"/>
                <asp:BoundField DataField="DateCreate" HeaderText="Fecha de creación"/>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSelectRoom" Text="Ver Mensajes" runat="server" CommandName='<%# Eval("Name") %>' CommandArgument='<%# Eval("RoomId") %>' OnClick="lnkSelectRoom_Click" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>


    <div class="form-group">
        <h3> <asp:Label ID="lblUserMessage" runat="server" Text=""></asp:Label> </h3>
        <asp:GridView ID="grvMessages" runat="server" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="DateSend" HeaderText="Fecha de envío"/>                     
                <asp:BoundField DataField="MessageText" HeaderText="Message"/>
                           
            </Columns>
        </asp:GridView>
    </div>

</asp:Content>
