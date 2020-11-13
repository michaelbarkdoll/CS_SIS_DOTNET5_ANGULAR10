# Console App Example:

dotnet new console -o App -n NetCore.Docker
dotnet run
dotnet publish -c Release

# Create Dockerfile
```
FROM mcr.microsoft.com/dotnet/nightly/runtime:5.0
#FROM mcr.microsoft.com/dotnet/nightly/aspnet:5.0

COPY bin/Release/net5.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "/App/NetCore.Docker.dll"]
```

docker build -t counter-image -f Dockerfile .

docker run -it --rm counter-image 3

# WebAPI example
```
mkdir -p ~/docker-working/WebAPI
cd ~/docker-working/WebAPI
dotnet new webapi -o App -n NetCore.Docker
dotnet run
dotnet publish -c Release
```

# Create Dockerfile
~/docker-working/WebAPI/App/Dockerfile
```
FROM mcr.microsoft.com/dotnet/nightly/runtime:5.0

COPY bin/Release/net5.0/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "/App/NetCore.Docker.dll"]
```

```
docker build -t webapi-image -f Dockerfile .
#docker run -it --rm webapi-image

docker run -it --rm -p 8080:80 webapi-image bash
```

http://localhost:8080/weatherforecast


To add https support inside the docker container:
https://github.com/dotnet/dotnet-docker/blob/master/samples/run-aspnetcore-https-development.md



```
#dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p crypticpassword
#dotnet dev-certs https --trust
#dotnet user-secrets -p aspnetapp\aspnetapp.csproj set "Kestrel:Certificates:Development:Password" "crypticpassword"


dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p 61068383469255809617
dotnet user-secrets -p NetCore.Docker.csproj set "Kestrel:Certificates:Development:Password" "61068383469255809617"
#docker build --pull -t aspnetapp .
#docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v %APPDATA%\microsoft\UserSecrets\:/root/.microsoft/usersecrets -v %USERPROFILE%\.aspnet\https:/root/.aspnet/https/ aspnetapp

#/etc/pki/ca-trust/source/anchors/certificate.pem
docker run --rm -it -p 8000:80 -p 8001:443 -e ASPNETCORE_URLS="https://+;http://+" -e ASPNETCORE_HTTPS_PORT=8001 -e ASPNETCORE_ENVIRONMENT=Development -v /etc/pki/ca-trust/source/anchors/:/root/.microsoft/usersecrets NetCore.Docker
```