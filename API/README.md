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

Nuget Identity Framework
```
microsoft.aspnetcore.identity.entityframeworkcore (preview) 5.0.100-rc.1.20452.10(same version you installed)
API.csproj
```

```
dotnet ef migrations add IdentityAdded
```

```
mkdir API/SignalR
```



EF doesn't support NoSQL databases like MongoDB...


docs.microsoft.com/en-us/ef/core/providers/?tabs=dotnet-core-cli


docker!!!

docker.com/get-started
Docker Desktop
Dashboard

hub.docker.com
postgres

hub.docker.com/_/postgres
```
docker run --name dev -e POSTGRES_USER=appuser -e POSTGRES_PASSWORD=Pa$$w0rd -p 5432:5432 -d postgres:latest
```


```
docker inspect 1b415b3b384f
```

```
"POSTGRES_USER=appuser",
                "POSTGRES_PASSWORD=Pa16173w0rd",
```

pgadmin.org

http://127.0.0.1:56861/browser

```
sudo rpm -i https://ftp.postgresql.org/pub/pgadmin/pgadmin4/yum/pgadmin4-redhat-repo-1-1.noarch.rpm
# Install for both desktop and web modes.
sudo yum install pgadmin4

# Install for desktop mode only.
sudo yum install pgadmin4-desktop

# Install for web mode only.
sudo yum install pgadmin4-web
```

Create Server
dev
connection
localhost
5432
appuser
Pa6340w0rd

# Switch EF from SQLITE to Postgres
```
dotnet ef databse drop
```

Npgsql.EntityFrameworkCore.PostgreSQL
v5.0.0.rc1
NuGet Gallery


appsettings.Development.json
```
  "ConnectionStrings" : {
    "DefaultConnection": "Data source=app.db"
  },
```

Change to a postgres connection string:
```
  "ConnectionStrings" : {
    "DefaultConnection": "Server=localhost; Port=5432; User Id=appuser; Password=Pa16173w0rd; Database=sis"
  },
```

ApplicationServiceExtensions.cs
```
            services.AddDbContext<DataContext>(options =>
            {
                //options.UseSqlite("Connection string");
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
```

```
            services.AddDbContext<DataContext>(options =>
            {
                //options.UseSqlite("Connection string");
                //options.UseSqlite(config.GetConnectionString("DefaultConnection"));
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
```

```
dotnet ef migrations add PostgresInitial -o Data/Migrations
```

```
dotnet watch run
```

devcenter.heroku.com/articles/heroku-cli

Heroku CLI
```
sudo snap install --classic heroku
```

```
$ heroku --version
 ›   Warning: Our terms of service have changed: https://dashboard.heroku.com/terms-of-service
heroku/7.46.2 linux-x64 node-v12.16.2
$ heroku
```

Heroku doens't offically support dotnet

We're using a community building pack (google search)
heroku dotnet buildpack

https://elements.heroku.com/buildpacks/jincod/dotnetcore-buildpack

.NET Core Preview release
```
heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack#preview
```

Create a new app in heroku

cssiuc-sis

```
$ heroku login
```

```
heroku git:remote -a cssiuc-sis
```

```
heroku buildpacks:set https://github.com/jincod/dotnetcore-buildpack#preview
```

Resources Addon
postgres
Heroku Postgres
Hobby Dev - Free

Settings
Config Vars

```
CloudinarySettings:CloudName
demwnups5
CloudinarySettings:ApiKey
822136612579413
CloudinarySettings:ApiSecret
TokenKey

```


```
heroku config:set ASPNETCORE_ENVIRONMENT=Production
```

```
git push heroku master
```

passwordsgenerator.net
32 length
```
heroku config:set TokenKey=
```

http://cssiuc-sis.herokuapp.com/
https://cssiuc-sis.herokuapp.com/

```
FileRepoSettings:RepoDirectory
/sis
FileRepoSettings:PhotosDirectory
userphotos
FileRepoSettings:DocumentsDirectory
documents
FileRepoSettings:UserFilesDirectory
userfiles
```

"FileRepoSettings": {
    "RepoDirectory": "/sis",
    "PhotosDirectory": "userphotos",
    "DocumentsDirectory": "documents",
    "UserFilesDirectory": "userfiles"

Switch to new branch
```
git checkout -b FixMessages
```
```
git add .
git commit -m "Applied fix"
git branch -a
git push origin FixMessages
```

Publishes our branch into GitHub, doesn't trigger into Heroku.



```
git checkout master
git merge FixMessages
```

```
ng build --prod
```



Dockerfile
```
#FROM mcr.microsoft.com/dotnet/nightly/runtime:5.0
FROM mcr.microsoft.com/dotnet/nightly/aspnet:5.0

COPY bin/Release/net5.0/publish/ /App/
WORKDIR /App
ENTRYPOINT ["dotnet", "/App/NetCore.Docker.dll"]
```