using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Images.Resize
{
    public class AzureBlobHelper
    {
        // Get the Azure Blob Storage connection string from environment variables.
        static CloudStorageAccount storageAccountConnection
        {
            get
            {
                string storageConnection = System.Environment.GetEnvironmentVariable("AzureWebJobsStorage", EnvironmentVariableTarget.Process);
                return CloudStorageAccount.Parse(storageConnection);
            }
        }

        // Create an instance of CloudBlobClient to interact with the Blob Storage.
        static CloudBlobClient _blobClient
        {
            get
            {
                return storageAccountConnection.CreateCloudBlobClient();
            }
        }

        // Get the reference to the Blob Container.
        static CloudBlobContainer _blobContainer
        {
            get
            {
                string storageContainer = System.Environment.GetEnvironmentVariable("StorageContainerthumbnail", EnvironmentVariableTarget.Process);
                return _blobClient.GetContainerReference(storageContainer);
            }
            set { }
        }

        // Constructor to create the Azure Blob Container if it doesn't exist.
        public AzureBlobHelper()
        {
            _blobContainer.CreateIfNotExistsAsync();
        }

        /// <summary>
        /// Uploads a file to Azure Blob Storage.
        /// </summary>
        /// <param name="buffer">Byte array representing the file content.</param>
        /// <param name="filename">Name of the file.</param>
        /// <param name="containerName">Name of the Blob Container (optional).</param>
        /// <returns>The URI of the uploaded blob.</returns>
        public static async Task<string> SaveDataToAzureBlob(byte[] buffer, string filename, string containerName = "")
        {
            if (!string.IsNullOrWhiteSpace(containerName))
            {
                var myClient = storageAccountConnection.CreateCloudBlobClient();
                var blobContainer = myClient.GetContainerReference(containerName);
                await blobContainer.CreateIfNotExistsAsync();

                CloudBlockBlob _blockBlob = blobContainer.GetBlockBlobReference(filename);
                await _blockBlob.UploadFromByteArrayAsync(buffer, 0, buffer.Length);
                return _blockBlob.Uri.AbsoluteUri.ToString();
            }
            else
            {
                CloudBlockBlob _blockBlob = _blobContainer.GetBlockBlobReference(filename);
                await _blockBlob.UploadFromByteArrayAsync(buffer, 0, buffer.Length);
                return _blockBlob.Uri.AbsoluteUri.ToString();
            }
        }

        // ... (Rest of the methods have similar comments explaining their purpose)

    }

    public class BlobFile
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
