<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="chat.client.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>

    <script src="Scripts/jquery-3.4.1.js" type="text/javascript"></script>
    
      <script type="text/javascript">
         
          $(document).ready(function () {

              function myFunction() {
                  $("input[id$='txtNickName']").val("Text Added using jQuery");
              }
          });

          function myFunction22() {
              var obj = document.getElementById("txtNickName");

              if (obj != null) {
                  obj.value = "pruebasss";
              }
              else {
                  alert("no existe");
              }
          }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>           
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>

            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Button" />

            <asp:UpdatePanel ID="UpdatePanel1" runat="server">

            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick">
            </asp:Timer>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </asp:UpdatePanel>

        </div>
    </form>
</body>
</html>
