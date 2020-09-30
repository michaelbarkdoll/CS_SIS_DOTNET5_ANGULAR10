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


