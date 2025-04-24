using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace proper_ws
{
    public class AzureStorage
    {
        
        private string connectionString = "BlobEndpoint=https://storagecomercialqa.blob.core.windows.net/filesdevproper;SharedAccessSignature=sp=rw&st=2024-04-26T15:02:42Z&se=2025-04-26T23:02:42Z&spr=https&sv=2022-11-02&sr=c&sig=UCcq1XsMy2Lhv0tH3nBGdHnWSDtDSMKn8rb21lagVJ4%3D";
        
        private BlobServiceClient blobServiceClient;

        public AzureStorage()
        {

            blobServiceClient = new BlobServiceClient(connectionString);

        }

        public async Task<int> UploadBlobAsyncToPath(string filePath, string name)
        {

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("filesdevproper");

            BlobClient blobClient = containerClient.GetBlobClient(name);

            FileStream fs = File.OpenRead(filePath);

            var a = await blobClient.UploadAsync(fs, true);

            return a.GetRawResponse().Status;
        }

        public async Task<int> UploadBlobAsyncToBase64(string base64, string name)
        {

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("filesdevproper");

            BlobClient blobClient = containerClient.GetBlobClient(name);

            byte[] binaryData = Convert.FromBase64String(base64);

            MemoryStream stream = new MemoryStream(binaryData);

            var a = await blobClient.UploadAsync(stream, true);

            return a.GetRawResponse().Status;
        }

        public async Task<string> DownloadBlobAsync(string blobName)
        {

            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("filesdevproper");

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();

            return blobClient.Uri.ToString();

        }
    }
}