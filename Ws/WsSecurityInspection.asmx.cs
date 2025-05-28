using proper_ws.Model;
using proper_ws.Services;
using proper_ws.Utils;
using System;
using System.Data;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services;
namespace proper_ws.Ws
{
    /// <summary>
    /// Summary description for wscal
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WsSecurityInspection : WebService
    {

        private readonly DownloadUserService _downloadUserService;
        private readonly UploadService _uploadService;
        private Data.Azure _azure;
        //private const string LogFilePath = @"C:\temp\WebServiceLog.txt"; 

        
        public WsSecurityInspection()
        {
            _downloadUserService = new DownloadUserService();
            _uploadService = new UploadService();
            _azure = new Data.Azure();
        }
        
        [WebMethod]
        public DataSet DameMS_Maestras(SimpleRequest request)
        {
            return _downloadUserService.GetMaestras(request);
        }
        
        [WebMethod]
        public async Task<string> ProbarConexionAzure()
        {
            try
            {
                var result = await _azure.TestAzureBlobStorageConnectionAsync();

                return $"Subida exitosa. URL: {result.ToString()}";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        
        [WebMethod]
        public MessageResponse GuardarSubidaContenedor(int dispositivo, string usuario_subida, DataSet ds, string uguid)
        {
            // Ejecutar la versión async de forma síncrona (esto puede causar deadlocks)
            return Task.Run(() => GuardarSubidaContenedorAsync(dispositivo, usuario_subida, ds, uguid)).GetAwaiter().GetResult();
        }

        private async Task<MessageResponse> GuardarSubidaContenedorAsync(int dispositivo, string usuario_subida, DataSet ds, string uguid)
        {
            LoggerHelper.LogMessage($"Inicio de GuardarSubidaContenedor. Dispositivo: {dispositivo}, Usuario: {usuario_subida}, UGUID: {uguid}, DataSet Tablas: {ds?.Tables?.Count ?? 0}");

            MessageResponse response = new MessageResponse();

            try
            {
                UploadRequest request = new UploadRequest()
                {
                    SubidaId = 0,
                    UsuarioSubidaId = usuario_subida,
                    DispositivoId = dispositivo,
                    UGUID = uguid,
                    FechaHoraRegistro = Convert.ToDateTime((DateTime.Now.ToString("s")).Replace("T", " ")),
                    SubidaExitosa = 0
                };
                LoggerHelper.LogMessage($"Request creado: {Newtonsoft.Json.JsonConvert.SerializeObject(request)}");

                response = _uploadService.UpladDataContainer(request);
                LoggerHelper.LogMessage($"Respuesta de UpladDataContainer: valueID = {response.valueID}, Mensaje = {response.mensaje}");

                if (response.valueID > 0)
                {
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                    {
                        LoggerHelper.LogMessage("DataSet vacío o sin la tabla esperada.");
                        response.mensaje = "DataSet vacío o inválido.";
                        response.valueID = 0;
                        return response;
                    }

                    DataTable dt = ds.Tables[0].Copy();
                    int cantsubidaexitosa = 0;
                    LoggerHelper.LogMessage($"Procesando {dt.Rows.Count} filas del DataSet.");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        DataRow dr = dt.Rows[i];
                        string tabla = dr["tabla"].ToString().ToUpper();
                        string datos = dr["datos"].ToString();
                        string datosParaGuardar = datos;
                        LoggerHelper.LogMessage($"Fila {i}: Tabla = {tabla}");
                        
                        bool errorEnProcesamientoEspecifico = false;

                        if (tabla == "[PLANTA].[INSPECCION]" && !string.IsNullOrEmpty(datos))
                        {
                            LoggerHelper.LogMessage($"Procesando tabla [PLANTA].[INSPECCION]. Datos originales (primeros 100 chars): {datos.Substring(0, Math.Min(100, datos.Length))}");
                            LoggerHelper.LogMessage($"Fila {i}: Datos = {datos}");
                            try
                            {
                                DataTable inspeccionTable = null;
                                DataSet tempDs = new DataSet();

                                using (StringReader sr = new StringReader(datos))
                                {
                                    tempDs.ReadXml(sr, XmlReadMode.Auto);
                                    
                                }
                                
                                // Verificar si se leyó alguna tabla y cuál usar
                                // El log muestra <MyTableName>, intentamos usar ese nombre.
                                if (tempDs.Tables.Contains("MyTableName"))
                                {
                                    inspeccionTable = tempDs.Tables["MyTableName"].Copy();
                                }
                                else if (tempDs.Tables.Count > 0) // Si "MyTableName" no existe, tomar la primera
                                {
                                    inspeccionTable = tempDs.Tables[0].Copy();
                                    LoggerHelper.LogMessage($"Advertencia: Tabla 'MyTableName' no encontrada en XML de Inspección, usando la primera tabla disponible: '{inspeccionTable.TableName}'.");
                                }
                                else
                                {
                                    // Si no hay tablas, esto es un error que el bloque catch manejará.
                                    LoggerHelper.LogMessage($"Error crítico: El XML de Inspección no contiene tablas o no pudo ser interpretado por DataSet.ReadXml. Datos XML (primeros 200 chars): {datos.Substring(0, Math.Min(200, datos.Length))}");
                                    throw new InvalidOperationException("El XML de Inspección no pudo ser convertido a DataTable porque no contenía tablas reconocibles.");
                                }
                                
                                LoggerHelper.LogMessage($"XML de Inspección deserializado. {inspeccionTable.Rows.Count} filas en inspeccionTable.");

                                foreach (DataRow inspeccionRow in inspeccionTable.Rows)
                                {
                                    string extension = ".png"; 
                                    string contentType = "image/png";

                                   // string accessLevel = await _azure.GetContainerAccessLevelAsync("agrosispkgseguridadtest");
                                    LoggerHelper.LogMessage($"_______________Comienzo de validaciones_____________");
                                    if (inspeccionRow.Table.Columns.Contains("firmaInspector") && inspeccionRow["firmaInspector"] != DBNull.Value) 
                                    {
                                        LoggerHelper.LogMessage($"firmaInspector__________");
    
                                        byte[] firmaBytes = null;

                                        // Verifica si el valor es string o byte[]
                                        if (inspeccionRow["firmaInspector"] is string firmaBase64)
                                        {
                                            try
                                            {
                                                firmaBytes = Convert.FromBase64String(firmaBase64);
                                                LoggerHelper.LogMessage($"[Depuración] firmaBase64 convertido a byte[]: {firmaBytes.Length} bytes");
                                            }
                                            catch (FormatException ex)
                                            {
                                                LoggerHelper.LogMessage($"[Error] La firma no está en formato Base64 válido: {ex.Message}");
                                            }
                                        }
                                        else if (inspeccionRow["firmaInspector"] is byte[] bytes)
                                        {
                                            firmaBytes = bytes;
                                            LoggerHelper.LogMessage($"[Depuración] firmaBytes ya es byte[]: {firmaBytes.Length} bytes");
                                        }
                                        else
                                        {
                                            LoggerHelper.LogMessage($"[Advertencia] firmaInspector no es string ni byte[]. Tipo: {inspeccionRow["firmaInspector"].GetType().FullName}");
                                        }
                                        LoggerHelper.LogMessage($"[Depuración] firmaBytes es {(firmaBytes == null ? "null" : firmaBytes.Length + " bytes")}");
                                        if (firmaBytes != null && firmaBytes.Length > 0)
                                        {
                                            LoggerHelper.LogMessage($"[firmaInspector] Tamaño de datos: {firmaBytes.Length} bytes");
                                            string guidFirma = inspeccionRow.Table.Columns.Contains("guidFirmaInspector") ? inspeccionRow["guidFirmaInspector"].ToString() : Guid.NewGuid().ToString();
                                            string blobName = $"firmaInspector_{uguid}_{guidFirma}{extension}";
                                            LoggerHelper.LogMessage($"Subiendo firmaInspector: {blobName}");
                                            string blobUrl = await _azure.UploadBytesToAzureStorage(firmaBytes, blobName, contentType).ConfigureAwait(false);
                                            //if (!inspeccionRow.Table.Columns.Contains("urlFirmaInspector")) inspeccionTable.Columns.Add("urlFirmaInspector", typeof(string));
                                            inspeccionRow["urlFirmaInspector"] = blobUrl;
                                            LoggerHelper.LogMessage($"firmaInspector subida a: {blobUrl}");
                                        }
                                    }
                                    if (inspeccionRow.Table.Columns.Contains("firmaSupervisor") && inspeccionRow["firmaSupervisor"] != DBNull.Value)
                                    {
                                        LoggerHelper.LogMessage($"firmaSupervisor___________");
    
                                        byte[] firmaBytes = null;

                                        // Verifica si el valor es string o byte[]
                                        if (inspeccionRow["firmaSupervisor"] is string firmaBase64)
                                        {
                                            try
                                            {
                                                firmaBytes = Convert.FromBase64String(firmaBase64);
                                                LoggerHelper.LogMessage($"[Depuración] firmaBase64 convertido a byte[]: {firmaBytes.Length} bytes");
                                            }
                                            catch (FormatException ex)
                                            {
                                                LoggerHelper.LogMessage($"[Error] La firma no está en formato Base64 válido: {ex.Message}");
                                            }
                                        }
                                        else if (inspeccionRow["firmaSupervisor"] is byte[] bytes)
                                        {
                                            firmaBytes = bytes;
                                            LoggerHelper.LogMessage($"[Depuración] firmaBytes ya es byte[]: {firmaBytes.Length} bytes");
                                        }
                                        else
                                        {
                                            LoggerHelper.LogMessage($"[Advertencia] firmaSupervisor no es string ni byte[]. Tipo: {inspeccionRow["firmaSupervisor"].GetType().FullName}");
                                        }
                                        LoggerHelper.LogMessage($"[Depuración] firmaBytes es {(firmaBytes == null ? "null" : firmaBytes.Length + " bytes")}");
                                        if (firmaBytes != null && firmaBytes.Length > 0)
                                        {
                                            LoggerHelper.LogMessage($"[firmaSupervisor] Tamaño de datos: {firmaBytes.Length} bytes");
                                            //byte[] firmaBytes = Convert.FromBase64String(inspeccionRow["firmaSupervisor"].ToString());
                                            string guidFirma = inspeccionRow.Table.Columns.Contains("guidFirmaSupervisor") ? inspeccionRow["guidFirmaSupervisor"].ToString() : Guid.NewGuid().ToString();
                                            string blobName = $"firmaSupervisor_{uguid}_{guidFirma}{extension}";
                                            LoggerHelper.LogMessage($"Subiendo firmaSupervisor: {blobName}");
                                            string blobUrl = await _azure.UploadBytesToAzureStorage(firmaBytes, blobName, contentType).ConfigureAwait(false);
                                            //if (!inspeccionRow.Table.Columns.Contains("urlFirmaSupervisor")) inspeccionTable.Columns.Add("urlFirmaSupervisor", typeof(string));
                                            inspeccionRow["urlFirmaSupervisor"] = blobUrl;
                                            LoggerHelper.LogMessage($"firmaSupervisor subida a: {blobUrl}");
                                        }
                                        
                                    }
                                    if (inspeccionRow.Table.Columns.Contains("firmaSeguridad") && inspeccionRow["firmaSeguridad"] != DBNull.Value)
                                    {
                                        LoggerHelper.LogMessage($"firmaSeguridad___________");
    
                                        byte[] firmaBytes = null;

                                        // Verifica si el valor es string o byte[]
                                        if (inspeccionRow["firmaSeguridad"] is string firmaBase64)
                                        {
                                            try
                                            {
                                                firmaBytes = Convert.FromBase64String(firmaBase64);
                                                LoggerHelper.LogMessage($"[Depuración] firmaBase64 convertido a byte[]: {firmaBytes.Length} bytes");
                                            }
                                            catch (FormatException ex)
                                            {
                                                LoggerHelper.LogMessage($"[Error] La firma no está en formato Base64 válido: {ex.Message}");
                                            }
                                        }
                                        else if (inspeccionRow["firmaSeguridad"] is byte[] bytes)
                                        {
                                            firmaBytes = bytes;
                                            LoggerHelper.LogMessage($"[Depuración] firmaBytes ya es byte[]: {firmaBytes.Length} bytes");
                                        }
                                        else
                                        {
                                            LoggerHelper.LogMessage($"[Advertencia] firmaSeguridad no es string ni byte[]. Tipo: {inspeccionRow["firmaSeguridad"].GetType().FullName}");
                                        }
                                        //byte[] firmaBytes = (byte[])inspeccionRow["firmaSeguridad"];
                                        LoggerHelper.LogMessage($"[Depuración] firmaBytes es {(firmaBytes == null ? "null" : firmaBytes.Length + " bytes")}");
                                        if (firmaBytes != null && firmaBytes.Length > 0)
                                        {
                                            LoggerHelper.LogMessage($"[firmaSeguridad] Tamaño de datos: {firmaBytes.Length} bytes");
                                            //byte[] firmaBytes = Convert.FromBase64String(inspeccionRow["firmaSeguridad"].ToString());
                                            string guidFirma = inspeccionRow.Table.Columns.Contains("guidFirmaSeguridad") ? inspeccionRow["guidFirmaSeguridad"].ToString() : Guid.NewGuid().ToString();
                                            string blobName = $"firmaSeguridad_{uguid}_{guidFirma}{extension}";
                                            LoggerHelper.LogMessage($"Subiendo firmaSeguridad: {blobName}");
                                            string blobUrl = await _azure.UploadBytesToAzureStorage(firmaBytes, blobName, contentType).ConfigureAwait(false);
                                            //if (!inspeccionRow.Table.Columns.Contains("urlFirmaSeguridad")) inspeccionTable.Columns.Add("urlFirmaSeguridad", typeof(string));
                                            inspeccionRow["urlFirmaSeguridad"] = blobUrl;
                                            LoggerHelper.LogMessage($"firmaSeguridad subida a: {blobUrl}");
                                        }
                                    }

                                }
                                
                                // Re-serializar la DataTable modificada a XML
                                using (StringWriter sw = new StringWriter())
                                {
                                    inspeccionTable.TableName = string.IsNullOrEmpty(inspeccionTable.TableName) ? "MyTableName" : inspeccionTable.TableName;
                                    inspeccionTable.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                                    datosParaGuardar = sw.ToString();

                                    LoggerHelper.LogMessage($"XML de Inspección modificado y re-serializado (primeros 100 chars): {datosParaGuardar.Substring(0, Math.Min(100, datosParaGuardar.Length))}");
                                }

                            }
                            catch (Exception e)
                            {
                                LoggerHelper.LogMessage($"Error procesando XML de Inspección: {e}");
                                // Manejar error de deserialización/procesamiento de firmas/serialización
                                // Quizás quieras guardar un detalle de error específico aquí
                                errorEnProcesamientoEspecifico = true; // Marcar que hubo un error
                                MessageResponse uploadErrorResponse = _uploadService.UpladDataDetail(new UploadDetailRequest
                                {
                                    SubidaId = response.valueID, Tabla = tabla + "_XML_PROCESSING_ERROR",
                                    Contenido = e.Message, Error = "1"
                                });
                                // Decide si continuar o no. Por ahora, continuaremos con los datos originales si hay error.
                                //datosParaGuardar = datos;
                                return uploadErrorResponse;
                            }
                        }
                        
                        UploadDetailRequest detailRequest = new UploadDetailRequest()
                        {
                            SubidaId = response.valueID,
                            Tabla = errorEnProcesamientoEspecifico ? tabla + "_XML_ORIGINAL_POR_ERROR" : tabla,
                            Contenido = datosParaGuardar,
                            Error = errorEnProcesamientoEspecifico ? "1" : "0"
                        };
                        LoggerHelper.LogMessage($"Guardando detalle: Tabla = {detailRequest.Tabla}, SubidaId = {detailRequest.SubidaId}, Error = {detailRequest.Error}");

                        MessageResponse responseDetail = _uploadService.UpladDataDetail(detailRequest);
                        LoggerHelper.LogMessage($"Respuesta de UpladDataDetail: valueID = {responseDetail.valueID}, Mensaje = {responseDetail.mensaje}");
                        if (responseDetail.valueID > 0 && !errorEnProcesamientoEspecifico)
                        {
                            cantsubidaexitosa++;
                        }
                        else if (responseDetail.valueID <= 0)
                        {
                            LoggerHelper.LogMessage($"Error en UpladDataDetail para tabla {tabla}. Retornando respuesta de detalle. Mensaje: {responseDetail.mensaje}");
                            return responseDetail;
                        }
                    }
                    
                    LoggerHelper.LogMessage($"Subidas de detalle procesadas. Subidas consideradas exitosas: {cantsubidaexitosa} de {dt.Rows.Count} filas totales.");
                    if (cantsubidaexitosa == dt.Rows.Count)
                    {
                        var simpleRequest = new SimpleRequest
                        {
                            searchValue1 = response.valueID.ToString()
                        };
                        LoggerHelper.LogMessage($"Procesando datos finales con searchValue1 = {simpleRequest.searchValue1}");
                        var finalResponse = _uploadService.ProcessData(simpleRequest);
                        LoggerHelper.LogMessage($"Respuesta de ProcessData: valueID = {finalResponse.valueID}, Error = {finalResponse.mensaje}, Mensaje = {finalResponse.error}");
                        return finalResponse;
                    }
                    else
                    {
                        LoggerHelper.LogMessage("No todas las subidas de detalle fueron exitosas. Retornando respuesta del contenedor.");
                        return new MessageResponse()
                        {
                            mensajeID = 0,
                            mensaje = "No todas las subidas de detalle fueron exitosas.",
                            valueID = response.valueID,
                            error = "0" // No es un error, solo una advertencia.
                        };
                    }
                    
                }
                else
                {
                    LoggerHelper.LogMessage("response.valueID no fue mayor que 0 después de UpladDataContainer. Retornando respuesta.");
                    return response;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogMessage($"Excepción general en GuardarSubidaContenedor: {e.ToString()}"); // Loguea el stack trace completo
                return new MessageResponse
                {
                    mensajeID = 0,
                    mensaje = e.Message,
                    valueID = 0,
                    error = e.Message
                };
            }
            finally
            {
                LoggerHelper.LogMessage($"Fin de GuardarSubidaContenedor. UGUID: {uguid}");
            }
        }
        
        [WebMethod]
        public MessageResponse ActualizarSubidaContenedor(int dispositivo, string usuario_subida, DataSet ds, string uguid)
        {
            LoggerHelper.LogMessage($"Inicio de ActualizarSubidaContenedorAsync. Dispositivo: {dispositivo}, Usuario: {usuario_subida}, UGUID: {uguid}, DataSet Tablas: {ds?.Tables?.Count ?? 0}");

            MessageResponse response = new MessageResponse();

            try
            {
                UploadRequest request = new UploadRequest()
                {
                    SubidaId = 0,
                    UsuarioSubidaId = usuario_subida,
                    DispositivoId = dispositivo,
                    UGUID = uguid,
                    FechaHoraRegistro = Convert.ToDateTime((DateTime.Now.ToString("s")).Replace("T", " ")),
                    SubidaExitosa = 0
                };
                LoggerHelper.LogMessage($"Request creado: {Newtonsoft.Json.JsonConvert.SerializeObject(request)}");

                response = _uploadService.UpladDataContainer(request);
                LoggerHelper.LogMessage($"Respuesta de UpladDataContainer: valueID = {response.valueID}, Mensaje = {response.mensaje}");

                if (response.valueID > 0)
                {
                    if (ds == null || ds.Tables.Count == 0 || ds.Tables[0] == null)
                    {
                        LoggerHelper.LogMessage("DataSet vacío o sin la tabla esperada.");
                        response.mensaje = "DataSet vacío o inválido.";
                        response.valueID = 0;
                        return response;
                    }

                    DataTable dt = ds.Tables[0].Copy();
                    int cantsubidaexitosa = 0;
                    LoggerHelper.LogMessage($"Procesando {dt.Rows.Count} filas del DataSet.");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        
                        DataRow dr = dt.Rows[i];
                        string tabla = dr["tabla"].ToString().ToUpper();
                        string datos = dr["datos"].ToString();
                        string datosParaGuardar = datos;
                        LoggerHelper.LogMessage($"Fila {i}: Tabla = {tabla}");
                        
                        UploadDetailRequest detailRequest = new UploadDetailRequest()
                        {
                            SubidaId = response.valueID,
                            Tabla = tabla,
                            Contenido = datosParaGuardar,
                            Error = "0"
                        };
                        LoggerHelper.LogMessage($"Guardando detalle: Tabla = {detailRequest.Tabla}, SubidaId = {detailRequest.SubidaId}, Error = {detailRequest.Error}");

                        MessageResponse responseDetail = _uploadService.UpladDataDetail(detailRequest);
                        LoggerHelper.LogMessage($"Respuesta de UpladDataDetail: valueID = {responseDetail.valueID}, Mensaje = {responseDetail.mensaje}");
                        if (responseDetail.valueID > 0)
                        {
                            cantsubidaexitosa++;
                        }
                        else if (responseDetail.valueID <= 0)
                        {
                            LoggerHelper.LogMessage($"Error en UpladDataDetail para tabla {tabla}. Retornando respuesta de detalle. Mensaje: {responseDetail.mensaje}");
                            return responseDetail;
                        }
                    }
                    
                    LoggerHelper.LogMessage($"Subidas de detalle procesadas. Subidas consideradas exitosas: {cantsubidaexitosa} de {dt.Rows.Count} filas totales.");
                    if (cantsubidaexitosa == dt.Rows.Count)
                    {
                        var simpleRequest = new SimpleRequest
                        {
                            searchValue1 = response.valueID.ToString()
                        };
                        LoggerHelper.LogMessage($"Procesando datos finales con searchValue1 = {simpleRequest.searchValue1}");
                        var finalResponse = _uploadService.ProcessDataUpdate(simpleRequest);
                        LoggerHelper.LogMessage($"Respuesta de ProcessData: valueID = {finalResponse.valueID}, Error = {finalResponse.mensaje}, Mensaje = {finalResponse.error}");
                        return finalResponse;
                    }
                    else
                    {
                        LoggerHelper.LogMessage("No todas las subidas de detalle fueron exitosas. Retornando respuesta del contenedor.");
                        return new MessageResponse()
                        {
                            mensajeID = 0,
                            mensaje = "No todas las subidas de detalle fueron exitosas.",
                            valueID = response.valueID,
                            error = "0" // No es un error, solo una advertencia.
                        };
                    }
                    
                }
                else
                {
                    LoggerHelper.LogMessage("response.valueID no fue mayor que 0 después de UpladDataContainer. Retornando respuesta.");
                    return response;
                }
            }
            catch (Exception e)
            {
                LoggerHelper.LogMessage($"Excepción general en GuardarSubidaContenedor: {e.ToString()}"); // Loguea el stack trace completo
                return new MessageResponse
                {
                    mensajeID = 0,
                    mensaje = e.Message,
                    valueID = 0,
                    error = e.Message
                };
            }
            finally
            {
                LoggerHelper.LogMessage($"Fin de GuardarSubidaContenedor. UGUID: {uguid}");
            }
        }
        
    }
}
