# BSE.Identity

Blazor Server implementation with [Fluent UI Blazor components](https://www.fluentui-blazor.net/) of a [Microsoft.AspNetCore.Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio) **User Management** application.

![Management](/docs/images/User%20Management_2023-11-04_09-55-28.gif)

## MySQL

### Necessary user privileges

For database creation, we need the following provileges

- DBManager
- DBDesigner

and the custom privilege

- REFERENCES

![MySQL Privileges](/docs/images/MySQL-UserPrivileges.png)

### Database creation

Open the Package Manager Console

and type **update-database** in.

![update-database](/docs/images/update-database.png)

### User Secrets

open a console navigate into the project directory and type 

```
dotnet user-secrets set ConnectionStrings:DefaultConnection "Server=localhost;Database=users123....."
```

## Restrictions

### Localization
The [Data annotations localization](https://github.com/dotnet/aspnetcore/issues/12158) isn't supported in Blazor Server.

### Fluent UI Components
Using of <FluentCheckbox/> on Fluent Dialogs and Panels is a bit wonky




