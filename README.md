# BSE.Identity

Blazor Server implementation with [Fluent UI Blazor components](https://www.fluentui-blazor.net/) of a [Microsoft.AspNetCore.Identity](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-7.0&tabs=visual-studio) **User Management** application.

![Management](/docs/images/User%20Management_2023-11-04_09-55-28.gif)

## MySQL

## Precondition

you need a mysql server. Tested versions of the [community edition](https://dev.mysql.com/downloads/mysql/) are:
- 5.7.33
- 8.0.34

The used **Pomelo.EntityFrameworkCore.MySql** database provider needs the used database version.

```
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionStringBuilder.ConnectionString, new MySqlServerVersion(new Version(8, 0, 34)))
);
```

### Necessary user privileges

For database creation, we need the following provileges

- DBManager
- DBDesigner

and the custom privilege

- REFERENCES

![MySQL Privileges](/docs/images/MySQL-UserPrivileges.png)

### Database creation

If the database is not available, it will be created on first startup

### User Secrets

open a console navigate into the project directory and type 

```
dotnet user-secrets set mysql:server "yourdatabaseserver"
dotnet user-secrets set mysql:database "yourdatabase"
dotnet user-secrets set mysql:userid "userid"
dotnet user-secrets set mysql:password "password"
```

## Restrictions

### Localization
The [Data annotations localization](https://github.com/dotnet/aspnetcore/issues/12158) isn't supported in Blazor Server.


## Azure Keyvault

[Azure Key Vault configuration provider in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-7.0#use-application-id-and-x509-certificate-for-non-azure-hosted-apps)

### New-SelfSignedCertificate
Use Powershell to create a self-signed certificate,

```
$cert = New-SelfSignedCertificate -DnsName "identity.bsetunes.com" -CertStoreLocation "Cert:\CurrentUser\My"
```
set a password,
```
$pwd = ConvertTo-SecureString -String "passwordfrompwmanager" -Force -AsPlainText
```
and export the certifiate for later use
```
Export-PfxCertificate -Cert $cert -FilePath "C:\Certificates\localmachine\identity-bsetunes-com.pfx" -Password $pwd
```

Open the management console mmc.exe, select the certificate snap-in for the local computer, find the certificate you just created, 

- open the properties and remember the fingerprint.


![mmc.exe](/docs/images/self-signed-certificate-thumbprint.png)

- export the PKCS#12 archive (.pfx) certificate as a DER-encoded certificate (.cer).

![mmc.exe](/docs/images/self-signed-certificate-export.png)

### Azure Portal

- upload the created cer certificate to the appropriate app registristration in Azure

![Azure App Registration](/docs/images/azure-app-reg-upload-cer-cerificate.png)

- import the previously exported pfx certificate into the keyvault.

![Azure Keyvault](/docs/images/azure-keyvault-import-pfx-certficate.png)

- configure the access policy for the previosly created app registration in the keyvault.

![Azure Keyvault Access Policy](/docs/images/azure-keyvault-configure-access-policy.png)

![Azure Keyvault Access Principal](/docs/images/azure-keyvault-configure-access-policy-principal.png)