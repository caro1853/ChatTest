<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="Connect.aspx.cs" Inherits="chat.client.Connect" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">    
        
            <div class="form-group ">
                <asp:Label ID="lblMessagesError" runat="server" Text="" CssClass="form-control border border-danger"></asp:Label>
                <asp:Button ID="btnCleanMessageError" Visible="false" runat="server" Text="Ok" CssClass="btn btn-danger" OnClick="btnCleanMessageError_Click" />
            </div>


        <div class="form-group">
            <asp:Button ID="btnJoin" runat="server" Text="Conectarse" OnClick="btnJoin_Click" CssClass="btn btn-success" />
            <asp:Label ID="lblInfo" runat="server" CssClass="form-control" Text="Usted No está conectado"></asp:Label>
        </div>

        <div class="form-group">
            Selececcione la sala
            <asp:ListBox ID="lstRooms" runat="server" CssClass="form-control"></asp:ListBox>
            O Cree una nueva Sala.
            <asp:TextBox ID="txtSala" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>
        </div>

         <div class="form-group">
            Digite el nombre
            <asp:TextBox ID="txtNickName" runat="server" CssClass="form-control form-control-sm"></asp:TextBox>

        </div>
         <div class="form-group">
             <asp:Button ID="btnJoinRoom" runat="server" Text="Unirse" OnClick="btnJoinRoom_Click" CssClass="btn btn-success" />
         </div>

        <div class="form-group">
            Mensaje<br />
            <asp:TextBox ID="txtMensaje" runat="server" CssClass="form-control form-control-sm"></asp:TextBox><br />
            <asp:Button ID="btnSend" runat="server" Text="Enviar" OnClick="btnSend_Click" CssClass="btn btn-success" /><br />
        </div>

        <div class="form-group">
            Mensajes de otros usuarios

            <asp:TextBox ID="txtAllMessages" runat="server" TextMode="MultiLine" CssClass="form-control form-control-lg" Height="90px"></asp:TextBox>
        </div>
                             
     
     <asp:Timer ID="Timmer1" runat="server" Interval="10000" OnTick="Unnamed2_Tick" Enabled="True"></asp:Timer>
     
        
        <asp:Button ID="btnTest" Visible="false" runat="server" Text="Test" OnClick="btnTest_Click" />

    

</asp:Content>

