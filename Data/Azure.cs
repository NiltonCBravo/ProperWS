using Azure;
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using proper_ws.Utils;
using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
namespace proper_ws.Data
{
    public class Azure
    {
        private readonly string connectionString;
        private readonly BlobServiceClient _blobServiceClient;
        
        private const string AzureStorageContainerName = "agrosispkgseguridadprod";
        
        public Azure()
        {
            connectionString = ConfigurationManager.AppSettings["AzureBlobStorageConnectionString"];
            LoggerHelper.LogMessage($"[Azure Constructor] Cadena de conexión: {(string.IsNullOrWhiteSpace(connectionString) ? "VACÍA o NULA" : "CARGADA")}");
            _blobServiceClient = new BlobServiceClient(connectionString);
        }
        
        public async Task<bool> UploadFileAsync(string containerName, string blobName, string base64)
        {
            try
            {
                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();
                var blobClient = containerClient.GetBlobClient(blobName);
                var binaryData = Convert.FromBase64String(base64);
                var stream = new MemoryStream(binaryData);
                Response<BlobContentInfo> response = await blobClient.UploadAsync(stream, overwrite: true);
                // Si llegó aquí sin excepción, la subida fue exitosa
                return response != null && response.GetRawResponse().Status == 201;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error al subir archivo: {e.Message}");
                return false;
            }
            
        }
        
        /*public async Task<string> UploadBytesToAzureStorage(byte[] dataBytes, string blobName, string contentType)
        {
            LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] Iniciando subida: {blobName}");
            if (dataBytes == null || dataBytes.Length == 0)
            {
                LoggerHelper.LogMessage("[UploadBytesToAzureStorage] dataBytes es nulo o vacío.");
                // Decide si lanzar excepción o retornar null/string vacío
                // throw new ArgumentNullException(nameof(dataBytes), "Los bytes no pueden ser nulos o vacíos.");
                return string.Empty; // O null, dependiendo de cómo quieras manejarlo
            }
            try
            {
            
                LoggerHelper.LogMessage("[UploadBytesToAzureStorage] Obteniendo containerClient...");
                var containerClient = _blobServiceClient.GetBlobContainerClient(AzureStorageContainerName);

                /*LoggerHelper.LogMessage("[UploadBytesToAzureStorage] Creando contenedor si no existe...");
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);#1#

                LoggerHelper.LogMessage("[UploadBytesToAzureStorage] Obteniendo blobClient...");
                var blobClient = containerClient.GetBlobClient(blobName);

                LoggerHelper.LogMessage("[UploadBytesToAzureStorage] Subiendo stream al blob...");
                using (var stream = new MemoryStream(dataBytes))
                {
                    /*var uploadOptions = new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders { ContentType = contentType }
                    };#1#

                    await blobClient.UploadAsync(stream, true);
                }

                string url = blobClient.Uri.ToString();
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] Subida completada. URL: {url}");
                return url;
            }
            catch (Exception e)
            {
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] ERROR: {e.Message}");
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] StackTrace: {e.StackTrace}");
                return string.Empty;
            }
            
        }*/
        
        public async Task<string> UploadBytesToAzureStorage(byte[] dataBytes, string blobName, string contentType)
        {
            LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] Iniciando subida: {blobName}");
            
            if (dataBytes == null || dataBytes.Length == 0)
            {
                LoggerHelper.LogMessage("[UploadBytesToAzureStorage] dataBytes es nulo o vacío.");
                return string.Empty;
            }

            try
            {
                LoggerHelper.LogMessage("[UploadBytesToAzureStorage] Obteniendo containerClient...");
                var containerClient = _blobServiceClient.GetBlobContainerClient(AzureStorageContainerName);

                // Verificar que el contenedor exista (descomentar si es necesario)
                await containerClient.CreateIfNotExistsAsync().ConfigureAwait(false);
                
                LoggerHelper.LogMessage("[UploadBytesToAzureStorage] Obteniendo blobClient...");
                var blobClient = containerClient.GetBlobClient(blobName);

                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] Tamaño de datos: {dataBytes.Length} bytes");
                LoggerHelper.LogMessage("[UploadBytesToAzureStorage] Subiendo stream al blob...");

                // Configurar opciones de subida
                var blobUploadOptions = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders 
                    {
                        ContentType = contentType
                    },
                    TransferOptions = new StorageTransferOptions
                    {
                        InitialTransferSize = 4 * 1024 * 1024, // 4MB
                        MaximumTransferSize = 4 * 1024 * 1024, // 4MB
                        MaximumConcurrency = Environment.ProcessorCount * 2
                    }
                };

                /*try
                {*/
                    using (var stream = new MemoryStream(dataBytes))
                    {
                        // Configurar timeout (ejemplo: 10 minutos)
                        /*var cancellationTokenSource = new CancellationTokenSource();
                        cancellationTokenSource.CancelAfter(TimeSpan.FromMinutes(10));

                        var response = await blobClient.UploadAsync(
                            stream,
                            blobUploadOptions,
                            cancellationTokenSource.Token);*/
                        var response = await blobClient.UploadAsync(stream, blobUploadOptions).ConfigureAwait(false);
                        LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] Subida completada. ETag: {response.Value.ETag}");
                    }
                /*
                }
                catch  (RequestFailedException ex) when (ex.ErrorCode == "ContainerNotFound")
                {
                    LoggerHelper.LogMessage("[UploadBytesToAzureStorage] ERROR: El contenedor no existe");
                    // Crear el contenedor y reintentar
                   
                }
                catch (Exception ex)
                {
                    LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] ERROR CRÍTICO: {ex.ToString()}");
                    throw; // Relanzar para ver el error completo
                }
                */
                
                

                string url = blobClient.Uri.ToString();
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] URL generada: {url}");
                return url;
            }
            catch (RequestFailedException azureEx)
            {
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] ERROR de Azure: {azureEx.Message}");
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] Código de error: {azureEx.ErrorCode}");
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] Status: {azureEx.Status}");
                return string.Empty;
            }
            catch (OperationCanceledException canceledEx)
            {
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] Operación cancelada: {canceledEx.Message}");
                return string.Empty;
            }
            catch (Exception e)
            {
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] ERROR: {e.Message}");
                LoggerHelper.LogMessage($"[UploadBytesToAzureStorage] StackTrace: {e.StackTrace}");
                return string.Empty;
            }
        }
        public async Task<bool> TestAzureBlobStorageConnectionAsync()
        {
            string containerName = "agrosispkgseguridadprod"; // Cambia si necesitas
            string testBlobName = $"test_connection_{Guid.NewGuid()}.txt";
            string testContent = "Hola desde WebService - prueba de conexión";

            try
            {
                LoggerHelper.LogMessage("[TestAzureBlobStorageConnection] Iniciando prueba...");

                var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
                LoggerHelper.LogMessage("[TestAzureBlobStorageConnection] Obteniendo containerClient...");

                try
                {
                    LoggerHelper.LogMessage("[TestAzureBlobStorageConnection] Intentando crear/verificar contenedor...");
                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob).ConfigureAwait(false);
                    LoggerHelper.LogMessage("[TestAzureBlobStorageConnection] Contenedor creado/verificado correctamente.");
                }
                catch (Exception ex)
                {
                    LoggerHelper.LogMessage($"[TestAzureBlobStorageConnection] ERROR al crear/verificar contenedor: {ex}");
                    return false;
                }

                var blobClient = containerClient.GetBlobClient(testBlobName);
                LoggerHelper.LogMessage($"[TestAzureBlobStorageConnection] Subiendo archivo de prueba: {testBlobName}");

                using (MemoryStream stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(testContent)))
                {
                    var uploadOptions = new BlobUploadOptions
                    {
                        HttpHeaders = new BlobHttpHeaders { ContentType = "text/plain" }
                    };
                    await blobClient.UploadAsync(stream, uploadOptions).ConfigureAwait(false);
                }

                LoggerHelper.LogMessage($"[TestAzureBlobStorageConnection] Subida exitosa. Blob URL: {blobClient.Uri}");

                return true;
            }
            catch (Exception ex)
            {
                LoggerHelper.LogMessage($"[TestAzureBlobStorageConnection] ERROR: {ex}");
                return false;
            }
        }
    }
}