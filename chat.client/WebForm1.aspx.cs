using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace chat.client
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        delegate void MethodCallBack(string cadena);
        MethodCallBack delegadoClase;
        public delegate void BusinessObjectEventHandler(object sender);
        protected void Page_Load(object sender, EventArgs e)
        {
            //ScriptManager.RegisterClientScriptBlock(this, GetType(), "mykey", "myFunction();", true);
            //ScriptManager.RegisterStartupScript(this.Page, GetType(), "key", "myFunction();", true);

            delegadoClase = new MethodCallBack(DelegateMethod);
            

        }

        private void MofifiedControlLoop(MethodCallBack delegado)
        {
            //this.txtNickName.Text = "hooolawww";
            //while(true)
            {
                //this.txtNickName.Text = "hooolawwwddd";
                MofifiedControl(delegado);
            }
        }

        private void MofifiedControl(MethodCallBack delegado)
        {
            //this.txtNickName.Text = "hooola";
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "key", "myFunction22();", true);

            if (delegado != null)
                delegado("un mensaje");

            //delegado.Invoke("un mensaje");
            //Method nom = new Method("");
            //nom.Invoke("hola");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Response.Redirect("Connect.aspx");
            //new Task(() => MofifiedControlLoop()).Start();
            Thread thread = new Thread(() => MofifiedControlLoop(delegadoClase));

            thread.Start();
            /*
            Thread t = new Thread(new ThreadStart( MofifiedControlLoop));

            t.Start();*/
        }

        protected void DelegateMethod(string mensaje)
        {
            //this.txtNickName.Text = mensaje;
            //Response.Redirect("Connect.aspx");

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            this.Label1.Text = System.DateTime.Now.ToString();
        }
    }
}