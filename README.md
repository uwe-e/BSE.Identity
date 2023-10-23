# BSE.Identity

## MySQL

### Necessary user privileges

For database creation, we need the following provileges

- DBManager
- DEBDesigner

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