# vue-core-jwt
Sample SPA application using Vue CLI, Asp.Net Core 3 API, with JWT for authentication. Also uses Vuetify,
Vee Validate, and includes a few other useful directives and utilities. This project does not use the Vue CLI Middleware package.

Features include:
* JWT tokens stored in a cookie with httponly
* In production client-side errors are sent to the backend for logging with Serilog
* 2 registration flows, with or without email confirmation
* password reset using email
* FontAwesome Free
* Vuetify (easy to swap for something else)
* Vee Validate
* Vuex store with user information
* Vue mixin passes common features through app
* Navigation guards for 3 levels of users: public, normal, admin (change to anything you like)
* Core API endpoint base classes for public or secure controllers
* ```CurrentUser``` available in secure controllers

**Both the API and Vue application are marked with ```// TODO:``` comments in places where you should customize the code for your own needs.** 

Uses a fake database and email service. There are a few demo values passed around as boilerplate for passing your own, like
an api key sent to the client for use with 3rd party services.

In production the /dist folder is served using the ```web.config``` settings. If you are trying to serving files
that end up redirecting to index.html by accident then edit the ```web.config``` file to exclude
those extensions from redirection.

The ESLint settings are a relaxed version of AirBnb + Vue rules, edit the ```.eslintrc.js``` file with your own preferences.

Change the ```ValidateEmail``` setting in appsettings.json to toggle the app between:
1) making new users confirm their email address before having access to the app
2) instantly approving the account and logging the new user in 

The login page includes buttons to test the features using data from the fake database.


## Computer setup

Change the JwtKey and other values in ```appsettings.json``` and ```appsettings.Development.json```

Install Vue CLI:

https://cli.vuejs.org/guide/installation.html

Assumed you have the .Net framework already installed.

Install the dotnet-watch utility. In a console run:

```dotnet tool install --global dotnet-watch --version 2.2.0```

If your linter or IDE complains about the ```@/foo``` paths in the JS files then you can
point the IDE to the webpack config file located in ```\VueApp\node_modules\@vue\cli-service\webpack.config.js```

## NPM Commands

**IMPORTANT**: Run all NPM commands from the /VueApp sub-folder. Skip the ```cd``` if you're already in there.

## Project setup
```
cd VueApp
npm install
```

I open two console windows, one for Vue and one for the API.

***Console 1: Start the backend API for development and watches for changes***
```
cd VueApp
npm run api
```

***Console 2: Compiles and hot-reloads for development***
```
cd VueApp
npm run serve
```

### Compiles and minifies for production
```
cd VueApp
npm run build
```

### Lints and fixes files
```
cd VueApp
npm run lint
```
## Vue Application

#### Mixin
```common-mixin.js``` includes some useful properties from the store and Vuetify. You can register this globally instead of
importing as-needed but third-party components like Vuetify will also get these properties and you might end up with naming
conflicts.

- ```currentUser``` = the object representing the current user or null if not signed-in
- ```signedIn``` = boolean of the user's sign-in state
- ```mobile``` = boolean, true if the page is in a [mobile layout size](https://vuetifyjs.com/en/customization/breakpoints/#breakpoint-service-object) 

#### Numeric Directives

- ```v-decimal```
- ```v-integer```

#### Icon Font

[Font Awesome](https://fontawesome.com/)

#### Validation

[Vee Validate](https://github.com/logaretm/vee-validate)

#### UI Framework

[Vuetify](https://vuetifyjs.com)

#### Toast Notifications

[Vue Toastification](https://github.com/Maronato/vue-toastification)

## References

1) https://vuetifyjs.com
1) https://github.com/logaretm/vee-validate
1) https://github.com/Maronato/vue-toastification
1) https://fontawesome.com/
1) https://cli.vuejs.org/
1) https://weblog.west-wind.com/posts/2017/Apr/27/IIS-and-ASPNET-Core-Rewrite-Rules-for-Static-Files-and-Html-5-Routing
1) https://github.com/neonbones/Boilerplate.AuthDemo
1) https://gist.github.com/jonasraoni/9dea65e270495158393f54e36ee6b78d

## Change Log

### 2020-07-21

- Edited the project file to hide the dist folder but still publish it

### 2020-07-18

- Replaced the global mixin with a separate class for as-needed use
- Use a default User object in the store for easier resetting
- Added a lot of documentation
- Added ```TODO:``` comments everywhere you might want to customize code for your needs
- Send a user to their original route after login if they were redirected to the login page
- Moved the cookie name into appsetting.json
