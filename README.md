# CS_SIS_DOTNET5_ANGULAR10

Sample Application using .NET 5 (formly Dotnet Core 5.0) with Angular 10.



# Angular CLI

http://cli.angular.io

Angular 10 requires:   
    "node": ">= 10.13.0",
    "npm": ">= 6.11.0",

```
node --version
v12.18.4
npm --version
6.14.6
```

```
nvm use v12.18.4
```

```
npm install -g @angular/cli
```

```
ng new client
```

```
? Would you like to add Angular routing? Yes
? Which stylesheet format would you like to use? CSS
```




ng new my-dream-app
cd my-dream-app
ng serve



# VS Code extensions for Angular applications
Angular Language Service v0.1000.8 by Angular
Angular Snippets (Version 9) by John Papa v9.1.2
Bracket Pair Colorizer 2 v0.2.0 by CoenraadS


openssl req -newkey rsa:2048 -nodes -keyout privkey.pem -x509 -days 36500 -out certificate.pem -subj "/C=US/ST=NRW/L=Earth/O=CompanyName/OU=IT/CN=www.example.com/emailAddress=email@example.com"


# Dotnet sdk

```
snap list
Name           Version                Rev    Tracking       Publisher    Notes
core           16-2.47.1              10185  latest/stable  canonical✓   core
core18         20200929               1932   latest/stable  canonical✓   base
dotnet-sdk     5.0.100-rc.2.20479.15  103    5.0/beta       dotnetcore✓  classic
heroku         v7.46.2                3999   latest/stable  heroku✓      classic
sqlitebrowser  3.12.0-3401-5664e28    2501   latest/stable  gajjgndu     -
```

```
snap refresh dotnet-sdk
```

```
sudo snap install dotnet-sdk --classic --channel=5.0
```

Next, register the dotnet command for the system with the snap alias command:

```
sudo snap alias dotnet-sdk.dotnet dotnet
```

