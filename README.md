# Backend

This is the repo for the .NET Core backend of our app

# Project Information

# Project Structure

### Running instructions

.NET 5 is used for the project, so make sure you have that

#### Short intro

After you clone the repo and run it, there are 2 requests in the swagger page: 
```GET WeatherForecast``` 
and 
```GET WeatherForecast/GetStuff```
The first one acts as an authentication: it can be accessed at any time, and it generates a JWT.
That JWT that is returned has to be added at the top right of the swagger page, on the "Authenticate" button, where you will be prompted to insert a bearer token (which is the JWT). You only have to add the JWT, not the `Bearer` word too in front. 
Afterwards, you can access the other request too, and it will not return 401 anymore, but instead it will return the proper page.

### IDE setup

- Make sure that you are using the `Code Cleanup On Save` extension, since we have an .editorconfig file that will be used for the automatic formatting

### File Structure

File structure:

- **Properties/** - contains the launchSettings for the app, which contains the settings and environment variables of the applcation.
	If some variable should be provided at startup to the application,
	then this is where it should be added, as an environment variable, under the `Development` profile environments
- **wwwroot/** - contains static resources of the application (such as images)
- **Attributes/** - directory for custom attributes
- **Controllers/** - directory that contains the REST controllers. There is already an example created, with JWT authorization (both creating and verifying a JWT token)
- **Middlewares/** - directory for custom middlewares.
- **Models/** - directory for the models of our app. I suggest splitting this into even more directories, as it would make sense (user models should go in a User directory, etc.)
- **Repos/** - directory for repositories. All of them will have interfaces and implementations, so please split the two accordingly
- **Utils/** - directory for other utilities.
- **.editorconfig** - file that contains code style rules
- **.gitignore** - I guess you know what this does
- **appsettings.json** - extra app settings. These will be loaded in the "Configuration" object, which can be accessed in Startup.cs and 
	every controller if it is injected (IConfiguration ...) like a dictionary (var hosts = Configuration["AllowedHosts"])
- **Program.cs** - the "main" of our program
- **README.md** - well.. hello there, this is me! :)
- **Startup.cs** - the class that is initialized in Program.cs, and where all the services and middlewares are defined. 
	This acts more like a "main" than Program.cs, since Program.cs only initializes this file and then this actually runs our server


Other directories/files can be created at will, as long as they have a well defined purpose

# Version Control

### Branches

- **main** - used for release versions
- **develop** - this is where you should be branching from, and creating pull requests to
- **'developer'/'jira-issue-code'/'issue-name-or-short-description'** - please name your branches this way :)

### Pull Requests, Reviews & Merging

- Active pull requests require at least **2** reviews approvals with at least **1** from a coordinator, although reviews
  from all people is highly advised.
- Reviews are made through comments on the pull request.
- When a comment is solved, it should be marked as resolved.
- Feature branches are merged into develop using the `Squash and merge` option.
