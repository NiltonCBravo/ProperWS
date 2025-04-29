using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace proper_ws.Data
{
    public class DB
    {
        private readonly string _cadenaConexion;
        private static DataSet Data;
        private static DataTable Datat;
        private static readonly int TiempoConexion = 3600;

        public DB()
        {
            _cadenaConexion = ConfigurationManager.ConnectionStrings["sispackindesa"].ConnectionString;
        }

        public SqlConnection GetConn()
        {
            SqlConnection sqlcon = new SqlConnection();
            sqlcon.ConnectionString = _cadenaConexion;
            return sqlcon;
        }

        public DataSet RunSpReturnDataset(string storedProcedure, SqlParameter[] parametros)
        {
            DataSet resultado = new DataSet();

            try
            {
                using (SqlConnection conn = GetConn())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = TiempoConexion;
                        cmd.UpdatedRowSource = UpdateRowSource.Both;

                        if (parametros != null)
                        {
                            cmd.Parameters.AddRange(parametros);
                            /*for (int j = 0; j <= parametros.Length - 1; j++)
                            {
                                cmd.Parameters.Add(parametros[j]);
                            }  */
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(resultado, "Temporal");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Puedes registrar la excepción con tu logger, o lanzarla para que otro sistema la capture.
                throw new Exception($"Error al ejecutar el SP '{storedProcedure}': {ex.Message}", ex);
            }

            return resultado;
        }
        
        /*public DataSet RunSpReturnDataset(string StrNomSp, SqlParameter[] myParamArray)
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
        }*/
        
        public DataSet RunSpReturnMultipleDataset (string storedProcedure, SqlParameter[] parametros)
        {
            DataSet resultado = new DataSet();

            try
            {
                using (SqlConnection conn = GetConn())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = TiempoConexion;

                        if (parametros != null)
                            cmd.Parameters.AddRange(parametros);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            // Eliminamos el nombre de tabla fijo "Temporal"
                            adapter.Fill(resultado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar el SP '{storedProcedure}': {ex.Message}", ex);
            }

            return resultado;
        }
        
        public DataTable RunSpReturnDataTable(string storedProcedure, SqlParameter[] parametros)
        {

            DataTable result = new DataTable();

            try
            {
                using (SqlConnection conn = GetConn())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = TiempoConexion;

                        if (parametros != null)
                        {
                            int j;
                            for (j = 0; j <= parametros.Length - 1; j++)
                                cmd.Parameters.Add(parametros[j]);
                        }
                        
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable ds = new DataTable();
                            adapter.Fill(ds);
                            result = ds;
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar el SP '{storedProcedure}': {ex.Message}", ex);
            }
            
        }
    }
}
