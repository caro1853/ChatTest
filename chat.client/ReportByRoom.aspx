<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportByRoom.aspx.cs" Inherits="chat.client.ReportByRoom" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-group">
        <h3> Salas</h3>
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
                <asp:BoundField DataField="Session.User.Name" HeaderText="Usuario"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
