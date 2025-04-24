using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;

namespace proper_ws
{
    /// <summary>
    /// Summary description for wscal_qa
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class wscal_qa : System.Web.Services.WebService
    {


        [WebMethod]
        public string WM_Calidad(int DISPOSITIVO, string USUARIO_SUBIDA, DataSet ds, string UGUID)
        {
            GlobalVariables.Web_ModoDeveloperQA = true;
            GlobalVariables.Web_ModoDeveloper = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            string subidaok = "0";
            string tabla = "";
            string datos = "000";
            int SUBIDA_ID = 0;
            Obj_Subidas os = new Obj_Subidas();
            os.InicializarObjeto();
            try
            {

                os.prop_DispositivoId = DISPOSITIVO;
                os.prop_UsuarioSubidaId = USUARIO_SUBIDA;
                os.prop_FechaHoraRegistro = Convert.ToDateTime((DateTime.Now.ToString("s")).Replace("T", " "));
                os.prop_SubidaExitosa = 0;
                os.prop_UGUID = UGUID;
                os.Prop_clsResultadoBd = os.Insert_();
                SUBIDA_ID = Convert.ToInt32(os.Prop_clsResultadoBd.Id);
                os.prop_SubidaId = SUBIDA_ID;
                int cantsubidaexitosa = 0;
                int SUBIDA_LOG_ID = 0;

                DataTable dt = ds.Tables[0].Copy();
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow dr = dt.Rows[j];
                    tabla = dr["tabla"].ToString().ToUpper();
                    datos = dr["datos"].ToString();

                    Obj_SubidasLog oslog = new Obj_SubidasLog();
                    oslog.InicializarObjeto();
                    oslog.prop_SubidaId = os.prop_SubidaId;
                    oslog.prop_Tabla = tabla;
                    oslog.prop_SubidaExitosa = 0;
                    oslog.prop_Error = "0";
                    oslog.prop_Contenido = datos;
                    oslog.Prop_clsResultadoBd = oslog.Insert_();
                    SUBIDA_LOG_ID = Convert.ToInt32(oslog.Prop_clsResultadoBd.Id);
                    if ((oslog.Prop_clsResultadoBd.Fallo == false) && (SUBIDA_LOG_ID > 0))
                    {
                        cantsubidaexitosa++;
                    }
                    else
                    {
                        j = dt.Rows.Count + 10;
                        // proceso para eliminar??
                        subidaok = "-1";
                    }
                }
                if (cantsubidaexitosa == dt.Rows.Count)
                {
                    string res = os.ds_INSXML_SQL_SQLLITE_Calidad(os.prop_SubidaId).Rows[0]["SUBIDA_ID"].ToString();
                    if (Convert.ToInt32(res) == os.prop_SubidaId)
                    {
                        subidaok = "1";
                    }
                }

            }
            catch (Exception ex)
            {
                subidaok = "-3." + os.prop_FechaHoraRegistro + "." + ex.ToString();
            }
            return subidaok;
        }

        [WebMethod]
        public DataSet DameMS_Calidad(string codigo)
        {
            GlobalVariables.Web_ModoDeveloperQA = true;
            GlobalVariables.Web_ModoDeveloper = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_CargarMaestros_SQL_SQLLITE_Calidad(codigo);
            return ds;
        }

        [WebMethod]
        public DataSet DameMS_EVALGRI(string codigo)
        {
            GlobalVariables.Web_ModoDeveloperQA = true;
            GlobalVariables.Web_ModoDeveloper = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_CargarMaestros_SQL_SQLLITE_EVALAGRI(codigo);
            return ds;
        }

        [WebMethod]
        public DataSet ConsultarJaba(string JABACODIGO, string USUARIOID)
        {
            GlobalVariables.Web_ModoDeveloperQA = true;
            GlobalVariables.Web_ModoDeveloper = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_ConsultarJabaxCodigo_SQLLITE(JABACODIGO, USUARIOID);
            return ds;
        }


        [WebMethod]
        public DataSet CONS_GUID(string codigo)
        {
            GlobalVariables.Web_ModoDeveloperQA = true;
            GlobalVariables.Web_ModoDeveloper = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_Consultar_GUID_SQLLITE(codigo);
            return ds;
        }

        [WebMethod]
        public string Sf2_Calidad(string nom, string f, string uq)
        {
            GlobalVariables.Web_ModoDeveloperQA = true;
            GlobalVariables.Web_ModoDeveloper = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            ////string nombre = "20240426_0932.jpeg";
            //int n = await SubirStringaAzure(f, nom);

            string resp = "";
            try
            {
                da_Web_Service da = new da_Web_Service();
                da.cls_sp_Fotos_INS(uq, nom, f);
                resp = "1";
            }
            catch (Exception)
            {
                resp = "0";
            }
            return resp;
        }

        public async Task<int> SubirStringaAzure(string sfoto, string nombre)
        {
            GlobalVariables.Web_ModoDeveloperQA = true;
            GlobalVariables.Web_ModoDeveloper = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            AzureStorage cls = new AzureStorage();
            int n = await cls.UploadBlobAsyncToBase64(sfoto, nombre);
            if (n == 200 || n == 201)
            {
                // Éxito
                return 1;
            }
            else
            {
                // Manejar el caso de error
                return 0; // O cualquier otro valor que desees
            }
        }


    }
}
