using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;

namespace proper_ws
{
    public class Database
    {
        private static DataSet Data;
        private static DataTable Datat;
        private static readonly int TiempoConexion = 3600;
        public static string NombreServidor;



        public static SqlConnection GetConn()
        {

            SqlConnection sqlcon = new SqlConnection();
            string connection_ = "";
            //ucser_agroamigo_qa -> PrU3b420254



            // agroamigo produccion
            if
                (
                (GlobalVariables.Web_ModoDeveloper == false)
                &&
                (GlobalVariables.Web_ModoDeveloperQA == false)
                &&
                (GlobalVariables.Web_ModoDeveloperSispackingTest == false)
                )
            {
                // 10.1.10.5
                // BD_PACKING_AGROMIGIVA_PRD
                //connection_ = @"Data Source= migivaazure.database.windows.net;Initial Catalog=BD_AGROAMIGO_PROD;Persist Security Info=True;User ID=ucser_agroamigo;Password=AgRF43ExfFDe;";
                connection_ = @"";
            }

            // sispacking unificacion test
            if
                (
                (GlobalVariables.Web_ModoDeveloper == false)
                &&
                (GlobalVariables.Web_ModoDeveloperQA == false)
                &&
                (GlobalVariables.Web_ModoDeveloperSispackingTest == true)
                )
            {
                connection_ = @"";
            }

            // agroamigo  desa
            if
                (
                (GlobalVariables.Web_ModoDeveloper == true)
                &&
                (GlobalVariables.Web_ModoDeveloperQA == false)
                &&
                (GlobalVariables.Web_ModoDeveloperSispackingTest == false)
                )
            {

                //ucser_agroamigo_desa
                //connection_ = @"Data Source= migivaazure.database.windows.net;Initial Catalog=BD_AGROAMIGO_DESA;Persist Security Info=True;User ID=ucown_agroamigo_desa;Password=D3s4rr0ll024;";
                connection_ = @"";
            }

            // agroamigo qa
            if
                (
                (GlobalVariables.Web_ModoDeveloper == false)
                &&
                (GlobalVariables.Web_ModoDeveloperQA == true)
                &&
                (GlobalVariables.Web_ModoDeveloperSispackingTest == false)
                )
            {
                //connection_ = @"Data Source= migivaazure.database.windows.net;Initial Catalog=BD_AGROAMIGO_QA;Persist Security Info=True;User ID=ucser_agroamigo_qa;Password=QAs4rr0ll024;";
                connection_ = @"";
            }


            sqlcon.ConnectionString = connection_;
            
            return sqlcon;
        }

        public static DataSet RunSpReturnDataset(string StrNomSp, SqlParameter[] myParamArray)
        {
            // Function q ejecuta un Store Procedure y retorna un DataSet
            // Dim conn As SqlConnection = New SqlConnection(GetStringConnection)
            SqlConnection conn = GetConn();
            conn.Open();

            SqlCommand cmd = new SqlCommand(StrNomSp, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.UpdatedRowSource = UpdateRowSource.Both;

            // Dim ST As String = ""

            if (myParamArray != null)
            {
                for (int j = 0; j <= myParamArray.Length - 1; j++)
                    cmd.Parameters.Add(myParamArray[j]);
            }


            SqlDataAdapter da = new SqlDataAdapter();
            Data = new DataSet();
            da.SelectCommand = cmd;
            da.SelectCommand.CommandTimeout = TiempoConexion;
            da.Fill(Data, "Temporal");
            DataSet s = Data;
            conn.Close();
            conn.Dispose();
            //RunSpReturnDataset = s;
            return s;
        }

        public static DataTable RunSpReturnDataTable(string StrNomSp, SqlParameter[] myParamArray)
        {
            //RunSpReturnDataTable = null/* TODO Change to default(_) if this is not a reference type */;

            // Function q ejecuta un Store Procedure y retorna un DataSet
            // RunSpReturnDataTable = Nothing
            // Dim conn As SqlConnection = New SqlConnection(GetStringConnection)

                SqlConnection conn = GetConn();
                conn.Open();
                SqlCommand cmd = new SqlCommand(StrNomSp, conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.UpdatedRowSource = UpdateRowSource.Both;
                if (myParamArray != null)
                {
                    int j;
                    for (j = 0; j <= myParamArray.Length - 1; j++)
                        cmd.Parameters.Add(myParamArray[j]);
                }
                SqlDataAdapter da = new SqlDataAdapter();
                Datat = new DataTable();
                da.SelectCommand = cmd;
                da.SelectCommand.CommandTimeout = TiempoConexion;
                da.Fill(Datat);
                DataTable s = Datat;
                conn.Close();
                conn.Dispose();
                int n = s.Rows.Count;

            return s;
        }

        public static string RunSpReturnString(string StrNomSp, SqlParameter[] myParamArray)
        {
            // Function q ejecuta un Store Procedure y retorna una cadena o valor
            // Dim conn As SqlConnection = New SqlConnection(GetStringConnection)
            SqlConnection conn = GetConn();
            conn.Open();
            SqlCommand cmd = new SqlCommand(StrNomSp, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (myParamArray != null)
            {
                int j;
                for (j = 0; j <= myParamArray.Length - 1; j++)
                    cmd.Parameters.Add(myParamArray[j]);
            }
            cmd.CommandTimeout = TiempoConexion;
            string s = Convert.ToString(cmd.ExecuteScalar());
            conn.Close();
            conn.Dispose();
            return s;
        }

        public static int RunSp(string StrNomSp, SqlParameter[] myParamArray)
        {
            // Function q ejecuta un Store Procedure y retorna una cadena o valor
            // Dim conn As SqlConnection = New SqlConnection(GetStringConnection)
            SqlConnection conn = GetConn();
            conn.Open();
            SqlCommand cmd = new SqlCommand(StrNomSp, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (myParamArray != null)
            {
                int j;
                for (j = 0; j <= myParamArray.Length - 1; j++)
                    cmd.Parameters.Add(myParamArray[j]);
            }
            cmd.CommandTimeout = TiempoConexion;
            int valor = cmd.ExecuteNonQuery();
            conn.Close();
            conn.Dispose();
            return valor;
        }
    }

}