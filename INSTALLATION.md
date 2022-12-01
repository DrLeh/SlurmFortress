# Installation Guide

## Automatic install

1. Clone this repository
2. Compile and run /install/AppTemplateInstaller/AppTemplateInstaller.sln in Visual Studio or CLI
3. Enter the name of the new app. e.g. `NewApp`
4. If you know a name of an entity, provide it in Singular and Plural form in the next two prompts to get a scaffolded service for this type e.g. "Alert", "Alerts" to replace "MyEntity" and "MyEntities" references in the code.
5. Open new directory `D:\websites\NewApp`
6. Delete .git folder (hidden)
7. In command line type `git init` on the folder
8. Verify the code in visual studio. 
9. Commit to git as Initial Commit
10. Proceed to removing unneeded components from the solution (and from folders if necessary)

Done! You should have a microservice 

If something installed incorrectly, simply close the new solution, delete the contents of the new folder and try again.

## Feature / Component selection

This template comes with several components in place, although your application might not need all of them.

### Removing unneeded components

If your app doesn't need search index access:

1. Remove `MyApp.Algolia` project and delete any lines of code that now error.
2. Remove the Core/Search folder

If your app doesn't need dedicated database access:

1. Remove `MyApp.Data` project and delete any lines of code that now error.
2. Remove the Core/Data folder
3. Remove `Microsoft.EntityFrameworkCore.Design` package from web api project.

## Web API setup

1. In the web api project, change your `launchsettings.json` file to use a new IIS Express port
2. update MyApp.Web/wwwroot/index.html to have `<base href="/myapp">` to match the path for `asiservice.asicentral.com/myapp`
3. Update startup.cs to `app.UsePathBase("myapp");` to match
4. Update SwaggerExtensions.cs application description and href to `/myapp` path
