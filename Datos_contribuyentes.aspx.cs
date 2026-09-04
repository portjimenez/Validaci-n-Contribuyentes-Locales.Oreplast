using identificadores_de_contribuyentes.ConsultaDPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace identificadores_de_contribuyentes
{
    public partial class Datos_contribuyentes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //boton consultar
        protected void Button1_Click1(object sender, EventArgs e)
        {
            var EntityW = ConfigurationManager.AppSettings["Entity"];
            var ReguestorW = ConfigurationManager.AppSettings["REQUESTOR"];
            var CoutryW = ConfigurationManager.AppSettings["Coutry"];
            var TransactionW = ConfigurationManager.AppSettings["Transaction"];
            var UserW = ConfigurationManager.AppSettings["User"];
            var UserNameW = ConfigurationManager.AppSettings["UserName"];
            var Data1W = ConfigurationManager.AppSettings["Data1"];
            var Data3W = ConfigurationManager.AppSettings["Data3"];

            //llamado de los XML que entrega el webservice
            ConsultaNIT.ConsultaNITSoapClient consultaNit = new ConsultaNIT.ConsultaNITSoapClient();
            ConsultaDPI.FactWSFrontSoapClient  consultaDpi = new ConsultaDPI.FactWSFrontSoapClient();

            var respuestaNit = consultaNit.getNIT(CUIorNIT.Text, EntityW, ReguestorW);
            var respuestaDPI = consultaDpi.RequestTransaction(ReguestorW, TransactionW, CoutryW, EntityW, UserW, UserNameW, 
                Data1W, CUIorNIT.Text, Data3W);
            string digitos = CUIorNIT.Text;
            int conteo = digitos.Length;
            
            if(conteo >= 13)
            {
                if (respuestaDPI != null && respuestaDPI.Response != null && respuestaDPI.ResponseData != null)
                {

                    var result = respuestaDPI.Response.Result;
                    var descripcion = respuestaDPI.Response.Description;
                    var cui = respuestaDPI.ResponseData.ResponseData1;

                    ResponseData datosCliente = JsonConvert.DeserializeObject<ResponseData>(cui);

                    if (result)
                    {
                        Label3.Visible = true;
                        Label4.Visible = true;
                        Label5.Visible = true;
                        Label6.Visible = true;

                        Label3.Text = $"Descripcion: {descripcion}";
                        Label4.Text = $"CUI: {datosCliente.CUI}";
                        Label5.Text = $"Nombre: {datosCliente.Nombre}";
                        var resultadoFallecido = datosCliente.Fallecido == true ? "Si" : "No";
                        Label6.Text = $"Fallecido: {resultadoFallecido}";
                    }
                    else
                    {
                        Label3.Visible = true;
                        Label3.Text = "CUI no fue encontrado, verifique los digitos.";
                    }
                }
                else
                {
                    Label3.Visible = true;
                    Label3.Text = "No se obtuvo respuesta del servicio (DPI).";
                }
            }
            else
            {
                if (respuestaNit != null && respuestaNit.Response != null)
                {

                    var result = respuestaNit.Response.Result;
                    var nit = respuestaNit.Response.NIT;
                    var nombre = respuestaNit.Response.nombre;
                    var error = respuestaNit.Response.error;

                    if (result)
                    {
                        Label3.Visible = true;
                        Label4.Visible = true;

                        Label3.Text = $"NIT: {nit}";
                        Label4.Text = $"Nombre: {nombre}";
                    }
                    else
                    {
                        Label3.Visible = true;
                        Label3.Text = $"Error: {error}";
                    }
                }
                else
                {
                    Label3.Visible = true;
                    Label3.Text = "No se obtuvo respuesta del servicio (NIT).";
                }
            }
        }
    }

    public class ResponseData
    {
        public string CUI { get; set; }
        public string Nombre { get; set; }
        public bool Fallecido { get; set; }
    }
}