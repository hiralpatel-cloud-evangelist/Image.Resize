using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Images.Resize;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

public class ImageResize
{
    // Azure Function triggered by a blob event.
    [FunctionName("ImageResize")]
    public async Task Run([BlobTrigger("images/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
    {
        try
        {
            log.LogInformation($"C# Blob trigger function processed blob\n Name: {name} \n Size: {myBlob.Length} Bytes");

            // Allowed image file extensions.
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };

            // Get the file extension of the blob.
            string fileExtension = Path.GetExtension(name);

            // Check if the file extension is allowed.
            if (allowedExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
            {
                // Create a new memory stream to hold blob data.
                using (var newMemoryStream = new MemoryStream())
                {
                    log.LogInformation("Creating base64 thumbnail...");

                    // Copy blob data to the memory stream.
                    myBlob.CopyTo(newMemoryStream);
                    newMemoryStream.Position = 0;
                    byte[] byteData;
                    byteData = ReadStream(newMemoryStream, Convert.ToInt32(newMemoryStream.Length));

                    var folderName = "thumbnail";
                    // Save the data as a thumbnail in Azure Blob Storage.
                    var blobpath = await AzureBlobHelper.SaveDataToAzureBlob(byteData, name, folderName);
                }
            }
            else
            {
                log.LogInformation($"Blob '{name}' has an unsupported extension '{fileExtension}'. Skipping processing.");
            }
        }
        catch (Exception ex)
        {
            log.LogError($"An error occurred: {ex.Message}");
            throw;
        }
    }

    // Read data from a stream into a byte array.
    private byte[] ReadStream(Stream stream, int initialLength, ILogger _log = null)
    {
        try
        {
            if (initialLength < 1)
            {
                initialLength = 32768;
            }

            byte[] buffer = new byte[initialLength];
            int read = 0;
            int chunk;

            // Read stream data into the buffer.
            while ((chunk = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                read += chunk;
                if (read == buffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte == -1)
                    {
                        return buffer;
                    }
                    byte[] newBuffer = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuffer, buffer.Length);
                    newBuffer[read] = (byte)nextByte;
                    buffer = newBuffer;
                    read++;
                }
            }

            byte[] bytes = new byte[read];
            Array.Copy(buffer, bytes, read);
            return bytes;
        }
        catch (Exception ex)
        {
            if (_log != null)
            {
                _log.LogError("ReadStream :: " + ex.Message);
            }
            throw ex;
        }
    }
}
