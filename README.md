# Clientes API

Esta breve guía ayudará para poder correr el sistema en modo desarrollo.

### Pasos:

> Antes de iniciar es necesario que tenga intalado el CLI de dotnet y el de Entity Framework. Ademas necesita tener instalado SQL Server y conocer el nombre del servidor, y credenciales para acceder al mismo.

1- Descargar o hacer fork de la aplicación.

2- Abrir el proyecto en el editor de código de su preferencia, preferiblemente: Visual Studio, Visual Studi Code or Rider.

4- Actualizar la cadena de conexion ubicada en el archivo appsettings.json con los datos de su servidor SQL.

3- Una vez en el proyecto necesita estar en la terminal en la dirección Customers.API, para hacer esto abra la carpeta del proyecto en la terminal y utilice el siguiente comando:

`cd Customers.API`

4- una vez en la ruta /Customers.API necesita correr el siguiente comando:
 `dotnet-ef migrations add InitCustomerDdContext --context CustomerDbContext --project ../Customers.INFRASTRUCTURE
`

5- Necesita actualizar la base de datos, para ello necesita correr el siguietne comando:
`dotnet-ef database update --context CustomerDbContext --project ../Customers.INFRASTRUCTURE`

6- Luego puede iniciar la aplicación usando el comando:
`dotnet run`

7- Puede usar la interfaz gráfica de Swagger, o puede utilizar el Cliente REST de su preferencia para comenzar a utilizar el API

Happy Testing 🤓
