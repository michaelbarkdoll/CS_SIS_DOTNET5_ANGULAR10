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


```
cd src/app
mkdir errors
cd errors
ng g c test-errors --skip-tests
```


```
cd src/app
mkdir _interceptors
cd _interceptors
ng g interceptor error --skip-tests
```

```
CREATE src/app/_interceptors/error.interceptor.ts (410 bytes)
```

```
cd src/app
cd errors
ng g c not-found --skip-tests
```

```
cd src/app
cd errors
ng g c server-error --skip-tests
```


# Create a new service
```
cd src/app/_services
ng g s members --skip-tests
```

```
cd src/app/members
ng g c member-card --skip-tests
```

# Create a interceptor to add token with requests
```
cd src/app/_interceptors
ng g interceptor jwt --skip-tests
```

```
CREATE src/app/_interceptors/jwt.interceptor.ts (408 bytes)
```

ngs tabs
https://valor-software.com/ngx-bootstrap/#/tabs

Google search
ngx gallery (3 year old one is abandoned)
https://www.npmjs.com/package/@kolkov/ngx-gallery

```
cd client
npm install @kolkov/ngx-gallery --save
```

```
cd client/src/app/members
ng g c member-edit --skip-tests
```

```
cd client/src/app/_guards
ng g guard prevent-unsaved-changes --skip-tests
```

```
? Which interfaces would you like to implement? CanDeactivate
CREATE src/app/_guards/prevent-unsaved-changes.guard.ts (564 bytes)
```


# Loading indicator

ngx-spinner v10.0.1
https://www.npmjs.com/package/ngx-spinner
```
cd client
###npm install ngx-spinner --save
npm install @angular/cdk
ng add ngx-spinner
```

```
✔ Packages installed successfully.
UPDATE src/app/app.module.ts (2472 bytes)
```

Create server to help with ngx-spinner
```
cd client/src/app/_services
ng g service busy --skip-tests
```
```
CREATE src/app/_services/busy.service.ts (133 bytes)
```


Create interceptor for the busy spinner
```
cd client/src/app/_interceptors
ng g interceptor loading --skip-tests
```

```
CREATE src/app/_interceptors/loading.interceptor.ts (412 bytes)
```

State management solutions:
Redux, modex .. too much for this application

We'll use the member service to store our data.


```
cd client/src/app/members
ng g c photo-editor --skip-tests
```

ng2-file-upload

http://valor-software.com/ng2-file-upload
```
npm install ng2-file-upload --save
```

```
cd client/src/app
mkdir _forms
cd client/src/app/_forms
ng g c text-input --skip-tests
```

ngx angular package comes with a date picker from ngx-bootstrap components we put it in our shared module
BsDatepickerModule.forRoot()

```
cd client/src/app/_forms
ng g c date-input --skip-tests
```

ngx-bootstrap pagination
https://valor-software.com/ngx-bootstrap/#/pagination

ngx-bootstrap buttons
https://valor-software.com/ngx-bootstrap/#/buttons
```
import { ButtonsModule } from 'ngx-bootstrap/buttons';

@NgModule({
  imports: [ButtonsModule.forRoot(),...]
})
```

npmjs.com/package/ngx-timeago
```
cd client
npm install ngx-timeago
```

```
import { TimeagoModule } from 'ngx-timeago';
 
@NgModule({
  imports: [
    BrowserModule,
    TimeagoModule.forRoot()
  ],
```


```
cd client/src/app/_services
ng g s message --skip-tests
```


```
cd client/src/app/members
ng g c member-messages --skip-tests
```


```
cd client/src/app
mkdir _resolvers
cd client/src/app/_resolvers
```


Admin components
```
cd client/src/app
mkdir admin
cd client/src/app/admin
ng g c admin-panel --skip-tests
```

```
cd client/src/app/_guards
ng g guard admin --skip-tests
? Which interfaces would you like to implement? CanActivate
CREATE src/app/_guards/admin.guard.ts (458 bytes)
```

```
cd client/src/app/
mkdir _directives
ng g -h
```

```
cd client/src/app/_directives
ng g directive has-role --skip-tests
```

```
CREATE src/app/_directives/has-role.directive.ts (143 bytes)
UPDATE src/app/app.module.ts (3207 bytes)
```

```
cd client/src/app/admin
ng g c user-management --skip-tests
ng g c photo-management --skip-tests
```

```
cd client/src/app/_services
ng g s admin --skip-tests
```


valor-software.com/ngx-bootstrap/#/modals
```
ng add ngx-bootstrap --component modals
```

Add to shared.module.ts


```
cd client/src/app
mkdir modals
cd client/src/app/modals
ng g c roles-modal --skip-tests
```

Install npm package for client side v3.1.9
```
cd client
npm install @microsoft/signalr
```

```
cd client/src/app/_services
ng g s prescence --skip-tests
```

Most scalable method is to implement something like redis and use the dataset across many dotnet backends
https://redis.io/

We use a dictionary here but this is not scalable it wont work on multiple servers
Alternative could use a Database to share the information

```
[cisadmin@localhost SIS]$ cd client/src/app/_services/
[cisadmin@localhost _services]$ ng g s confirm --skip-tests
CREATE src/app/_services/confirm.service.ts (136 bytes)
[cisadmin@localhost _services]$ 
```

```
cd client/src/app/modals
ng g c confirm-dialog --skip-tests
```

```
ng build
```
angular.io/guide/deployment


https://angular.io/guide/deployment#production-optimizations
```
ng build --prod
```











```
cd client/src/app/docker
ng g c docker-container-view --skip-tests
ng g c docker-request-container --skip-tests
ng g c docker-admin-view --skip-tests

ng g c docker-container-status-dialog --skip-tests
```