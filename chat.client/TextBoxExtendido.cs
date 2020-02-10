using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static chat.client.WebForm1;

namespace chat.client
{
    public class TextBoxExtendido: System.Web.UI.WebControls.TextBox
    {

        public event BusinessObjectEventHandler onCambio;

        public TextBoxExtendido(): base()
        {

        }

        public void CambiarTexto(string texto)
        {
            this.Text = texto;
        }

        
    }
}