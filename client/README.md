# Client

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 10.1.3.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).


```
ng generate -h
```

# New component
```
cd client/src/app
ng generate component nav --skip-tests
```

```
CREATE src/app/nav/nav.component.css (0 bytes)
CREATE src/app/nav/nav.component.html (18 bytes)
CREATE src/app/nav/nav.component.ts (263 bytes)
UPDATE src/app/app.modu
```

# New Service
```
cd client/src/app
mkdir _services
cd _services
```

```
ng generate service account --skip-tests
```

```
CREATE src/app/_services/account.service.ts (136 bytes)
```

```
cd client/src/app
ng generate component home --skip-tests
```

```
CREATE src/app/home/home.component.css (0 bytes)
CREATE src/app/home/home.component.html (19 bytes)
CREATE src/app/home/home.component.ts (267 bytes)
UPDATE src/app/app.module.ts (879 bytes)
```

```
cd client/src/app
ng generate component register --skip-tests
```

# members folder for several members components
```
cd client/src/app
mkdir members
cd members
ng generate component member-list --skip-tests
ng generate component member-detail --skip-tests
cd ..
ng generate component lists --skip-tests
ng generate component messages --skip-tests
```

# Add toaster notification service ngxToaster v13.0.1
```
cd client
npm install ngx-toastr
```

Styling was added to angular.json for this package.

            "styles": [
              "./node_modules/bootstrap/dist/css/bootstrap.min.css",
              "./node_modules/ngx-bootstrap/datepicker/bs-datepicker.css",
              "./node_modules/font-awesome/css/font-awesome.css",
              "./node_modules/ngx-toastr/toastr.css",

Restart the app

```
cd client/src/app/_guards
ng generate guard auth --skip-tests
```

```
? Which interfaces would you like to implement? CanActivate
CREATE src/app/_guards/auth.guard.ts (457 bytes)
```

bootswatch.com
Has bootstrap themes.


```
cd client
npm install bootswatch
```

angular.json file

            "styles": [
              "./node_modules/bootstrap/dist/css/bootstrap.min.css",
              "./node_modules/ngx-bootstrap/datepicker/bs-datepicker.css",
              "./node_modules/bootswatch/dist/united/bootstrap.css",


Store modules that app.modules.ts import
```
cd src/app
mkdir _modules
cd _modules
ng generate module shared --flat
```

```
CREATE src/_modules/shared.module.ts (192 bytes)
```

