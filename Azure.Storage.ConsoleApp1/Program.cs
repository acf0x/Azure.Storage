using StorageRestApiAuth;
using System.Globalization;
using System.Xml.Linq;

namespace Azure.Storage.ConsoleApp1
{
    internal class Program
    {
        static string storageName = "DemoStorageACF";
        static string storageKey = @"KEY";

        static void Main(string[] args)
        {
            Console.Clear();
            GetListContainers();
        }

        static void GetListContainers()
        {
            HttpClient http = new HttpClient();
            string url = $"https://{storageName}.blob.core.windows.net/?comp=list";


            // Mensaje de petición al API

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Content = null;

            DateTime fecha = DateTime.UtcNow;

            request.Headers.Add("x-ms-date", fecha.ToString("R", CultureInfo.InvariantCulture));
            request.Headers.Add("x-ms-version", "2024-08-04");
            request.Headers.Authorization = AzureStorageAuthenticationHelper
                .GetAuthorizationHeader(storageName, storageKey, fecha, request);


            // Mensaje de respuesta del API

            HttpResponseMessage response = http.Send(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseData = response.Content.ReadAsStringAsync().Result;
                XElement xml = XElement.Parse(responseData);
                foreach (var element in xml.Element("Containers").Elements("Container"))
                {
                    Console.WriteLine($"Nombre del Contenedor: {element.Element("Name").Value}");
                    GetListBlobs(element.Element("Name").Value);
                }
            }

            else Console.WriteLine($"Error: {response.StatusCode}");
        }

        static void GetListBlobs(string containerName)
        {
            HttpClient http = new HttpClient();
            string url = $"https://{storageName}.blob.core.windows.net/{containerName}?restype=container&comp=list";

            // Mensaje de petición al API

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Content = null;

            DateTime fecha = DateTime.UtcNow;

            request.Headers.Add("x-ms-date", fecha.ToString("R", CultureInfo.InvariantCulture));
            request.Headers.Add("x-ms-version", "2024-08-04");
            request.Headers.Authorization = AzureStorageAuthenticationHelper
                .GetAuthorizationHeader(storageName, storageKey, fecha, request);


            // Mensaje de respuesta del API

            HttpResponseMessage response = http.Send(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseData = response.Content.ReadAsStringAsync().Result;
                XElement xml = XElement.Parse(responseData);
                foreach (var element in xml.Element("Blobs").Elements("Blob"))
                {
                    Console.WriteLine($"  > {element.Element("Name").Value}");
                    if (containerName == "publico") GetBlob(containerName, element.Element("Name").Value);
                }
                Console.WriteLine("========================================");
            }

            else Console.WriteLine($"Error: {response.StatusCode}");
        }

        static void GetBlob(string containerName, string blobName)
        {
            HttpClient http = new HttpClient();
            string url = $"https://{storageName}.blob.core.windows.net/{containerName}/{blobName}";


            // Mensaje de petición al API

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Content = null;

            DateTime fecha = DateTime.UtcNow;

            request.Headers.Add("x-ms-date", fecha.ToString("R", CultureInfo.InvariantCulture));
            request.Headers.Add("x-ms-version", "2024-08-04");
            request.Headers.Authorization = AzureStorageAuthenticationHelper
                .GetAuthorizationHeader(storageName, storageKey, fecha, request);


            // Mensaje de respuesta del API

            HttpResponseMessage response = http.Send(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine($"    - Content-Type: {response.Content.Headers.ContentType.MediaType}");
                switch (response.Content.Headers.ContentType.MediaType.ToString().ToLower())
                {
                    case "text/plain":
                        string contenido = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine(contenido);

                        // Opción 1
                        StreamWriter writer = new StreamWriter(@$"C:\CarpetaDeEjemplo\Azure\Azure.Storage.ConsoleApp1\{blobName}");
                        writer.Write(contenido);
                        writer.Close();
                        writer.Dispose();

                        // Opción 2
                        File.WriteAllText(@$"C:\CarpetaDeEjemplo\Azure\Azure.Storage.ConsoleApp1\2_{blobName}", contenido);

                        break;

                    default:
                        var file = new FileStream(@$"C:\CarpetaDeEjemplo\Azure\Azure.Storage.ConsoleApp1\{blobName}", FileMode.Create, FileAccess.Write);
                        response.Content.ReadAsStream().CopyTo(file);
                        file.Close();
                        file.Dispose();
                        break;
                }
            }

            else Console.WriteLine($"Error: {response.StatusCode}");
        }
    }
}