using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace proper_ws
{
    /// <summary>
    /// Summary description for ws_dev
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ws_dev : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            return "Hello World";
        }

        [WebMethod]
        public string WM(int DISPOSITIVO, string USUARIO_SUBIDA, DataSet ds, string UGUID)
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
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
                    string res = os.ds_INSXML_SQL_SQLLITE(os.prop_SubidaId).Rows[0]["SUBIDA_ID"].ToString();
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
        public DataSet DameMS(string codigo)
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_CargarMaestros_SQL_SQLLITE(codigo);
            return ds;
        }

        [WebMethod]
        public DataSet ConsultarJaba(string JABACODIGO, string USUARIOID)
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_ConsultarJabaxCodigo_SQLLITE(JABACODIGO, USUARIOID);
            return ds;
        }

        [WebMethod]
        public DataSet CQRP(string JABACODIGO, string USUARIOID)
        {
            // consultar qr de planta

            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;


            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_ConsultarQrPlanta_SQLLITE(JABACODIGO, USUARIOID);
            return ds;
        }

        [WebMethod]
        public DataSet ds_DAGSM2(string GROWERID, string FARMID, string LOTID, string USUARIOID)
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_PRP_TBL_PROGRAMACION_SEL_MAESTROS2(GROWERID, FARMID, LOTID, USUARIOID);
            return ds;
        }

        [WebMethod]
        public DataSet ConsultarFormDinamica(int FormPlantillaPorLaborDetalleId)
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.cls_sp_FormPlantillaPorLaborDetalle_CARGARMAESTROS(FormPlantillaPorLaborDetalleId);
            return ds;
        }

        [WebMethod]
        public string Sf(string b64, string uq, string r, string t, string d)
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            string resp = "";
            //r = uq + ".jpg";
            //da_Web_Service da = new da_Web_Service();
            //try
            //{
            //    using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(b64)))
            //    {
            //        using (Bitmap bm2 = new Bitmap(ms))
            //        {
            //            bm2.Save(r);
            //            // guardamos en tabla proper.fotos y asignamos el rowidcorrespondiente
            //            da.cls_sp_Fotos_INS(0,uq,t,0,d,r,1,"","");
            //            resp = "1";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    string err = ex.Message;
            //    if (err.Length > 499)
            //    {
            //        err = err.Substring(0, 490);
            //    }
            //    da.cls_sp_Fotos_INS(0, uq, t, 0, d, r, 0, err ,b64);
            //    resp = "0";
            //    throw;
            //}

            return resp;
        }

        [WebMethod]
        public string Sf2(string b64, string uq, string r, string t, string d)
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;
            GlobalVariables.Web_ModoDeveloperSispackingTest = false;

            string resp = "";

            try
            {
                SubirStringaAzure(b64, uq + ".jpeg");
            }
            catch (Exception)
            {

                resp = "0"; 
            }

            return resp;
            
        }
        async void SubirStringaAzure(string sfoto, string nombre)
        {
            //string sfoto = "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";
            AzureStorage cls = new AzureStorage();
            int n = await cls.UploadBlobAsyncToBase64(sfoto, nombre);
            if (n == 200 | n == 201)
            {
                // exito
            }
            else
            {

            }
        }


        [WebMethod]
        public DataSet CONS_GUID(string codigo)
        {
            GlobalVariables.Web_ModoDeveloper = true;
            GlobalVariables.Web_ModoDeveloperQA = false;

            DataSet ds = new DataSet();
            da_Web_Service da = new da_Web_Service();
            ds = da.ds_Consultar_GUID_SQLLITE(codigo);
            return ds;
        }

    }
}
