using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace proper_ws
{
    public class cls_sp_Subidas_Asist
    {
        public DataTable cls_sp_Subidas_SEL(int SubidaId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SubidaId", SqlDbType.Int);
            Para[0].Value = SubidaId;
            return Database_Asist.RunSpReturnDataTable("[asist].[sp_Subidas_SEL]", Para);
        }

        public DataTable cls_sp_Subidas_SEL_VISTA(int SubidaId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SubidaId", SqlDbType.Int);
            Para[0].Value = SubidaId;
            return Database_Asist.RunSpReturnDataTable("[asist].[sp_Subidas_SEL_VISTA]", Para);
        }

        public DataSet cls_sp_Subidas_CARGARMAESTROS(int SubidaId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SubidaId", SqlDbType.Int);
            Para[0].Value = SubidaId;
            return Database_Asist.RunSpReturnDataset("[asist].[sp_Subidas_CARGAR_MAESTROS]", Para);
        }

        public DataTable cls_sp_Subidas_INS(int SubidaId, string UsuarioSubidaId, int DispositivoId, DateTime FechaHoraRegistro, int SubidaExitosa, string UGUID)
        {
            SqlParameter[] Para = new SqlParameter[6];
            Para[0] = new SqlParameter("@SubidaId", SqlDbType.Int);
            Para[0].Value = SubidaId;
            Para[1] = new SqlParameter("@UsuarioSubidaId", SqlDbType.VarChar, 11);
            Para[1].Value = UsuarioSubidaId;
            Para[2] = new SqlParameter("@DispositivoId", SqlDbType.SmallInt);
            Para[2].Value = DispositivoId;
            Para[3] = new SqlParameter("@FechaHoraRegistro", SqlDbType.SmallDateTime);
            Para[3].Value = FechaHoraRegistro;
            Para[4] = new SqlParameter("@SubidaExitosa", SqlDbType.Bit);
            Para[4].Value = SubidaExitosa;
            Para[5] = new SqlParameter("@UGUID", SqlDbType.VarChar, 50);
            Para[5].Value = UGUID;
            return Database_Asist.RunSpReturnDataTable("[asist].[sp_Subidas_INS]", Para);
        }

        public DataTable cls_sp_Subidas_UPD(int SubidaId, string UsuarioSubidaId, int DispositivoId, DateTime FechaHoraRegistro, int SubidaExitosa, string UGUID)
        {
            SqlParameter[] Para = new SqlParameter[6];
            Para[0] = new SqlParameter("@SubidaId", SqlDbType.Int);
            Para[0].Value = SubidaId;
            Para[1] = new SqlParameter("@UsuarioSubidaId", SqlDbType.VarChar, 11);
            Para[1].Value = UsuarioSubidaId;
            Para[2] = new SqlParameter("@DispositivoId", SqlDbType.SmallInt);
            Para[2].Value = DispositivoId;
            Para[3] = new SqlParameter("@FechaHoraRegistro", SqlDbType.SmallDateTime);
            Para[3].Value = FechaHoraRegistro;
            Para[4] = new SqlParameter("@SubidaExitosa", SqlDbType.Bit);
            Para[4].Value = SubidaExitosa;
            Para[5] = new SqlParameter("@UGUID", SqlDbType.VarChar, 50);
            Para[5].Value = UGUID;
            return Database_Asist.RunSpReturnDataTable("[asist].[sp_Subidas_UPD]", Para);
        }

        public int cls_sp_Subidas_DEL(int SubidaId)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SubidaId", SqlDbType.Int);
            Para[0].Value = SubidaId;
            return Database_Asist.RunSp("[asist].[sp_Subidas_DEL]", Para);
        }

        public int cls_sp_Subidas_INSXML(string S_XML)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@S_XML", SqlDbType.VarChar, -1);
            Para[0].Value = S_XML;
            return Database_Asist.RunSp("[asist].[sp_Subidas_INSXML]", Para);
        }


        public DataTable ds_INSXML_SQL_SQLLITE(int SUBIDA_ID)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SUBIDA_ID", SqlDbType.Int);
            Para[0].Value = SUBIDA_ID;
            return Database_Asist.RunSpReturnDataTable("[asist].[ds_INSXML_SQL_SQLLITE]", Para);
        }

        public DataTable ds_INSXML_SQL_SQLLITE_Calidad(int SUBIDA_ID)
        {
            SqlParameter[] Para = new SqlParameter[1];
            Para[0] = new SqlParameter("@SUBIDA_ID", SqlDbType.Int);
            Para[0].Value = SUBIDA_ID;
            return Database_Asist.RunSpReturnDataTable("[asist].[ds_INSXML_SQL_SQLLITE_Calidad]", Para);
        }

    }
}