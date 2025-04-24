using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace proper_ws
{
    public class cls_sp_SubidasLog_Asist
    {
        public DataTable cls_sp_SubidasLog_SEL(int SubidaLogId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SubidaLogId", SqlDbType.Int);
            Para[0].Value = SubidaLogId;
            return Database_Asist.RunSpReturnDataTable("[asist].sp_SubidasLog_SEL", Para);
        }

        public DataTable cls_sp_SubidasLog_SEL_VISTA(int SubidaLogId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SubidaLogId", SqlDbType.Int);
            Para[0].Value = SubidaLogId;
            return Database_Asist.RunSpReturnDataTable("[asist].sp_SubidasLog_SEL_VISTA", Para);
        }

        public DataSet cls_sp_SubidasLog_CARGARMAESTROS(int SubidaLogId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SubidaLogId", SqlDbType.Int);
            Para[0].Value = SubidaLogId;
            return Database_Asist.RunSpReturnDataset("[asist].sp_SubidasLog_CARGAR_MAESTROS", Para);
        }

        public DataTable cls_sp_SubidasLog_INS(int SubidaLogId, int SubidaId, string Tabla, int SubidaExitosa, string Contenido, string Error)
        {
            SqlParameter[] Para = new SqlParameter[6];
            Para[0] = new SqlParameter("@SubidaLogId", SqlDbType.Int);
            Para[0].Value = SubidaLogId;
            Para[1] = new SqlParameter("@SubidaId", SqlDbType.Int);
            Para[1].Value = SubidaId;
            Para[2] = new SqlParameter("@Tabla", SqlDbType.VarChar, 100);
            Para[2].Value = Tabla;
            Para[3] = new SqlParameter("@SubidaExitosa", SqlDbType.Bit);
            Para[3].Value = SubidaExitosa;
            Para[4] = new SqlParameter("@Contenido", SqlDbType.VarChar, -1);
            Para[4].Value = Contenido;
            Para[5] = new SqlParameter("@Error", SqlDbType.VarChar, -1);
            Para[5].Value = Error;
            return Database_Asist.RunSpReturnDataTable("[asist].sp_SubidasLog_INS", Para);
        }

        public DataTable cls_sp_SubidasLog_UPD(int SubidaLogId, int SubidaId, string Tabla, int SubidaExitosa, string Contenido, string Error)
        {
            SqlParameter[] Para = new SqlParameter[6];
            Para[0] = new SqlParameter("@SubidaLogId", SqlDbType.Int);
            Para[0].Value = SubidaLogId;
            Para[1] = new SqlParameter("@SubidaId", SqlDbType.Int);
            Para[1].Value = SubidaId;
            Para[2] = new SqlParameter("@Tabla", SqlDbType.VarChar, 100);
            Para[2].Value = Tabla;
            Para[3] = new SqlParameter("@SubidaExitosa", SqlDbType.Bit);
            Para[3].Value = SubidaExitosa;
            Para[4] = new SqlParameter("@Contenido", SqlDbType.VarChar, -1);
            Para[4].Value = Contenido;
            Para[5] = new SqlParameter("@Error", SqlDbType.VarChar, -1);
            Para[5].Value = Error;
            return Database_Asist.RunSpReturnDataTable("[asist].sp_SubidasLog_UPD", Para);
        }

        public int cls_sp_SubidasLog_DEL(int SubidaLogId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SubidaLogId", SqlDbType.Int);
            Para[0].Value = SubidaLogId;
            return Database_Asist.RunSp("[asist].sp_SubidasLog_DEL", Para);
        }

        public int cls_sp_SubidasLog_INSXML(string S_XML)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@S_XML", SqlDbType.VarChar, -1);
            Para[0].Value = S_XML;
            return Database_Asist.RunSp("[asist].sp_SubidasLog_INSXML", Para);
        }

    }
}