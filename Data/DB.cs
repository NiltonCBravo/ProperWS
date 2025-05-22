using proper_ws.Configuration;
using proper_ws.Model;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
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
            string environment = AppSettings.Environment;
            string connName;
            switch (environment)
            {
                case "Prod":
                    connName = "agmprod";
                    break;
                case "QA":
                    connName = "agmqa";
                    break;
                case "Dev":
                default:
                    connName = "agmdesa";
                    break;
            }
            _cadenaConexion = ConfigurationManager.ConnectionStrings[connName].ConnectionString;
        }

        private SqlConnection GetConn()
        {
            /*SqlConnection sqlcon = new SqlConnection();
            sqlcon.ConnectionString = _cadenaConexion;*/
            var sqlcon = new SqlConnection(_cadenaConexion);
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
        
        public MessageResponse ExecuteSpReturnMessageResponse(string storedProcedureName, SqlParameter[] inputParameters)
        {
            MessageResponse response = new MessageResponse();

            using (SqlConnection conn = GetConn())
            {
                using (SqlCommand cmd = new SqlCommand(storedProcedureName, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = TiempoConexion;

                    // Agregar parámetros de entrada
                    if (inputParameters != null)
                    {
                        cmd.Parameters.AddRange(inputParameters);
                    }

                    // Definir parámetros de salida (deben coincidir con los de tu SP)
                    SqlParameter pMensajeID = new SqlParameter("@PmensajeID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pValueID = new SqlParameter("@PvalueID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter pMensaje = new SqlParameter("@Pmensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                    SqlParameter pError = new SqlParameter("@Perror", SqlDbType.VarChar, 2000) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(pMensajeID);
                    cmd.Parameters.Add(pValueID);
                    cmd.Parameters.Add(pMensaje);
                    cmd.Parameters.Add(pError);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery(); // Usar ExecuteNonQuery ya que los resultados vienen por parámetros de salida

                        // Poblar el objeto MessageResponse desde los parámetros de salida
                        response.mensajeID = Convert.ToInt32(pMensajeID.Value == DBNull.Value ? 0 : pMensajeID.Value);
                        response.valueID = Convert.ToInt32(pValueID.Value == DBNull.Value ? 0 : pValueID.Value);
                        response.mensaje = Convert.ToString(pMensaje.Value == DBNull.Value ? string.Empty : pMensaje.Value);
                        response.error = Convert.ToString(pError.Value == DBNull.Value ? string.Empty : pError.Value);
                    }
                    catch (SqlException sqlEx)
                    {
                        response.mensajeID = -1; // Código de error para excepción SQL
                        response.valueID = 0;
                        response.mensaje = $"Error SQL al ejecutar el SP '{storedProcedureName}'.";
                        response.error = $"Error Number: {sqlEx.Number}. Message: {sqlEx.Message}";
                        // Aquí podrías agregar un log de la excepción si lo necesitas
                    }
                    catch (Exception ex)
                    {
                        response.mensajeID = -2; // Código de error para excepción general
                        response.valueID = 0;
                        response.mensaje = $"Error general al ejecutar el SP '{storedProcedureName}'.";
                        response.error = ex.Message;
                        // Aquí podrías agregar un log de la excepción si lo necesitas
                    }
                }
            }
            return response;
        }

        
        public MessageResponse Ejecutar_sp_Subidas_INS_Async(string storedProcedure, UploadRequest upload)
        {
            MessageResponse response = new MessageResponse();
            
                using (SqlConnection conn = GetConn())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@SubidaId", upload.SubidaId);
                        cmd.Parameters.AddWithValue("@UGUID", upload.UGUID);
                        cmd.Parameters.AddWithValue("@UsuarioSubidaId", upload.UsuarioSubidaId);;
                        cmd.Parameters.AddWithValue("@DispositivoId", upload.DispositivoId);
                        cmd.Parameters.AddWithValue("@FechaHoraRegistro", upload.FechaHoraRegistro);
                        cmd.Parameters.AddWithValue("@SubidaExitosa", upload.SubidaExitosa);

                        // Parámetros de Salida (igual que antes)
                        SqlParameter pMensajeID = new SqlParameter("@PmensajeID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        SqlParameter pMensaje = new SqlParameter("@Pmensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                        SqlParameter pError = new SqlParameter("@Perror", SqlDbType.VarChar, 2000) { Direction = ParameterDirection.Output };
                        SqlParameter pValueID = new SqlParameter("@PvalueID", SqlDbType.Int) { Direction = ParameterDirection.Output };

                        cmd.Parameters.Add(pMensajeID);
                        cmd.Parameters.Add(pMensaje);
                        cmd.Parameters.Add(pError);
                        cmd.Parameters.Add(pValueID);

                        try
                        {
                            conn.OpenAsync(); // Usar OpenAsync
                            cmd.ExecuteNonQueryAsync(); // Usar ExecuteNonQueryAsync

                            // La lectura de parámetros de salida es síncrona
                            response.mensajeID = Convert.ToInt32(pMensajeID.Value == DBNull.Value ? 0 : pMensajeID.Value);
                            response.mensaje = Convert.ToString(pMensaje.Value == DBNull.Value ? string.Empty : pMensaje.Value);
                            response.error = Convert.ToString(pError.Value == DBNull.Value ? string.Empty : pError.Value);
                            response.valueID = Convert.ToInt32(pValueID.Value == DBNull.Value ? 0 : pValueID.Value);
                        }
                        catch (SqlException sqlEx)
                        {
                            response.mensajeID = -2;
                            response.mensaje = "Error en la capa de acceso a datos (SQL Async).";
                            response.error = sqlEx.Message;
                            response.valueID = 0;
                        }
                        catch (Exception ex)
                        {
                            response.mensajeID = -3;
                            response.mensaje = "Error general en la capa de acceso a datos (Async).";
                            response.error = ex.Message;
                            response.valueID = 0;
                        }
                    }
                }
                return response;
        }

        public MessageResponse Ejecutar_sp_Subidas_INS_Async_v1(string storedProcedure ,ContenedorHora contenedorHora)
        {
            MessageResponse response = new MessageResponse();
            
                using (SqlConnection conn = GetConn())
                {
                    using (SqlCommand cmd = new SqlCommand(storedProcedure, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@PfechaIngreso", contenedorHora.FechaIngreso);
                        cmd.Parameters.AddWithValue("@PhoraIngreso", contenedorHora.HoraIngreso);
                        cmd.Parameters.AddWithValue("@PfechaSalida", (object)contenedorHora.FechaSalida ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PhoraSalida", (object)contenedorHora.HoraSalida ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PrefferID", contenedorHora.RefferID);
                        cmd.Parameters.AddWithValue("@PusuarioAutorizadoID", contenedorHora.UsuarioAutorizadoID);;
                        cmd.Parameters.AddWithValue("@PprecintoIngreso", (object)contenedorHora.PrecintoIngreso ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Pcomentarios", (object)contenedorHora.Comentarios ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PestadoID", contenedorHora.EstadoID);
                        cmd.Parameters.AddWithValue("@PCreadoPor", contenedorHora.CreadoPor);
                        cmd.Parameters.AddWithValue("@PfechaCreacion", (object)contenedorHora.FechaCreacion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PActualizadoPor", (object)contenedorHora.ActualizadoPor ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PfechaActualizacion", (object)contenedorHora.FechaActualizacion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PAprobadoPor", (object)contenedorHora.AprobadoPor ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PFechaAprobacion", (object)contenedorHora.FechaAprobacion ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PcomentarioSalida", (object)contenedorHora.ComentarioSalida ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@PtablaOrigen", (object)contenedorHora.TablaOrigen ?? DBNull.Value);

                        // Parámetros de Salida (igual que antes)
                        SqlParameter pMensajeID = new SqlParameter("@PmensajeID", SqlDbType.Int) { Direction = ParameterDirection.Output };
                        SqlParameter pMensaje = new SqlParameter("@Pmensaje", SqlDbType.VarChar, 100) { Direction = ParameterDirection.Output };
                        SqlParameter pError = new SqlParameter("@Perror", SqlDbType.VarChar, 2000) { Direction = ParameterDirection.Output };
                        SqlParameter pValueID = new SqlParameter("@PvalueID", SqlDbType.Int) { Direction = ParameterDirection.Output };

                        cmd.Parameters.Add(pMensajeID);
                        cmd.Parameters.Add(pMensaje);
                        cmd.Parameters.Add(pError);
                        cmd.Parameters.Add(pValueID);

                        try
                        {
                            conn.OpenAsync(); // Usar OpenAsync
                            cmd.ExecuteNonQueryAsync(); // Usar ExecuteNonQueryAsync

                            // La lectura de parámetros de salida es síncrona
                            response.mensajeID = Convert.ToInt32(pMensajeID.Value == DBNull.Value ? 0 : pMensajeID.Value);
                            response.mensaje = Convert.ToString(pMensaje.Value == DBNull.Value ? string.Empty : pMensaje.Value);
                            response.error = Convert.ToString(pError.Value == DBNull.Value ? string.Empty : pError.Value);
                            response.valueID = Convert.ToInt32(pValueID.Value == DBNull.Value ? 0 : pValueID.Value);
                        }
                        catch (SqlException sqlEx)
                        {
                            response.mensajeID = -2;
                            response.mensaje = "Error en la capa de acceso a datos (SQL Async).";
                            response.error = sqlEx.Message;
                            response.valueID = 0;
                        }
                        catch (Exception ex)
                        {
                            response.mensajeID = -3;
                            response.mensaje = "Error general en la capa de acceso a datos (Async).";
                            response.error = ex.Message;
                            response.valueID = 0;
                        }
                    }
                }
                return response;
        }

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
