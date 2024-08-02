# Azure Storage

Este repositorio contiene dos aplicaciones: ConsoleApp1 y ConsoleApp2.

### ConsoleApp1
ConsoleApp1 es una aplicación de consola que interactúa con el servicio de almacenamiento de Azure utilizando peticiones HTTP directas. Su principal función es listar los contenedores de almacenamiento y sus blobs, y descargar los blobs del contenedor a un directorio local.  
Diferencia el tratamiento de los blobs según su tipo de contenido, escribiéndolos como texto plano o como archivos binarios.
  
  
### ConsoleApp2
ConsoleApp2 es una aplicación de consola que utiliza la biblioteca de cliente Azure.Storage.Blobs para interactuar con el servicio de almacenamiento de Azure. Su principal función es listar los contenedores de almacenamiento y sus blobs, y descargar todos los blobs a un directorio local.
