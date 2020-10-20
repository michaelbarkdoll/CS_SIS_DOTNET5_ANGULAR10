dotnet new sln
dotnet sln add API/

Extensions:
NuGet Gallery by pcislo v0.0.22
C# by Microsoft v1.23.2
C# Extensions v1.3.6  by JosKreativ
Material Icon Theme v4.3.0 by Philipp Kief

F1-> NuGet Gallery
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Sqlite by Microsoft (Prelease checked) v5.0.0- API.csproj install


nuget.org
search for dotnet-ef
dotnet tool install --global dotnet-ef --version 5.0.0-rc.1.20451.13

F1-> NuGet Gallery
Microsoft.EntityFrameCore.Design

# Create a migration (stop application)
dotnet ef migrations add InitialCreate -o Data/Migrations

# Create the database
dotnet ef database update

Extension
sqlite v0.9.0 by alexcvzz


dotnet ef migrations add UserPasswordAdded
dotnet ef database update

nuget gallery:
system.identitymodel.tokens.jwt v6.7.1
microsoft.aspnetcore.authentication.jwtbearer by Microsoft v3.1.8


# Trust ssl
dotnet dev-certs https -ep ./certificate.crt --trust --format Pem

cp certificate.pem /etc/pki/ca-trust/source/anchors/
sudo update-ca-trust

Trust it in your browser on linux by visiting:
https://localhost:5001/api/users


# Entity Framework
Stop api server
```
dotnet ef migrations add ExtendedUserEntity
dotnet ef migrations remove #Remove migration created
```

How do we use conventions to:
  Make when user is deleted all photos are deleted
  Make photos not nullable

We need to fully define the relationship.

```
dotnet ef migrations add ExtendedUserEntity
dotnet ef database update
```


# Generate seed data
https://www.json-generator.com/


Now restarting app will automatically recreated db if it was dropped.

Automapper
- Allows us to map from one object to another (that is all it does)
- Allows us to map a Dto to a Entity to avoid sending back unneed information

nuget:
Automapper.Extensions.Microsoft.DependencyInjection v8.0.1


CloudinaryDotNet by Cloudinaryu v1.13.1


```
dotnet ef migrations add LikedEntityAdded
dotnet ef database update
```
