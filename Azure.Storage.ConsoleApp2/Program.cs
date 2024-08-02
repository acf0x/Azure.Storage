using Azure.Storage.Blobs;

namespace Azure.Storage.ConsoleApp2
{
    internal class Program
    {
        static string connectionString = "DefaultEndpointsProtocol=https;AccountName=DemoStorageACF;AccountKey=KEY;EndpointSuffix=core.windows.net";
        static BlobServiceClient client;
        static void Main(string[] args)
        {
            // Configurar el cliente
            client = new BlobServiceClient(connectionString);

            // Listado de contenedores
            var contenedores = client.GetBlobContainers();
            foreach (var container in contenedores)
            {
                Console.WriteLine($"Nombre del contenedor: {container.Name}");

                // Listado de Blobs
                var containerClient = client.GetBlobContainerClient(container.Name);
                var blobs = containerClient.GetBlobs();

                foreach (var blob in blobs)
                {
                    Console.WriteLine($"  > {blob.Name} ({blob.Properties.ContentType})");

                    var blobClient = containerClient.GetBlobClient(blob.Name);
                    blobClient.DownloadTo(@$"C:\CarpetaDeEjemplo\Azure\Azure.Storage.ConsoleApp2\{blob.Name}");
                }
                Console.WriteLine($"");
            }
        }
    }
}