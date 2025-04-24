using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace proper_ws
{
    public class da_Web_Service
    {

        public DataSet Asist_ds_CargarMaestros_SQL_SQLLITE(string codigo)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@CODIGO", SqlDbType.VarChar, 50);
            Para[0].Value = codigo;
            return Database_Asist.RunSpReturnDataset("[asist].[ds_CargarMaestros_SQL_SQLLITE]", Para);
        }

        public DataSet Asist_ds_Consultar_GUID_SQLLITE(string codigo)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@CODIGO", SqlDbType.VarChar, 50);
            Para[0].Value = codigo;
            return Database_Asist.RunSpReturnDataset("[asist].[ds_Consultar_GUID_SQLLITE]", Para);
        }

        public DataSet ds_CargarMaestros_SQL_SQLLITE(string codigo)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@CODIGO", SqlDbType.VarChar, 50);
            Para[0].Value = codigo;
            return Database.RunSpReturnDataset("[proper].[ds_CargarMaestros_SQL_SQLLITE]", Para);
        }

        public DataSet ds_Consultar_GUID_SQLLITE(string codigo)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@CODIGO", SqlDbType.VarChar, 50);
            Para[0].Value = codigo;
            return Database.RunSpReturnDataset("[proper].[ds_Consultar_GUID_SQLLITE]", Para);
        }

        public DataSet ds_CargarMaestros_SQL_SQLLITE_Calidad(string codigo)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@CODIGO", SqlDbType.VarChar, 50);
            Para[0].Value = codigo;
            return Database.RunSpReturnDataset("[proper].[ds_CargarMaestros_SQL_SQLLITE_Calidad]", Para);
        }

        public DataSet ds_CargarMaestros_SQL_SQLLITE_EVALAGRI(string codigo)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@CODIGO", SqlDbType.VarChar, 50);
            Para[0].Value = codigo;
            return Database.RunSpReturnDataset("[proper].[ds_CargarMaestros_SQL_SQLLITE_EVALAGRI]", Para);
        }

        public DataSet ds_ConsultarJabaxCodigo_SQLLITE(string JABACODIGO, string USUARIOID)
        {
            SqlParameter[] Para = new SqlParameter[2];
            Para[0] = new SqlParameter("@JABACODIGO", SqlDbType.VarChar, 15);
            Para[0].Value = JABACODIGO;
            Para[1] = new SqlParameter("@USUARIOID", SqlDbType.VarChar, 15);
            Para[1].Value = USUARIOID;
            return Database.RunSpReturnDataset("[proper].[ds_ConsultarJabaxCodigo_SQLLITE]", Para);
        }

        public DataSet ds_ConsultarQrPlanta_SQLLITE(string JABACODIGO, string USUARIOID)
        {
            SqlParameter[] Para = new SqlParameter[2];
            Para[0] = new SqlParameter("@JABACODIGO", SqlDbType.VarChar, 15);
            Para[0].Value = JABACODIGO;
            Para[1] = new SqlParameter("@USUARIOID", SqlDbType.VarChar, 15);
            Para[1].Value = USUARIOID;
            return Database.RunSpReturnDataset("[proper].[ds_ConsultarQrPlanta_SQLLITE]", Para);
        }

        public DataSet cls_sp_FormPlantillaPorLaborDetalle_CARGARMAESTROS(int FormPlantillaPorLaborDetalleId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@FormPlantillaPorLaborDetalleId", SqlDbType.Int);
            Para[0].Value = FormPlantillaPorLaborDetalleId;
            return Database.RunSpReturnDataset("[proper].[sp_FormPlantillaPorLaborDetalle_CARGAR_MAESTROS]", Para);
        }

        public DataSet ds_PRP_TBL_PROGRAMACION_SEL_MAESTROS2(string GROWERID, string FARMID, string LOTID, string USUARIOID)
        {
            SqlParameter[] Para = new SqlParameter[4];
            Para[0] = new SqlParameter("@GROWERID", SqlDbType.VarChar, 15);
            Para[0].Value = GROWERID;
            Para[1] = new SqlParameter("@FARMID", SqlDbType.VarChar, 15);
            Para[1].Value = FARMID;
            Para[2] = new SqlParameter("@LOTID", SqlDbType.VarChar, 50);
            Para[2].Value = LOTID;
            Para[3] = new SqlParameter("@USUARIOID", SqlDbType.VarChar, 15);
            Para[3].Value = USUARIOID;
            return Database.RunSpReturnDataset("[proper].[PRP_TBL_PROGRAMACION_SEL_MAESTROS2]", Para);
        }

        public DataTable cls_sp_Fotos_INS( string UniqueString, string Nombre, string Foto)
        {
            SqlParameter[] Para = new SqlParameter[3];
            Para[0] = new SqlParameter("@UniqueString", SqlDbType.VarChar, 100);
            Para[0].Value = UniqueString;
            Para[1] = new SqlParameter("@Nombre", SqlDbType.VarChar, 100);
            Para[1].Value = Nombre;
            Para[2] = new SqlParameter("@Foto", SqlDbType.VarChar, -1);
            Para[2].Value = Foto;
            return Database.RunSpReturnDataTable("[proper].[sp_Fotos_INS]", Para);
        }

    }
}