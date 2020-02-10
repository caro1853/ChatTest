<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageConnectedUsers.aspx.cs" Inherits="chat.client.ManageConnectedUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-group ">
                <asp:Label ID="lblMessagesError" runat="server" Text="" CssClass="form-control border border-danger"></asp:Label>
                <asp:Button ID="btnCleanMessageError" Visible="false" runat="server" Text="Ok" CssClass="btn btn-danger" OnClick="btnCleanMessageError_Click" />
    </div>

    <div class="form-group">
        <br />
        <br />
        <br />
        <br />
        <asp:Button ID="btnJoin" runat="server" Text="Conectarse" OnClick="btnJoin_Click" CssClass="btn btn-success" />
        <asp:Button ID="btnGetUser" runat="server" Text="Obtener Usuarios" CssClass="btn btn-success" OnClick="btnGetUser_Click" />
        <asp:Button ID="btnConnectedUsers" runat="server" Text="Ver Usuarios" CssClass="btn btn-success" OnClick="btnConnectedUsers_Click" />
    </div>

    <div class="form-group">
        <h3> Usuarios Conectados</h3>        
        <asp:GridView ID="grvUsers" runat="server" AutoGenerateColumns="false">
            <Columns>                
                <asp:BoundField DataField="User.Name" HeaderText="Usuario"/>
                <asp:BoundField DataField="Room.Name" HeaderText="Sala"/>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSelectUser" Text="Desconectar" runat="server" OnClick="lnkSelectUser_Click" CommandName='<%# Eval("User.Name") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
